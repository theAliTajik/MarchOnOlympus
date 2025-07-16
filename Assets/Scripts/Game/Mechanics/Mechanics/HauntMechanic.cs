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
            GameActionHelper.AddMechanicToFighter(GameInfoHelper.GetPlayer(), 1, MechanicType.PANIC);
            ReduceStack(10);
            return true;
        }

        if (phase == CombatPhase.TURN_START && isMyTurn)
        {
            ReduceStack(m_stack / 2);
            return true;
        }
        return false;
    }
    
    public override void Apply(Fighter.DamageContext context)
    {
        if (!MechanicsManager.Instance.Contains(context.Target, MechanicType.HAUNT) && !context.DoesReturnToSender)
        {
            return;
        }

        int returnDamageAmout = MechanicsManager.Instance.GetMechanicsStack(context.Target, MechanicType.HAUNT);

        if (context.Sender is IDamageable damageable)
        {
            damageable.TakeDamage(returnDamageAmout, context.Target as Fighter, false);
        }
    }
}
