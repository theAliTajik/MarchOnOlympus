using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class FortifiedMechanic : BaseMechanic
{
    public FortifiedMechanic()
    {
        
    }

    public FortifiedMechanic(int stack, IHaveMechanics mOwner, bool hasGuard = false, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.FORTIFIED;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn)
    {
        if (phase == CombatPhase.TURN_START && isMyTurn)
        {
            ReduceStack(1);
            return true;
        }

        return false;
    }
    
    public override void Apply(Fighter.DamageContext context)
    {
        if (MechanicsManager.Instance.Contains(context.Target, MechanicType.FORTIFIED))
        {
            context.ModifiedDamage = (int)Math.Round(context.ModifiedDamage / 2.0f);
        }
    }
}
