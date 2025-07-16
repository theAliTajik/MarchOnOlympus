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
        if (phase == CombatPhase.PLAYER_TURN_START)
        {
            Fighter player = GameInfoHelper.GetPlayer();
			player.TakeDamage(4, player, true, true);
            Debug.Log("[PLAYER_TURN_START] : [Panic] ---> Damage: 4");
			return true;
        }
        return false;
    }
}