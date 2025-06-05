using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class BerserkersRageCardAction : BaseCardAction
{
    private BerserkersRageCard m_data;
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (BerserkersRageCard)cardData;

        GameplayEvents.GamePhaseChanged += OnPhaseChange;
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

    private void OnPhaseChange(EGamePhase phase)
    {
        switch (phase)
        {
            case EGamePhase.CARD_PLAYED:
                CardDisplay lastCard = GameInfoHelper.GetLastCardPlayed();
                if (lastCard.CardInDeck.GetStance() == Stance.BERSERKER)
                {
                    GameActionHelper.AddMechanicToPlayer(m_data.StrGain, MechanicType.STRENGTH);
                    GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), GameInfoHelper.GetPlayer(), m_data.SelfDamage);
                }
                break;
            case EGamePhase.PLAYER_TURN_END:
                GameplayEvents.GamePhaseChanged -= OnPhaseChange;
                break;
        }
    }

    private void OnDestroy()
    {
        GameplayEvents.GamePhaseChanged -= OnPhaseChange;
    }
}