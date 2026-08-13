using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using GCRuntime.BTree;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[UxmlElement] 
public partial class BehaviorTreeView : GraphView
{
    private BTree Tree;
    public Action<NodeView> OnNodeSelected;

    public BehaviorTreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/GCRuntime/Editor/BehaviorTreeEditor/BehaviorTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }

    internal void PopulateView(BTree Tree)
    {
        this.Tree = Tree;
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        if (Tree.Root == null)
        {
            Tree.Root = Tree.CreateNode(typeof(RootNode)) as RootNode;
            EditorUtility.SetDirty(Tree);
            AssetDatabase.SaveAssets();
        }

        Tree.Nodes.ForEach(n => CreateNodeView(n));
        Tree.Nodes.ForEach(n =>
        {
            var children = Tree.GetChildren(n);
            children.ForEach(c =>
            {
                NodeView parentView = FindNodeView(n);
                NodeView childView = FindNodeView(c);
                Edge edge = parentView.Output.ConnectTo(childView.Input);
                AddElement(edge);
            });
        });
    }

    private NodeView FindNodeView(BTNode node)
    {
        return GetNodeByGuid(node.Guid) as NodeView;
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
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    Tree.DeleteNode(nodeView.node);
                } 

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    Tree.RemoveChild(parentView.node, childView.node);
                }
            });
        }

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edges =>
            {
                NodeView parentView = edges.output.node as NodeView;
                NodeView childView = edges.input.node as NodeView;
                Tree.AddChild(parentView.node, childView.node);
            });
        }
        return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types)
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var type in types)
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types)
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
        }
    }

    private void CreateNode(Type type)
    {
        if (Tree == null)
        {
            Debug.LogError("Cannot create node: No BTree asset is currently loaded. Please select a BTree asset in the Project window first.");
            return;
        }
        BTNode node = Tree.CreateNode(type);
        CreateNodeView(node);
    }

    private void CreateNodeView(BTNode node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }
}
