
using Game;
using UnityEngine;

public class AcidicDotMechanic : BaseMechanic
{
    private ITurnCounter m_turnCounter;
    private const int m_turnThreshold = 1;

    public AcidicDotMechanic()
    {
        
    }

    public AcidicDotMechanic(int stack, IHaveMechanics mOwner, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
        
        m_stack.SetGuard(guardMin);

        m_turnCounter = new StandardEnemyTurnCounter();
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.ACIDICDOT;
    }

    public override void IncreaseStack(int amount)
    {
        if (m_turnCounter != null)
        {
            int turn = m_turnCounter.GetRelativeTurn();
            if (turn > 0)
            {
                GameActionHelper.AddMechanicToOwner(m_mechanicOwner, amount, MechanicType.ACIDICDOTTWO);
                return;
            }
        }
        
        base.IncreaseStack(amount);
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn, bool isFirstTimeInTurn = false)
    {
        if (phase != CombatPhase.TURN_START) return false;
        if (!isMyTurn) return false;
        
        CustomDebug.Log($"Relative Turn: {m_turnCounter.GetRelativeTurn()}", Categories.Mechanics.AcidicDot, DebugTag.LOGIC);
        if (m_turnCounter.GetRelativeTurn() < m_turnThreshold)
        {
            m_turnCounter.NextTurn();
            return false;
        }

        if (m_mechanicOwner is IDamageable damageable)
        {
            int damageMultiplier = 5;
            int damage = m_stack * damageMultiplier;
            CustomDebug.Log($"Hit: {damage}", Categories.Mechanics.AcidicDot, DebugTag.LOGIC);
            damageable.TakeDamage(damage, null, false);
        }
        RaiseOnEnd();
        return true;
    }
}
