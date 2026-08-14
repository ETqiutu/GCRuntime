using UnityEditor;
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
}
