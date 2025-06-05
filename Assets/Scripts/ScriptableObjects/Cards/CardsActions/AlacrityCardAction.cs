using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class AlacrityCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {

        
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute (Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        AlacrityCard c = (AlacrityCard)cardData;
    
        MechanicsManager.Instance.AddMechanic(new BlockMechanic(c.Block, CombatManager.Instance.Player));
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            CombatManager.Instance.DrawCard(1);
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}