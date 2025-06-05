using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class RepairCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {

        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        //yield return new WaitForSeconds(delay);
        int block = MechanicsManager.Instance.GetMechanicsStack(CombatManager.Instance.Player, MechanicType.BLOCK);
        if (block > 0)
        {
            CombatManager.Instance.Player.Heal(block);
        }


        finishCallback?.Invoke();
        yield break;
    }
    
}