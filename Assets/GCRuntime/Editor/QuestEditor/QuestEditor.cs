using System;
using System.Collections.Generic;
using System.Linq;
using GCRuntime.QuestSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestEditor : EditorWindow
{
    /// <summary>
    /// ScriptObject资源
    /// </summary> 
    private QuestContainer Container;

    /// <summary>
    /// 资源列表
    /// </summary> 
    private List<QuestInfo> QuestList;

    /// <summary>
    /// 列表Item模板
    /// </summary> 
    /// </summary>
    private VisualTreeAsset QuestRowTemplate;

    /// <summary>
    /// 窗口下的listView
    /// </summary> 
    private ListView QuestListView;

    /// <summary>
    /// 所有的编辑窗口
    /// </summary>
    private InspectorView Inspector;

    /// <summary>
    /// 当前激活的任务
    /// </summary>
    private QuestInfo ActiveQuest;

    /// <summary>
    /// 添加按钮
    /// </summary> 
    private Button AddButton;

    /// <summary>
    /// 移除按钮
    /// </summary> 
    private Button RemoveButton;

    /// <summary>
    /// 按钮的名称
    /// </summary> 
    private TextField QuestName;

    /// <summary>
    /// 所创建的文本
    /// </summary> 
    private Label QuestLabel;
    
    /// <summary>
    /// 打开窗口
    /// </summary> 
    [MenuItem("GC/Runtime/Quest Editor")]
    public static void OpenWindow()
    {
        QuestEditor wnd = GetWindow<QuestEditor>();
        wnd.titleContent = new GUIContent("QuestEditor");
    }

    /// <summary>
    /// 创建GUI
    /// </summary> 
    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/GCRuntime/Editor/QuestEditor/QuestEditor.uxml");
        visualTree.CloneTree(root);
        
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/GCRuntime/Editor/QuestEditor/QuestEditor.uss");
        root.styleSheets.Add(styleSheet);
        QuestListView = root.Q<ListView>();
        Inspector = root.Q<InspectorView>();
        QuestRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/GCRuntime/Editor/QuestEditor/QuestRowTemplate.uxml");

        AddButton = root.Q<Button>("Add");
        RemoveButton = root.Q<Button>("Remove");
        QuestName = root.Q<TextField>();
        QuestLabel = root.Q<Label>("QuestName");
        Container = AssetDatabase.LoadAssetAtPath<QuestContainer>("Assets/Resources/QuestContainer.asset");
        EditorUtility.SetDirty(Container);
        AssetDatabase.SaveAssets();
        QuestList = Container.QuestInfos;

        BindButtonEvent();
        GenerateListView();
    }

    private void GenerateListView()
    {
        Func<VisualElement> makeItem = () => QuestRowTemplate.CloneTree();

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            if (i < QuestList.Count)
            {
                e.Q<Label>().text = QuestList[i].QuestName;
            }
        };
        QuestListView.itemsSource = QuestList;
        QuestListView.makeItem = makeItem;
        QuestListView.bindItem = bindItem;  
        QuestListView.Rebuild();
        QuestListView.selectionChanged += OnSelectionChanged;
    }

    private void BindButtonEvent()
    {
        if (AddButton != null)
        {
            AddButton.clicked += () =>
            {
                CreateQuestInfo();
                RefreshListView();
            };
        }
        if (RemoveButton != null)
        {
            RemoveButton.clicked += () =>
            {
                DeleteQuestInfo();
                RefreshListView();
            };
        }
    }

    private void RefreshListView()
    {
        QuestList = Container.QuestInfos;
        QuestListView.itemsSource = QuestList;
        QuestListView.Rebuild();
        if (Inspector != null)
        {
            Inspector.Clear();
        }
        ActiveQuest = null;
    }

    private void OnSelectionChanged(IEnumerable<object> selectItem)
    {
        Inspector.UpdateSelection((QuestInfo)selectItem.First());
        ActiveQuest = (QuestInfo)selectItem.First();
        QuestLabel.text = ActiveQuest.QuestName;
    }

    public void CreateQuestInfo()
    {
        if (string.IsNullOrEmpty(QuestName.text))
        {
            Debug.LogError("[GCRuntime]: 创建前请先定义名称!");
            return;
        }
        QuestInfo questInfo = ScriptableObject.CreateInstance("QuestInfo") as QuestInfo;
        questInfo.QuestName = QuestName.text;
        Container.AddQuestInfo(questInfo);
        AssetDatabase.AddObjectToAsset(questInfo, Container);
        AssetDatabase.SaveAssets();
    }

    public void DeleteQuestInfo()
    {
        if (ActiveQuest == null) return;
        AssetDatabase.RemoveObjectFromAsset(ActiveQuest);
        AssetDatabase.SaveAssets();
        Container.RemoveQuest(ActiveQuest);
    }
}
