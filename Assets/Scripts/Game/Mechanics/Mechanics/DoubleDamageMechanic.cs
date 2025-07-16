using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class DoubleDamageMechanic : BaseMechanic
{
    public DoubleDamageMechanic()
    {
        
    }

    public DoubleDamageMechanic(int stack, IHaveMechanics mOwner, bool hasGuard = false, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.DOUBLEDAMAGE;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn)
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
