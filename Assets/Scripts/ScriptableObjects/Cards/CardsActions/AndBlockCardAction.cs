using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AndBlockCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        AndBlockCard c = (AndBlockCard)cardData;
        MechanicsManager.Instance.AddMechanic(new BlockMechanic(c.Block, CombatManager.Instance.Player));
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            if (GameInfoHelper.CheckIfLastCardPlayedWas(c.PreviousCardName))
            {
                CombatManager.Instance.SpawnCard(c.SpawnCardName, CardStorage.HAND);
            }
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}