using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class DexterityMechanic : BaseMechanic
{
    public DexterityMechanic()
    {
        
    }

    public DexterityMechanic(int stack, IHaveMechanics mOwner, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
        
        m_stack.SetGuard(guardMin);
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.DEXTERITY;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn, bool isFirstTimeInTurn = false)
    {
        return false;
    }
}
