using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class ImproviseMechanic : BaseMechanic
{
    public ImproviseMechanic()
    {
        
    }

    public ImproviseMechanic(int stack, IHaveMechanics mOwner, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
        
        m_stack.SetGuard(guardMin);
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.IMPROVISE;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn, bool isFirstTimeInTurn = false)
    {
        return false;
    }
}
