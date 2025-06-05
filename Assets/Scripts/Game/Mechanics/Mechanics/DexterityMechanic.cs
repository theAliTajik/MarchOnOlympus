using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class DexterityMechanic : BaseMechanic
{
    public DexterityMechanic()
    {
        
    }

    public DexterityMechanic(int stack, Fighter fighter, bool hasGuard = false, int guardMin = 0)
    {
        m_stack = stack;
        m_fighter = fighter;
        m_hasGuard = hasGuard;
        m_guardMin = guardMin;
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
