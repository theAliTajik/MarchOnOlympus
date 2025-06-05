using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class UntilTheEndCardAction : BaseCardAction
{
    private int m_restore;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        UntilTheEndCard c = (UntilTheEndCard)cardData;

        GameplayEvents.MechanicAddedToFighter += OnMechanicAdded;
        m_restore = c.Restore;
        if (MechanicsManager.Instance.Contains(CombatManager.Instance.Player, MechanicType.BLOCK))
        {
            OnMechanicAdded(CombatManager.Instance.Player, MechanicsManager.Instance.GetMechanic(CombatManager.Instance.Player, MechanicType.BLOCK));
        }
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        yield return new WaitForSeconds(delay);
        finishCallback?.Invoke();
    }

    private void OnMechanicAdded(Fighter fighter, BaseMechanic mechanic)
    {
        if (fighter == CombatManager.Instance.Player && mechanic.GetMechanicType() == MechanicType.BLOCK)
        {
            mechanic.OnEnd += OnBlockMechanicRemoved;
        }
    }
    
    private void OnBlockMechanicRemoved(MechanicType mechanicType)
    {
        CombatManager.Instance.Player.Heal(m_restore);
    }

}