using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using GCRuntime.Dialogue;

public class DialogueTreeEditor : EditorWindow
{
    private DialogueTreeView TreeView;
    private InspectorView InspectorView;

    [MenuItem("GC/Runtime/Dialogue Tree Editor")]
    public static void OpenWindow()
    {
        DialogueTreeEditor wnd = GetWindow<DialogueTreeEditor>();
        wnd.titleContent = new GUIContent("DialogueTreeEditor");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        if (Selection.activeObject is DialogueTree)
        {
            OpenWindow();
            return true;
        }
        return false;
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/GCRuntime/Editor/DialogueTreeEditor/DialogueTreeEditor.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/GCRuntime/Editor/DialogueTreeEditor/DialogueTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        TreeView = root.Q<DialogueTreeView>();
        InspectorView = root.Q<InspectorView>();
        TreeView.OnNodeSelected = OnNodeSelectionChange;
        OnSelectionChange();
    }

    private void OnSelectionChange()
    {
        DialogueTree Tree = Selection.activeObject as DialogueTree;
        if (Tree != null && AssetDatabase.CanOpenAssetInEditor(Tree.GetEntityId()))
        {
            TreeView.PopulateView(Tree);
        }
    }

    private void OnNodeSelectionChange(DialogueNodeView nodeView)
    {
        InspectorView.UpdateSelection(nodeView);
    }
}
