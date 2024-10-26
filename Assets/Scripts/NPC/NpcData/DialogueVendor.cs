
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class DialogueVendor : MonoBehaviour
{
    public string Guid;
    
    private DialogueContainer dialogueContainer;
    public DialogueContainer Dialogue => Resources.Load<DialogueContainer>($"Dialogue/{Guid}");
}
