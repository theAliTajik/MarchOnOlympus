using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class ImproviseMechanic : BaseMechanic
{
    public ImproviseMechanic()
    {
        
    }

    public ImproviseMechanic(int stack, Fighter fighter, bool hasGuard = false, int guardMin = 0)
    {
        m_stack = stack;    
        m_fighter = fighter;
        m_hasGuard = hasGuard;
        m_guardMin = guardMin;
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.IMPROVISE;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn)
    {
        return false;
    }
}
