
using Oculus.Voice;
using UnityEngine;
using UnityEngine.Events;

public class VoiceManager : MonoBehaviour
{
    public static VoiceManager Instance;

    [SerializeField] private AppVoiceExperience _appVoiceExperience;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RequestPlayerVoice(UnityAction<string> callback)
    {
        if (_appVoiceExperience.Active) return;
        
        _appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener(callback);

        _appVoiceExperience.VoiceEvents.OnRequestCompleted.AddListener(
            _appVoiceExperience.VoiceEvents.OnFullTranscription.RemoveAllListeners);

        _appVoiceExperience.Activate();
    }
}