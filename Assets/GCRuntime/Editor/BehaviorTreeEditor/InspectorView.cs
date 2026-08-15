using GCRuntime.QuestSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class InspectorView : VisualElement
{
    private Editor editor;

    public InspectorView()
    {
        
    }

    internal void UpdateSelection(NodeView nodeView)
    {
        Clear();
        UnityEngine.Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(nodeView.node);
        IMGUIContainer container = new IMGUIContainer(() =>
        {
            editor.OnInspectorGUI();
        });
        Add(container);
    }

    internal void UpdateSelection(DialogueNodeView nodeView)
    {
        Clear();
        UnityEngine.Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(nodeView.node);
        IMGUIContainer container = new IMGUIContainer(() =>
        {
            editor.OnInspectorGUI();
        });
        Add(container);
    }

    internal void UpdateSelection(QuestInfo questInfo)
    {
        Clear();
        if (questInfo == null) return;
        Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(questInfo);
        IMGUIContainer container = new IMGUIContainer(() =>
        {
            editor.OnInspectorGUI();
        });
        Add(container);
    }
}
