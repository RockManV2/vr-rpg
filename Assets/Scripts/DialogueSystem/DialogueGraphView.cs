
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    public readonly Vector2 DefaultNodeSize;
    
    public DialogueGraphView()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("Editor/DialogueGraph"));
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();
        
        AddElement(GenerateEntryPointNode());
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        ports.ForEach((port) =>
        {
            if(startPort != port && startPort.node != port.node)
                compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }

    private Port GeneratePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }
    
    private DialogueNode GenerateEntryPointNode()
    {
        var dialogueNode = new DialogueNode
        {
            title = "START",
            Guid = Guid.NewGuid().ToString(),
            DialogueText = "ENTRYPOINT",
            EntryPoint = true,
        };

        var generatedPort = GeneratePort(dialogueNode, Direction.Output);
        generatedPort.portName = "Next";
        dialogueNode.outputContainer.Add(generatedPort);

        dialogueNode.capabilities -= Capabilities.Movable;
        dialogueNode.capabilities -= Capabilities.Deletable;
        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(100, 200, 100, 150));
        
        return dialogueNode;
    }

    public void AddDialogueNode(string nodeName) => AddElement(CreateDialogueNode(nodeName));
    public void AddQuestNode(string nodeName) => AddElement(CreateQuestNode(nodeName));

    
    public DialogueNode CreateDialogueNode(string nodeName)
    {
        var dialogueNode = new DialogueNode
        {
            title = nodeName,
            NodeType = NodeType.Dialogue,
            DialogueText = nodeName,
            Guid = Guid.NewGuid().ToString(),
        };

        var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        dialogueNode.inputContainer.Add(inputPort);
        
        dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Editor/Node"));
        
        var button = new Button(() => { AddChoicePort(dialogueNode); });
        button.text = "New Choice";
        dialogueNode.titleContainer.Add(button);

        var textField = new TextField(string.Empty);
        textField.RegisterValueChangedCallback(evt =>
        {
            dialogueNode.DialogueText = evt.newValue;
            dialogueNode.title = evt.newValue;
        });
        textField.SetValueWithoutNotify(dialogueNode.title);
        dialogueNode.mainContainer.Add(textField);
        
        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        
        dialogueNode.SetPosition(new Rect(Vector2.zero, DefaultNodeSize));

        return dialogueNode;
    }
    
    public DialogueNode CreateQuestNode(string nodeName, Quest quest = null)
    {
        var dialogueNode = new DialogueNode
        {
            title = nodeName,
            DialogueText = "Quest Node",
            NodeType = NodeType.Quest,
            Quest = quest,
            Guid = Guid.NewGuid().ToString(),
        };

        var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        dialogueNode.inputContainer.Add(inputPort);
        
        dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Editor/QuestNode"));
        
        var generatedPort = GeneratePort(dialogueNode, Direction.Output);
        generatedPort.portName = "Next";
        dialogueNode.outputContainer.Add(generatedPort);

        var questField = new ObjectField() { objectType = typeof(Quest)};
        questField.SetValueWithoutNotify(quest);
        questField.RegisterValueChangedCallback(evt =>
        {
            dialogueNode.Quest = (Quest)questField.value;
        });
        dialogueNode.mainContainer.Add(questField);
        
        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        
        dialogueNode.SetPosition(new Rect(Vector2.zero, DefaultNodeSize));
       
        return dialogueNode;
    }

    public void AddChoicePort(DialogueNode graphNode, string overridenPortName = "")
    {
        var generatedPort = GeneratePort(graphNode, Direction.Output);

        var oldLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(oldLabel);
        
        var outputPortCount = graphNode.outputContainer.Query("connector").ToList().Count;

        var choicePortName = string.IsNullOrEmpty(overridenPortName)
            ? $"Choice {outputPortCount + 1}"
            : overridenPortName;
        
        var textField = new TextField
        {
            name = string.Empty,
            value = choicePortName,
        };
        
        textField.style.maxWidth = 0;
        textField.style.minWidth = 75;
        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
        
        generatedPort.contentContainer.Add(new Label("  "));
        generatedPort.contentContainer.Add(textField);
        var deleteButton = new Button(() => RemovePort(graphNode, generatedPort))
        {
            text = "X",
        };
        generatedPort.contentContainer.Add(deleteButton);

        generatedPort.portName = choicePortName;
        graphNode.outputContainer.Add(generatedPort);
        graphNode.RefreshExpandedState();
        graphNode.RefreshPorts();
    }

    private void RemovePort(DialogueNode graphNode, Port generatedPort)
    {
        var targetEdge = edges.ToList()
            .Where(x => x.output.portName == generatedPort.portName && x.output.node == graphNode);

        if (targetEdge.Any())
        {
            var edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }
        graphNode.outputContainer.Remove(generatedPort);
        graphNode.RefreshPorts();
        graphNode.RefreshExpandedState();
    }
}
