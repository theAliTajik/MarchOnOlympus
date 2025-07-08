using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class BurnMechanic : BaseMechanic
{
    public BurnMechanic()
    {
        
    }

    public BurnMechanic(int stack, IHaveMechanics mOwner, bool hasGuard = false, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
    }
    public override MechanicType GetMechanicType()
    {
        return MechanicType.BURN;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn)
    {
        if (phase == CombatPhase.TURN_START && isMyTurn)
        {
            ReduceStack(m_stack/2);
            return true;
        }
        return false;
    }
    
    public override void Apply(Fighter.DamageContext context)
    {
        if (!MechanicsManager.Instance.Contains(context.Target, MechanicType.BURN) && !context.DoesReturnToSender)
        {
            return;
        }

        int returnDamageAmout = MechanicsManager.Instance.GetMechanicsStack(context.Target, MechanicType.BURN);

        if (context.Sender is IDamageable damageable)
        {
            damageable.TakeDamage(returnDamageAmout, context.Target as Fighter, false);
        }
    }
}
