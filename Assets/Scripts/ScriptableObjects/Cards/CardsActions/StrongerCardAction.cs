using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StrongerCardAction : BaseCardAction
{
    private StrongerCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (StrongerCard)cardData;

        GameplayEvents.OnFighterDamaged += OnFighterDamaged;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private void OnFighterDamaged(Fighter fighter, int damage)
    {
        bool isPlayer = GameInfoHelper.CompareFighterToPlayer(fighter);
        if(!isPlayer) return;
        
        GameActionHelper.GainInvent(m_data.Invent);
    }
}