
public class StandardEnemyTurnCounter : ITurnCounter
{
    private int m_currentRelativeTurn;

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
    }
}
