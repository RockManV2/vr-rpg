
using System;
using System.Collections.Generic;
using Oculus.Voice;
using UnityEngine;

public class SpellHandler : MonoBehaviour
{
    [SerializeField] private AppVoiceExperience _appVoiceExperience;
    private VoiceHandler _voiceHandler;

    public readonly Dictionary<string, Action<string>> Spells = new();

    private void Awake()
    {
        _appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener(TriggerSpellCast);
        _voiceHandler = new VoiceHandler();
    }

    private void TriggerSpellCast(string phrase)
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
