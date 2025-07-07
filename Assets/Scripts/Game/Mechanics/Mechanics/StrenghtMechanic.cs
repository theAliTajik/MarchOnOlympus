using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class StrenghtMechanic : BaseMechanic
{
    public StrenghtMechanic()
    {
        
    }

    public StrenghtMechanic(int stack, Fighter fighter, bool hasGuard = false, int guardMin = 0)
    {
        m_stack = stack;    
        m_fighter = fighter;
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.STRENGTH;
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
    
    public override void Apply(Fighter.DamageContext context)
    {
        if (context.Sender != context.Target)
        {
            int strenghtStack = MechanicsManager.Instance.GetMechanicsStack(context.Sender, MechanicType.STRENGTH);
            if (strenghtStack > 0)
            {
                context.ModifiedDamage += strenghtStack;
            }
        }
    }
}
