using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackstabCardAction : BaseCardAction
{
    private BackstabCard m_card;
    private CardDisplay m_cardDisplay = null;

    public override void Config(CardDisplay cardDisplay)
    {
        m_cardDisplay = cardDisplay;
        m_card = (BackstabCard)cardDisplay.CardInDeck.GetCardData();
    }
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        BackstabCard c = (BackstabCard)cardData;
        target.TakeDamage(c.Damage, CombatManager.Instance.Player, true);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

    private void Update()
    {
        if (m_cardDisplay == null)
        {
            return;
        }

        if (m_cardDisplay.CardState == CardState.DEACTIVE)
        {
            return;
        }
        //check targets for debuff
        if (MechanicsManager.Instance.AnyEnemyContainsAny(MechanicsManager.Instance.DebuffMechanics))
        {
            GameActionHelper.SetCardEnergyOverride(m_cardDisplay, ECardInDeckState.STANCE, m_card.CostIfAnyTargetHasADebuff);
            m_cardDisplay.RefreshUI();
        }
        else
        {
            m_card.stanceDataSet.energyCost = m_card.NormalCost;
            m_cardDisplay.RefreshUI();
        }
        
    }
}