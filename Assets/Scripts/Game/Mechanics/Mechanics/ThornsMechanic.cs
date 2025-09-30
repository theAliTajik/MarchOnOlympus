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

    public ThornsMechanic(int stack, IHaveMechanics mOwner, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;

        m_stack.SetGuard(guardMin);
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.THORNS;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn, bool isFirstTimeInTurn = false)
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

        context.IsDamageSentByThorns = true;
        damageable.TakeDamage(thorns.Stack, context.Target as Fighter, false, false, context);
    }
}
