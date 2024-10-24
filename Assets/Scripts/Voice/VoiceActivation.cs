
using Oculus.Voice;
using UnityEngine;
using UnityEngine.InputSystem;

public class VoiceActivation : MonoBehaviour
{
    [SerializeField] private AppVoiceExperience _appVoiceExperience;

    private void StartRecording(InputAction.CallbackContext _)
    {
        SoundController.PlaySound("invalidcast");
        _appVoiceExperience.Activate();
    }
}
