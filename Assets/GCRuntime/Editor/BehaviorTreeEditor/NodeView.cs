using GCRuntime.BTree;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System;

public class NodeView : Node
{
    public Action<NodeView> OnNodeSelected;
    public BTNode node;
    public Port Input;
    public Port Output;

    public NodeView(BTNode node)
    {
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.Guid;
        style.left = node.Position.x;
        style.top = node.Position.y;

        CreateInputPort();
        CreateOutputPort();
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        node.Position.x = newPos.xMin;
        node.Position.y = newPos.yMin;
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null)
        {
            OnNodeSelected?.Invoke(this);
        }
    }

    public void CreateInputPort()
    {
        if (node is ActionNode)
        {
            Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is CompositeNode)
        {
            Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is DecoratorNode)
        {
            Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is RootNode)
        {
        }

        if (Input != null)
        {
            Input.portName = "";
            inputContainer.Add(Input);
        }
    }
    
    public void CreateOutputPort()
    {
        if (node is ActionNode)
        {
            
        }
        else if (node is CompositeNode)
        {
            Output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if (node is DecoratorNode)
        {
            Output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        else if (node is RootNode)
        {
            Output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        }

        if (Output != null)
        {
            Output.portName = "";
            outputContainer.Add(Output);
        }
    }
}
