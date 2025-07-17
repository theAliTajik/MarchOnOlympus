using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class PanicMechanic : BaseMechanic
{
    public PanicMechanic()
    {
        
    }

    public PanicMechanic(int stack, IHaveMechanics mOwner, bool hasGuard = false, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.PANIC;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn)
    {
        if (phase == CombatPhase.TURN_START && isMyTurn)
        {
            Fighter target = GameInfoHelper.GetPlayer();
			target.TakeDamage(m_stack.Amount * 4, null, false, true);
            Debug.Log($"---> [PLAYER_TURN_START] : [PanicMechanic] ---> Damage: {m_stack.Amount * 4}");
			return true;
        }
        return false;
    }
}