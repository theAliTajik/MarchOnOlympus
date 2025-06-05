using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IceCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        IceCard c = (IceCard)cardData;
        MechanicsManager.Instance.AddMechanic(new DexterityMechanic(c.Dex, CombatManager.Instance.Player));
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            GameActionHelper.TransformCard(cardDisplay, c.TransformCardName);
            CombatManager.Instance.Player.Heal(c.Restore);

        }
        
        finishCallback?.Invoke();
        yield break;
    }

}