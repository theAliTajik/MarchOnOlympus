using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class BurningAegisPerk : BasePerk
{

    private BurningAegisPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (BurningAegisPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}
    
    private void OnDestroy(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.CARD_PLAYED};
        return phases;
    }

    public override float GetPriority()
    {
        return 5;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        CardDisplay lastCard = GameInfoHelper.CardsData.SelectedCard;
        //Debug.Log("card selected: " + lastCard.CardInDeck.GetCardName());
        List<CardActionType> normalActions = lastCard.CardInDeck.NormalState.GetActionsTypes();
        List<CardActionType> stanceActions = lastCard.CardInDeck.StanceState.GetActionsTypes();
        
        if (normalActions.Contains(CardActionType.BLOCK))
        {
            damageRandEnemy();
        }

        if (stanceActions.Contains(CardActionType.BLOCK) && CombatManager.Instance.CurrentStance == lastCard.CardInDeck.GetStance())
        {
            damageRandEnemy();
        }
    }

    private void damageRandEnemy()
    {
        Fighter randEnemy = GameInfoHelper.GetRandomEnemy();
        randEnemy.TakeDamage(m_perkData.Damage, GameInfoHelper.GetPlayer(), false);
    }
}
