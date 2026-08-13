using GCRuntime.BTree;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;

public class BehaviorTreeEditor : EditorWindow
{
    private BehaviorTreeView TreeView;
    private InspectorView InspectorView;

    [MenuItem("GC/Runtime/Behavior Tree Editor")]
    public static void OpenWindow()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditor");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        if (Selection.activeObject is BTree)
        {
            OpenWindow();
            return true;
        }
        return false;
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/GCRuntime/Editor/BehaviorTreeEditor/BehaviorTreeEditor.uxml");
        visualTree.CloneTree(root);
        
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/GCRuntime/Editor/BehaviorTreeEditor/BehaviorTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        TreeView = root.Q<BehaviorTreeView>();
        InspectorView = root.Q<InspectorView>();
        TreeView.OnNodeSelected = OnNodeSelectionChange;
        OnSelectionChange();
    }

    private void OnSelectionChange()
    {
        BTree Tree = Selection.activeObject as BTree;
        if (Tree != null && AssetDatabase.CanOpenAssetInEditor(Tree.GetEntityId()))
        {
            TreeView.PopulateView(Tree);
        }
    }

    private void OnNodeSelectionChange(NodeView nodeView)
    {
        InspectorView.UpdateSelection(nodeView);
    }
}
