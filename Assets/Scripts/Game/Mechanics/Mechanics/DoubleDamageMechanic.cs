using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class DoubleDamageMechanic : BaseMechanic
{
    public DoubleDamageMechanic()
    {
        
    }

    public DoubleDamageMechanic(int stack, IHaveMechanics mOwner, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
        
        m_stack.SetGuard(guardMin);
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.DOUBLEDAMAGE;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn, bool isFirstTimeInTurn = false)
    {
        return false;
    }
    
    public override void Apply(Fighter.DamageContext context)
    {
        if (MechanicsManager.Instance.Contains(context.Target, MechanicType.DOUBLEDAMAGE))
        {
            context.ModifiedDamage = (int)(context.ModifiedDamage * 2f);
        }
    }
}
