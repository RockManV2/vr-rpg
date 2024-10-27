
using System;
using UnityEngine;

[Serializable]
public struct DialogueNodeData
{
    public string Guid;
    public NodeType NodeType;
    public string DialogueText;
    public Quest Quest;
    public Vector2 Position;
}
