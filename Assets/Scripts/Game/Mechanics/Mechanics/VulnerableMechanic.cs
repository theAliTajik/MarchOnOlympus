using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class VulnerableMechanic : BaseMechanic
{
    public VulnerableMechanic()
    {
        
    }

    public VulnerableMechanic(int stack, IHaveMechanics mOwner, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
        
        m_stack.SetGuard(guardMin);
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.VULNERABLE;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn, bool isFirstTimeInTurn = false)
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
