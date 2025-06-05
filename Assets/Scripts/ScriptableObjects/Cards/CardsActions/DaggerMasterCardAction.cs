using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DaggerMasterCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        DaggerMasterCard c = (DaggerMasterCard)cardData;
        
        GameplayEvents.SendDaggerMasterCardPlayed(c);
        for (int i = 0; i < c.DaggerCardSpawnAmount; i++)
        {
            GameActionHelper.SpawnCard(c.DaggerCardID, CardStorage.DRAW_PILE);
        }
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        

        finishCallback?.Invoke();
        yield break;
    }

}