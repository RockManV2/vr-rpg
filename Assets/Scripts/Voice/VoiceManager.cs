using System.Threading.Tasks;
using Oculus.Voice;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    public static VoiceManager Instance;

    [SerializeField] private AppVoiceExperience _appVoiceExperience;

    private string _processedText = string.Empty;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener(GetVoiceText);
    }

    private void GetVoiceText(string phrase)
    {
        _processedText = phrase;
    }
    
    public async Task<string> GetVoiceTextAsync()
    {
        _appVoiceExperience.Activate();

        await Task.Run(() => {
            while (string.IsNullOrEmpty(_processedText))
            {
                Debug.Log("getting voiceline");
            }
        });

        string result = _processedText;
        
        _processedText = string.Empty;
        
        return result;
    }
}