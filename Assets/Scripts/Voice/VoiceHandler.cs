using System;
using Oculus.Voice;
using UnityEngine;

public class VoiceHandler
{
    private AppVoiceExperience _appVoiceExperience;
    
    private readonly string[] _modifiers =
    {
        "greater",
        "lesser"
    };
    
    private readonly string[] _spellTypes =
    {
        "fireball",
        "water ball",
        "earth shard",
        "air blast",
        "sonic boom",
    };

    public PhraseData ProcessPhrase(string phrase)
    {
        phrase = phrase.ToLower();
        
        string modifier = String.Empty;
        string spellType = String.Empty;

        foreach (string m in _modifiers)
            if (phrase.Contains(m))
                modifier = m;
        
        foreach (string s in _spellTypes)
            if (phrase.Contains(s))
                spellType = s;

        return new PhraseData(modifier, spellType);
    }
}

public struct PhraseData
{
    public string modifier;
    public string spellType;
    
    public bool IsValid => spellType.Length != 0;
    public bool HasModifier => modifier.Length != 0;
    
    public PhraseData(string pModifier, string pSpellType)
    {
        modifier = pModifier;
        spellType = pSpellType;
    }
}