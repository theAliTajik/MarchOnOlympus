using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class BurnDrain_STARCardAction : BaseCardAction
{
    private BurnDrain_STARCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (BurnDrain_STARCard)cardData;

        var player = GameInfoHelper.GetPlayer();
        int burnStack = GameActionHelper.RemoveMechanicOfType(player, MechanicType.BURN);
        
        GameActionHelper.HealPlayer(burnStack);
        
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}