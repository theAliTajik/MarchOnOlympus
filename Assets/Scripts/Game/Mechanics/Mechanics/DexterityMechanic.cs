using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class DexterityMechanic : BaseMechanic
{
    public DexterityMechanic()
    {
        
    }

    public DexterityMechanic(int stack, IHaveMechanics mOwner, bool hasGuard = false, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.DEXTERITY;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn)
    {
        return false;
    }
}
