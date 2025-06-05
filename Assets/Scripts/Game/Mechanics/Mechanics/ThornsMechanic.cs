using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class ThornsMechanic : BaseMechanic
{
    public ThornsMechanic()
    {
        
    }

    public ThornsMechanic(int stack, Fighter fighter, bool hasGuard = false, int guardMin = 0)
    {
        m_stack = stack;    
        m_fighter = fighter;
        m_hasGuard = hasGuard;
        m_guardMin = guardMin;
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.THORNS;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn)
    {
        return false;
    }
    
    public override void Apply(Fighter.DamageContext context)
    {
        if (MechanicsManager.Instance.Contains(context.Target, MechanicType.THORNS) && context.DoesReturnToSender)
        {
            BaseMechanic thorns = MechanicsManager.Instance.GetMechanic(context.Target, MechanicType.THORNS);
            context.Sender.TakeDamage(thorns.Stack, context.Target, false);
        }
    }
}
