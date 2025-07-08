using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class DazeMechanic : BaseMechanic
{
    public DazeMechanic(int stack, IHaveMechanics mOwner, bool hasGuard = false, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.DAZE;
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
        context.ModifiedDamage = (int)Math.Round(context.ModifiedDamage / 2.0f);
    }
}
