using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class BulwarkCardAction : BaseCardAction
{

    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        BulwarkCard c = (BulwarkCard)cardData;
        
        MechanicsManager.Instance.AddMechanic(new BlockMechanic(c.Block, CombatManager.Instance.Player));

        
        StartCoroutine(WaitAndExecute(finishCallback, 2f));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay)
    {
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }
    
}