
using Oculus.Voice;
using UnityEngine;
using UnityEngine.InputSystem;

public class VoiceActivation : MonoBehaviour
{
    [SerializeField] private AppVoiceExperience _appVoiceExperience;
    [SerializeField] private PlayerInput _playerInput;
    
    private void Start()
    {
        _playerInput.actions["Voice"].performed += StartRecording;
    }

    private void StartRecording(InputAction.CallbackContext _)
    {
        _appVoiceExperience.Activate();
    }
}
