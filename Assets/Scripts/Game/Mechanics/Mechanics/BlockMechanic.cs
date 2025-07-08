using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class BlockMechanic : BaseMechanic
{
    public BlockMechanic()
    {
        
    }

    public BlockMechanic(int stack, IHaveMechanics mOwner, bool hasGuard = false, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
    }
    public override MechanicType GetMechanicType()
    {
        return MechanicType.BLOCK;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn)
    {
        if (phase == CombatPhase.TURN_START && isMyTurn)
        {
            ReduceStack(m_stack/2);
            return true;
        }
        return false;
    }
    
    public override void Apply(Fighter.DamageContext context)
    {
        if (context.IsArmorPiercing)
        {
            return;
        }


        if (!MechanicsManager.Instance.Contains(context.Target, MechanicType.BLOCK))
        {
            return;
        }

        BaseMechanic block = MechanicsManager.Instance.GetMechanic(context.Target, MechanicType.BLOCK);
        int blockToReduce = Math.Min(context.ModifiedDamage, block.Stack);
        MechanicsManager.Instance.ReduceMechanic(context.Target, MechanicType.BLOCK, blockToReduce);
        context.ModifiedDamage -= blockToReduce;
    }
}
