using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FootworkCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        FootworkCard c = (FootworkCard)cardData;
        
        MechanicsManager.Instance.AddMechanic(new StrenghtMechanic(c.Str, CombatManager.Instance.Player));
        MechanicsManager.Instance.AddMechanic(new FortifiedMechanic(1, CombatManager.Instance.Player));

        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            CombatManager.Instance.ForceChangeStance(c.SwitchToStance);
            GameActionHelper.TransformCard(cardDisplay, c.TransformCardName);
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}