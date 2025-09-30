using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class StrenghtMechanic : BaseMechanic
{
    public StrenghtMechanic()
    {
        
    }

    public StrenghtMechanic(int stack, IHaveMechanics mOwner, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
        
        m_stack.SetGuard(guardMin);
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.STRENGTH;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn, bool isFirstTimeInTurn = false)
    {
        // if (phase == CombatPhase.TURN_START && isMyTurn)
        // {
        //     ReduceStack(1);
        //     return true;
        // }
        return false;
    }
    
    public override void Apply(Fighter.DamageContext context)
    {
        if (context.Sender == context.Target)
        {
            return;
        }

        if (context.IsDamageSentByThorns || context.IsDamageSentByBurn)
        {
            return;
        }

        int strenghtStack = MechanicsManager.Instance.GetMechanicsStack(context.Sender, MechanicType.STRENGTH);
        if (strenghtStack > 0)
        {
            context.ModifiedDamage += strenghtStack;
        }
    }
}
