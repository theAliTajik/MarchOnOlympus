using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class FrenzyMechanic : BaseMechanic
{
    public FrenzyMechanic()
    {
        
    }

    public FrenzyMechanic(int stack, Fighter fighter, bool hasGuard = false, int guardMin = 0)
    {
        m_stack = stack;
        m_fighter = fighter;
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.FRENZY;
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
        context.ModifiedDamage = (int)(context.ModifiedDamage * 1.5f);
    }
}
