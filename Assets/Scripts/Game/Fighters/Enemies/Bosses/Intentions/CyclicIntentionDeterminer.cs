
public class CyclicIntentionDeterminer : IDetermineIntention
{
    private BaseEnemy.MoveData[] m_moves;
    private int m_currentMove;
    
    public CyclicIntentionDeterminer(BaseEnemy.MoveData[] moves)
    {
        m_moves = moves;
    }
    
    public BaseEnemy.MoveData DetermineIntention()
    {
        BaseEnemy.MoveData move = m_moves[m_currentMove];
        m_currentMove++;
        if (m_currentMove >= m_moves.Length)
        {
            m_currentMove = 0;
        }
        return move;
    }
}
