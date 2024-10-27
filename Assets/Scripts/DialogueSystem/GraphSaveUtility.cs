using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSaveUtility
{
    private DialogueGraphView _targetGraphView;
    private DialogueContainer _containerCache;

    private List<Edge> _edges => _targetGraphView.edges.ToList();
    private List<DialogueNode> _nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();

    public static GraphSaveUtility GetInstance(DialogueGraphView targetGrapghView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGrapghView
        };
    }

    public void SaveGraph(string fileName)
    {
        if (!_edges.Any())
            return;

        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

        var connectedPorts = _edges.Where(x => x.input.node != null).ToArray();
        for (var i = 0; i < connectedPorts.Length; i++)
        {
            var edge = connectedPorts[i];
            var outputNode = edge.output.node as DialogueNode;
            var inputNode = edge.input.node as DialogueNode;
            dialogueContainer.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGuid = outputNode.Guid,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.Guid,
            });
        }

        foreach (var dialogueNode in _nodes.Where(node => !node.EntryPoint))
        {
            dialogueContainer.DialogueNodeData.Add(new DialogueNodeData
            {
                Guid = dialogueNode.Guid,
                NodeType = dialogueNode.NodeType,
                DialogueText = dialogueNode.DialogueText,
                Quest = dialogueNode.Quest,
                Position = dialogueNode.GetPosition().position
            });
        }

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");

        if (!AssetDatabase.IsValidFolder("Assets/Resources/Dialogue"))
            AssetDatabase.CreateFolder("Assets/Resources", "Dialogue");

        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/Dialogue/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadGraph(string fileName)
    {
        _containerCache = Resources.Load<DialogueContainer>($"Dialogue/{fileName}");
        if (_containerCache == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file does net exist.", "OK");
            return;
        }

        ClearGraph();
        GenerateNodes();
        ConnectNodes();
    }

    private void ConnectNodes()
    {
        for (var i = 0; i < _nodes.Count; i++)
        {
            var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == _nodes[i].Guid).ToList();

            for (var j = 0; j < connections.Count(); j++)
            {
                var targetNodeGuid = connections[j].TargetNodeGuid;
                var targetNode = _nodes.First(x => x.Guid == targetNodeGuid);
                LinkNodes(_nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                targetNode.SetPosition(
                    new Rect(_containerCache.DialogueNodeData.First(x => x.Guid == targetNodeGuid).Position,
                        _targetGraphView.DefaultNodeSize
                    )
                );
            }
        }
    }

    private void LinkNodes(Port output, Port input)
    {
        var tempEdge = new Edge
        {
            output = output,
            input = input,
        };

        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);
        _targetGraphView.Add(tempEdge);
    }

    private void GenerateNodes()
    {
        foreach (var nodeData in _containerCache.DialogueNodeData)
        {
            DialogueNode tempNode = null;
            switch (nodeData.NodeType)
            {
                case NodeType.Dialogue:
                {
                    tempNode = _targetGraphView.CreateDialogueNode(nodeData.DialogueText);
                    var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
                    nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.PortName));
                    break;
                }
                case NodeType.Quest:
                    tempNode = _targetGraphView.CreateQuestNode(nodeData.DialogueText, nodeData.Quest);
                    break;
            }
            
            tempNode.Guid = nodeData.Guid;
            _targetGraphView.AddElement(tempNode);
        }
    }

    private void ClearGraph()
    {
        if(_containerCache.NodeLinks.Count > 0)
            _nodes.Find(x => x.EntryPoint).Guid = _containerCache.NodeLinks[0].BaseNodeGuid;

        foreach (var node in _nodes)
        {
            if (node.EntryPoint)
                continue;

            _edges.Where(x => x.input.node == node).ToList()
                .ForEach(edge => _targetGraphView.RemoveElement(edge));

            _targetGraphView.RemoveElement(node);
        }
    }
}