using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class BleedMechanic : BaseMechanic
{
    public BleedMechanic()
    {
        
    }

    public BleedMechanic(int stack, Fighter fighter, bool hasGuard = false, int guardMin = 0)
    {
        m_stack = stack;    
        m_fighter = fighter;
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.BLEED;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn)
    {
        if (phase == CombatPhase.TURN_START && isMyTurn)
        {
            if (m_fighter != null)
            {
                m_fighter.TakeDamage(m_stack, m_fighter, false);
            }
            ReduceStack(1);
            return true;
        }
        return false;
    }
}
