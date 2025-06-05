using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class PatienceCardAction : BaseCardAction
{
    private PatienceCard m_data;
    private bool m_cardWasDrawnThisTurn;
    
    private bool m_cardWasUsedThisTurn;
    private Fighter m_target;
    
    private CardDisplay m_cardDisplay;
    private int m_numOfTurnsCardWasUnused = 0;

    public override void Config(CardDisplay cardDisplay)
    {
        base.Config(cardDisplay);
        m_cardDisplay = cardDisplay;
        GameplayEvents.GamePhaseChanged += OnPhaseChange;
    }

    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (PatienceCard)cardData;
        
        //damage per not used

        if (m_numOfTurnsCardWasUnused > 0)
        {
            int damage = m_data.DamagePerNotUsed * m_numOfTurnsCardWasUnused;
            
            if (damage > 0)
            {
                m_target.TakeDamage(damage, CombatManager.Instance.Player, true);
            }
            
            if (CombatManager.Instance.CurrentStance == cardData.MStance)
            {
                int block = m_data.BlockPerNotUsed * m_numOfTurnsCardWasUnused;
                if (block > 0)
                {
                    GameActionHelper.AddMechanicToPlayer(block, MechanicType.BLOCK);
                }

            }
        }

        m_numOfTurnsCardWasUnused = 0;
        m_cardWasUsedThisTurn = true;
        finishCallback?.Invoke();
        yield break;
    }

    private void OnPhaseChange(EGamePhase phase)
    {
        switch (phase)
        {
            case EGamePhase.CARD_DRAW_FINISHED:
                List<CardDisplay> currentCards = GameInfoHelper.GetAllCardsIn(CardStorage.HAND);
                if (currentCards.Contains(m_cardDisplay))
                {
                    m_cardWasDrawnThisTurn = true;
                }
                else
                {
                    m_cardWasDrawnThisTurn = false;
                }
                break;
            case EGamePhase.PLAYER_TURN_END:
                if (m_cardWasDrawnThisTurn && !m_cardWasUsedThisTurn)
                {
                    m_numOfTurnsCardWasUnused++;
                }
                break;
        }
    }

    private void OnDestroy()
    {
        GameplayEvents.GamePhaseChanged -= OnPhaseChange;
    }
}