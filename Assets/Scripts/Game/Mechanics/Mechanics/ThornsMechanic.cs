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

    public ThornsMechanic(int stack, IHaveMechanics mOwner, bool hasGuard = false, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
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
        if (!MechanicsManager.Instance.Contains(context.Target, MechanicType.THORNS) && !context.DoesReturnToSender)
        {
            return;
        }

        BaseMechanic thorns = MechanicsManager.Instance.GetMechanic(context.Target, MechanicType.THORNS);

        if (context.Sender is not IDamageable damageable)
        {
            return;
        }
        
        damageable.TakeDamage(thorns.Stack, context.Target as Fighter, false);
    }
}
