
using Game;
using UnityEngine;

public class AcidicDotTwoMechanic : BaseMechanic
{
    private ITurnCounter m_turnCounter;
    private const int m_turnThreshold = 1;

    public AcidicDotTwoMechanic()
    {
        
    }

    public AcidicDotTwoMechanic(int stack, IHaveMechanics mOwner, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
        
        m_stack.SetGuard(guardMin);
        m_turnCounter = new StandardEnemyTurnCounter();
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.ACIDICDOTTWO;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn, bool isFirstTimeInTurn = false)
    {
        if (phase != CombatPhase.TURN_START) return false;
        if (!isMyTurn) return false;
        
        CustomDebug.Log($"Relative Turn: {m_turnCounter.GetRelativeTurn()}", Categories.Mechanics.AcidicDotTwo, DebugTag.LOGIC);
        if (m_turnCounter.GetRelativeTurn() < m_turnThreshold)
        {
            m_turnCounter.NextTurn();
            return false;
        }

        if (m_mechanicOwner is IDamageable damageable)
        {
            int damageMultiplier = 5;
            int damage = m_stack * damageMultiplier;
            CustomDebug.Log($"Hit: {damage}", Categories.Mechanics.AcidicDotTwo, DebugTag.LOGIC);
            damageable.TakeDamage(damage, null, false);
        }
        RaiseOnEnd();
        return true;
    }
}
