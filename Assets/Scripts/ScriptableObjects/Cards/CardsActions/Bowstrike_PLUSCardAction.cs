using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bowstrike_PLUSCardAction : BaseCardAction
{
    private Bowstrike_PLUSCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (Bowstrike_PLUSCard)cardData;
        
        GameActionHelper.AddMechanicToFighter(target, m_data.Daze, MechanicType.DAZE);
        int inventLevel = GameInfoHelper.GetInventLevel();
        
        if (inventLevel > m_data.InventLevelThreshold)
        {
            GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), m_data.Damage);
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}