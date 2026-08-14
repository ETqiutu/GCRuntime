using GCRuntime.Dialogue;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System;

public class DialogueNodeView : Node
{
    public Action<DialogueNodeView> OnNodeSelected;
    public DialogueNode node;
    public Port Input;
    public Port Output;

    public DialogueNodeView(DialogueNode node)
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
        if (node is DialogueEntry)
        {
            Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is DialogueBranch)
        {
            Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is DialogueRoot)
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
        if (node is DialogueEntry)
        {
            Output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        else if (node is DialogueBranch)
        {
            Output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if (node is DialogueRoot)
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
