
using UnityEngine;

public class CyclicalEnemyTurnCounter : ITurnCounter
{
    private int m_cycleMaxNumber;
    private int m_currentRelativeTurn;

    public CyclicalEnemyTurnCounter(int cycleMaxNum)
    {
        m_cycleMaxNumber = cycleMaxNum;
    }

    public int GetGameTurn()
    {
        return CombatManager.Instance.CurrentTurn;
    }

    public int GetRelativeTurn()
    {
        return m_currentRelativeTurn;
    }

    public void NextTurn()
    {
        m_currentRelativeTurn++;

        if (m_currentRelativeTurn > m_cycleMaxNumber)
        {
            m_currentRelativeTurn = 1;
        }
        // Debug.Log($"----Current Turn {m_currentRelativeTurn}----");
    }
}
