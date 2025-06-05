using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RefreshCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        RefreshCard c = (RefreshCard)cardData;
        
        GameActionHelper.AllowStanceChange(true);
        GameplayEvents.StanceChanged += OnStanceChanged;
        
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private void OnStanceChanged(Stance stance)
    {
        GameActionHelper.AllowStanceChange(false);
        GameplayEvents.StanceChanged -= OnStanceChanged;
    }

    private void OnDestroy()
    {
        GameplayEvents.StanceChanged -= OnStanceChanged;
    }
}