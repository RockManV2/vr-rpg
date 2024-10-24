
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Oculus.Voice;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SpellHandler : MonoBehaviour
{
    private VoiceHandler _voiceHandler;
    [SerializeField] private PlayerInput _playerInput;
    
    public readonly Dictionary<string, Action<string>> Spells = new();

    public delegate void PhraseProcessedCallback(string phrase);
    
    private void Awake()
    {
        _voiceHandler = new VoiceHandler();
    }

    private void OnEnable()
    {
        _playerInput.actions["Voice"].performed += OnCastSpellPressed;
    }
    
    private void OnDisable()
    {
       _playerInput.actions["Voice"].performed -= OnCastSpellPressed;
    }
    
    private void OnCastSpellPressed(InputAction.CallbackContext _)
    {
        VoiceManager.Instance.RequestPlayerVoice(CastSpell);
    }
    
    private void CastSpell(string phrase)
    {
        PhraseData phraseData = _voiceHandler.ProcessPhrase(phrase);

        if (!phraseData.IsValid)
        {
            SoundController.PlaySound("invalidcast");
            return;
        }
        
        Spells[phraseData.spellType].Invoke(phraseData.modifier);
    }
}
