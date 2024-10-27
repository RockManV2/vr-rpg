
using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class DialogueVendor : MonoBehaviour
{
    private string _guid;
    public string Guid => _guid;
    
    private DialogueContainer dialogueContainer;
    public DialogueContainer Dialogue => Resources.Load<DialogueContainer>($"Dialogue/{Guid}");

    private void Reset()
    {
        _guid = System.Guid.NewGuid().ToString();
    }
    
    
}
