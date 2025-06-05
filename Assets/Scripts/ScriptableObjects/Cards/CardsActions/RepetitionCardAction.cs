using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class RepetitionCardAction : BaseCardAction
{
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        RepetitionCard c = (RepetitionCard)cardData;

        CombatManager.Instance.OnCombatPhaseChanged += OnCardPlayed;
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

    public void OnCardPlayed(CombatPhase phase)
    {
        if (phase == CombatPhase.CARD_PLAYED)
        {
            if (GameInfoHelper.CheckIfLastCardPlayedWas(CardType.TECH, true))
            {
                MechanicsManager.Instance.AddMechanic(new StrenghtMechanic(1, CombatManager.Instance.Player));
            }
        }
    }
}