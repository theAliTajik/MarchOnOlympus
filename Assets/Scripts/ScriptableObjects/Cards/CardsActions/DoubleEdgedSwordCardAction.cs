using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleEdgedSwordCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        DoubleEdgedSwordCard c = (DoubleEdgedSwordCard)cardData;
        GameplayEvents.FighterRestoredHP += OnRestoredHP;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        
        finishCallback?.Invoke();
        yield break;
    }

    private void OnRestoredHP(Fighter fighter, int restoreAmount)
    {
        if (fighter != CombatManager.Instance.Player)
        {
            return;
        }
        Fighter randEnemy = EnemiesManager.Instance.GetRandomEnemy();
        if (restoreAmount > 0)
        {
            randEnemy.TakeDamage(restoreAmount, GameInfoHelper.GetPlayer(), false);
        }
    }

    private void OnDestroy()
    {
        GameplayEvents.FighterRestoredHP -= OnRestoredHP;
    }
}