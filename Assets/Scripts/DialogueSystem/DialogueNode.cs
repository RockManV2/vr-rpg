
using UnityEditor.Experimental.GraphView;

public class DialogueNode : Node
{
    public string Guid;
    public NodeType NodeType;
    
    public string DialogueText;
    public Quest Quest;
    
    public bool EntryPoint;
}

public enum NodeType
{
    Undefined,
    Dialogue,
    Quest,
    Execution,
}
