using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class HectorCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        HectorCard c = (HectorCard)cardData;

        Hector hector = FindHector();
        GameActionHelper.HealPlayer(c.Restore);
        GameActionHelper.AddMechanicToPlayer(c.Bleed, MechanicType.BLEED);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private Hector FindHector()
    {
        List<Fighter> allEnemies = GameInfoHelper.GetAllEnemies();
        return allEnemies.Find(enemy => enemy.GetType().Name == "Hector") as Hector;
    }

}