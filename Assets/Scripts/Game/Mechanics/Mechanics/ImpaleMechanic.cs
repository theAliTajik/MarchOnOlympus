using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class ImpaleMechanic : BaseMechanic
{
    public ImpaleMechanic()
    {
        
    }

    public ImpaleMechanic(int stack, IHaveMechanics mOwner, bool hasGuard = false, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.IMPALE;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn)
    {
        return false;
    }

    public override void Apply(Fighter.DamageContext context)
    {
        if (context.Sender == context.Target)
        {
            return;
        }

        int impaleAmount = MechanicsManager.Instance.GetMechanicsStack(context.Sender, MechanicType.IMPALE);
        if (impaleAmount > 0)
        {
            context.ModifiedDamage -= impaleAmount;
        }
    }
}
