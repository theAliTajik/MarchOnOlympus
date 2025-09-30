using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class BleedMechanic : BaseMechanic
{
    private const string m_SoundEffectID = "BleedTic";

    public BleedMechanic()
    {
        
    }

    public BleedMechanic(int stack, IHaveMechanics mOwner, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;

        m_stack.SetGuard(guardMin);
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.BLEED;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn, bool isFirstTimeInTurn = false)
    {
        if (phase != CombatPhase.TURN_START && !isMyTurn)
        {
            return false;
        }

        if(m_mechanicOwner is IDamageable damageable)
        {
            damageable.TakeDamage(m_stack, null, false);
        }
        ReduceStack(1);
        SoundEffectsEventBus.SendPlay(m_SoundEffectID);
        return true;
    }
}
