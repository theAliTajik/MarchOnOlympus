using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class VulnerableMechanic : BaseMechanic
{
    public VulnerableMechanic()
    {
        
    }

    public VulnerableMechanic(int stack, Fighter fighter, bool hasGuard = false, int guardMin = 0)
    {
        m_stack = stack;
        m_fighter = fighter;
        m_hasGuard = hasGuard;
        m_guardMin = guardMin;
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.VULNERABLE;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn)
    {
        if (phase == CombatPhase.CARD_PLAYED)
        {
            ReduceStack(1);
            return true;
        }
        return false;
    }
    
    public override void Apply(Fighter.DamageContext context)
    {
        if (MechanicsManager.Instance.Contains(context.Target, MechanicType.VULNERABLE))
        {
            context.ModifiedDamage = (int)(context.ModifiedDamage * 1.5f);
        }
    }
}
