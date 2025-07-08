using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class StunMechanic : BaseMechanic
{
    public StunMechanic()
    {
        
    }

    public StunMechanic(int stack, IHaveMechanics mOwner, bool hasGuard = false, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
        
        if(m_mechanicOwner is IGetStunned stunnable)
        {
            stunnable.Stun();
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
