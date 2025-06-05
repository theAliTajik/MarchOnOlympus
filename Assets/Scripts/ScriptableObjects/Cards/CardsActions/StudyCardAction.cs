using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class StudyCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target)
    {
        StudyCard c = (StudyCard)cardData;
        MechanicsManager.Instance.AddMechanic(new FortifiedMechanic(1, CombatManager.Instance.Player));
        
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            MechanicsManager.Instance.AddMechanic(new VulnerableMechanic(2, target));
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}