using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShakeItOffCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        ShakeItOffCard c = (ShakeItOffCard)cardData;

        List<MechanicType> debuffs = GameInfoHelper.GetAllDebuffMechanics();
        
        Fighter player = GameInfoHelper.GetPlayer();
        int debuffsCount = 0;
        foreach (MechanicType debuff in debuffs)
        {
            if (GameInfoHelper.CheckIfFighterHasMechanic(player, debuff))
            {
                debuffsCount++;
                GameActionHelper.RemoveMechanicFromPlayer(debuff);
            }
        }
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            if (debuffsCount > 0)
            {
                CombatManager.Instance.Player.Heal(debuffsCount * c.RestoreForEachDebuff);
            }
        }
        

        finishCallback?.Invoke();
        yield break;
    }

}