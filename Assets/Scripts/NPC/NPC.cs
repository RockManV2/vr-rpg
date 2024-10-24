
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public delegate NpcEvent NpcEvent();
    public NpcEvent OnDialogueStarted;
    public NpcEvent OnDialogueEnded;

    [SerializeField] private DialogueData[] _dialogue;

    public void Interact()
    {
        OnDialogueStarted?.Invoke();
    }

    private void PlayDialogue()
    {
        foreach (DialogueData data in _dialogue)
        {
            ShowDialogue(data.Dialogue);
        }
    }

    private void ShowDialogue(string text)
    {
        
    }
}
