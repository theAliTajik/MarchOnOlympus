using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using UnityEditor.IMGUI.Controls;

public class HauntMechanic : BaseMechanic
{
    public HauntMechanic()
    {
        
    }

    public HauntMechanic(int stack, IHaveMechanics mOwner, bool hasGuard = false, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
    }

    public override MechanicType GetMechanicType()
    {
        return MechanicType.HAUNT;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn)
    {
        if (m_stack >= 10)
        {
            Fighter target = GameInfoHelper.GetPlayer();
			GameActionHelper.AddMechanicToFighter(target, 1, MechanicType.PANIC);
            ReduceStack(10);
            return true;
        }

        return false;
    }
}
