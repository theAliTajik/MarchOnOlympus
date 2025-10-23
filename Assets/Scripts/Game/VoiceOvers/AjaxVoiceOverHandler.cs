
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AjaxVoiceOverHandler
{
    [SerializeField] private List<AjaxVoiceOverData> m_voiceOvers;
    
    public void Configure(Ajax ajax)
    {
        SubscribeToEvents();
        SubscribeToPlayerEvents();
        SubscribeToAjaxEvents(ajax);
    }

    private void SubscribeToEvents()
    {
        GameplayEvents.GamePhaseChanged += HandleGamePhase;
    }

    public void SubscribeToAjaxEvents(Ajax ajax)
    {
        ajax.HP.OnPercentageTrigger += HandleOnAjaxPercentTriggered;
    }
    
    public void SubscribeToPlayerEvents()
    {
        var player = GameInfoHelper.GetPlayer();
        player.Death += _ => HandleOnDeath();
    }

    private void HandleOnAjaxPercentTriggered(FighterHP.TriggerPercentage Trigger)
    {
        int percentage = (int)(Trigger.Percentage*100); // 0.66 => 66
        CustomDebug.Log($"Trying to handle Ajax precentage: {percentage}", Categories.VoiceOver.Root);
        if (percentage == 66)
        {
            HandleOn66Percent();
        }

        if (percentage == 33)
        {
            HandleOn33Percent();
        }
    }

    private void HandleGamePhase(EGamePhase phase)
    {
        if(phase != EGamePhase.CARD_DRAW_FINISHED) return;
        HandleOnEntry();
        GameplayEvents.GamePhaseChanged -= HandleGamePhase;
    }

    public void HandleOnDeath()
    {
        PlayVoiceOverOfTriggerType(VoiceOverTriggerType.OnPlayerDeath);
    }

    public void HandleOnEntry()
    {
        PlayVoiceOverOfTriggerType(VoiceOverTriggerType.OnCombatEntry);
    }


    public void HandleOn66Percent()
    {
        PlayVoiceOverOfTriggerType(VoiceOverTriggerType.OnPlayer66Percent);
    }

    public void HandleOn33Percent()
    {
        PlayVoiceOverOfTriggerType(VoiceOverTriggerType.OnPlayer33Percent);
    }
    
    private AjaxVoiceOverData GetVoiceOverTargetFighterVoiceOver(List<AjaxVoiceOverData> voiceOvers, Fighter currentFighter)
    {
        if (voiceOvers.Count == 0)
        {
            CustomDebug.LogWarning("VoiceOver count is 0", Categories.VoiceOver.Root);
            return null;
        }

        if (currentFighter == null)
        {
            CustomDebug.LogWarning("CurrentFighter is null", Categories.VoiceOver.Root);
        }
        
        var NoTargetVoiceOver = voiceOvers.Find(voiceOver => voiceOver.TargetFighter is null);
        var TargetVoiceOver = voiceOvers.Find(voiceover => voiceover.TargetFighter?.GetType() == currentFighter.GetType());

        if (TargetVoiceOver == null)
        {
            return NoTargetVoiceOver;
        }

        return TargetVoiceOver;
    }


    private List<AjaxVoiceOverData> GetVoiceOversOfTriggerType(List<AjaxVoiceOverData> data, VoiceOverTriggerType triggerType)
    {
        List<AjaxVoiceOverData> voiceOvers = new List<AjaxVoiceOverData>();
        foreach (AjaxVoiceOverData voiceData in data)
        {
            if (voiceData.triggerType == triggerType) voiceOvers.Add(voiceData);
        }

        if (voiceOvers.Count == 0)
        {
            CustomDebug.LogWarning($"Did not find voiceover of trigger type: {triggerType}", Categories.VoiceOver.Root);
        }
        return voiceOvers;
    }

    public void PlayVoiceOverOfTriggerType(VoiceOverTriggerType triggerType)
    {
        List<AjaxVoiceOverData> voiceOvers = GetVoiceOversOfTriggerType(m_voiceOvers, triggerType);
        var currentFighter = GameInfoHelper.GetPlayer();
        AjaxVoiceOverData voiceOverData = GetVoiceOverTargetFighterVoiceOver(voiceOvers, currentFighter);

        if (voiceOverData == null)
        {
            CustomDebug.LogWarning("Voice over data to be played is null", Categories.VoiceOver.Root);
            return;
        }
        
        PlayVoiceOver(voiceOverData);
    }

    public void PlayVoiceOver(AjaxVoiceOverData voiceOverData)
    {
        SoundEffectsEventBus.SendPlay(voiceOverData.soundEffect);
        CustomDebug.Log($"Played voiceOver: {voiceOverData.soundEffect.effectName}", Categories.VoiceOver.Root);
    }
}
