using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class StunMechanic : BaseMechanic
{
    public StunMechanic()
    {
        
    }

    public StunMechanic(int stack, Fighter fighter, bool hasGuard = false, int guardMin = 0)
    {
        m_stack = stack;
        m_fighter = fighter;
        if (fighter is BaseEnemy)
        {
            BaseEnemy enemy = (BaseEnemy)fighter;
            enemy.GetStuned();
        }
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.STUN;
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
    
}
