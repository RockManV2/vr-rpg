
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Oculus.Voice;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellHandler : MonoBehaviour
{
    private VoiceHandler _voiceHandler;
    [SerializeField] private PlayerInput _playerInput;
    
    public readonly Dictionary<string, Action<string>> Spells = new();

    private void Awake()
    {
        _voiceHandler = new VoiceHandler();
    }

    private void OnEnable()
    {
        _playerInput.actions["Voice"].performed += CastSpell;
    }
    
    private void OnDisable()
    {
        _playerInput.actions["Voice"].performed -= CastSpell;
    }

    private async void CastSpell(InputAction.CallbackContext _)
    {
        Debug.Log("CastSpell Started");
        var phraseTask = VoiceManager.Instance.GetVoiceTextAsync();
        var phrase = await Task.WhenAll(phraseTask);

        Debug.Log("voiceline regocnized");
        Debug.Log(phrase[0]);
        
        PhraseData phraseData = _voiceHandler.ProcessPhrase(phrase[0]);

        if (!phraseData.IsValid)
        {
            SoundController.PlaySound("invalidcast");
            return;
        }
        
        Spells[phraseData.spellType].Invoke(phraseData.modifier);
    }
}
