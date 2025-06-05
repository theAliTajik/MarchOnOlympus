using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class PunisherCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        PunisherCard c = (PunisherCard)cardData;
        int damagePerBuff = c.Damage;
        int enemyBuffCount = MechanicsManager.Instance.GetMechanicsCount(target, MechanicCategory.BUFF);
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            damagePerBuff = c.StanceDamage;
        }

        if (enemyBuffCount > 0)
        {
            target.TakeDamage(damagePerBuff * enemyBuffCount, CombatManager.Instance.Player, true);
        }

        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

}