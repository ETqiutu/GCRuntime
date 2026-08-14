using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System;
using GCRuntime.Dialogue;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[UxmlElement] 
public partial class DialogueTreeView : GraphView
{
    private DialogueTree Tree;
    public Action<DialogueNodeView> OnNodeSelected;

    public DialogueTreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/GCRuntime/Editor/DialogueTreeEditor/DialogueTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }

    internal void PopulateView(DialogueTree Tree)
    {
        this.Tree = Tree;
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        if (Tree.Root == null)
        {
            Tree.Root = Tree.CreateNode(typeof(DialogueRoot)) as DialogueRoot;
            EditorUtility.SetDirty(Tree);
            AssetDatabase.SaveAssets();
        }

        Tree.Nodes.ForEach(n => CreateNodeView(n));
        Tree.Nodes.ForEach(n =>
        {
            var children = Tree.GetChildren(n);
            
            children.ForEach(c =>
            {
                DialogueNodeView parentView = FindNodeView(n);
                DialogueNodeView childView = FindNodeView(c);
                Edge edge = parentView.Output.ConnectTo(childView.Input);
                AddElement(edge);
            });
            
        });
    }

    private DialogueNodeView FindNodeView(DialogueNode node)
    {
        return GetNodeByGuid(node.Guid) as DialogueNodeView;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort =>
            endPort.direction != startPort.direction && 
            endPort.node != startPort.node).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                DialogueNodeView nodeView = elem as DialogueNodeView;
                if (nodeView != null)
                {
                    Tree.DeleteNode(nodeView.node);
                } 

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    DialogueNodeView parentView = edge.output.node as DialogueNodeView;
                    DialogueNodeView childView = edge.input.node as DialogueNodeView;
                    Tree.RemoveChild(parentView.node, childView.node);
                }
            });
        }

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edges =>
            {
                DialogueNodeView parentView = edges.output.node as DialogueNodeView;
                DialogueNodeView childView = edges.input.node as DialogueNodeView;
                Tree.AddChild(parentView.node, childView.node);
            });
        }
        return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        {
            evt.menu.AppendAction("DialogueEntry", (a) => CreateNode(typeof(DialogueEntry)));
        }

        {
            evt.menu.AppendAction("DialogueBranch", (a) => CreateNode(typeof(DialogueBranch)));
        }
    }

    private void CreateNode(Type type)
    {
        if (Tree == null)
        {
            Debug.LogError("Cannot create node: No BTree asset is currently loaded. Please select a BTree asset in the Project window first.");
            return;
        }
        DialogueNode node = Tree.CreateNode(type);
        CreateNodeView(node);
    }

    private void CreateNodeView(DialogueNode node)
    {
        DialogueNodeView nodeView = new DialogueNodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }
}
