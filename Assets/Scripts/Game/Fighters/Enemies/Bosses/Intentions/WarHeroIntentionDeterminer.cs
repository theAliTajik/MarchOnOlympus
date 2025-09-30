
using System.Collections.Generic;

public class WarHeroIntentionDeterminer : IDetermineIntention
{
    private ConditionalRandomIntentionDeterminer m_CRIntentionDeterminer;

    private BaseEnemy.MoveData m_keyMove;

    public WarHeroIntentionDeterminer(List<BaseEnemy.MoveData> moves, BaseEnemy.MoveData keyMove)
    {
        m_CRIntentionDeterminer = new ConditionalRandomIntentionDeterminer(moves);
        m_keyMove = keyMove;
    }
    
    public void SetMoves(BaseEnemy.MoveData[] moves)
    {
        m_CRIntentionDeterminer.SetMoves(moves);
    }

    public BaseEnemy.MoveData? DetermineIntention()
    {
        if (m_keyMove.Condition())
        {
            return m_keyMove;
        }
        
        return m_CRIntentionDeterminer.DetermineIntention();
    }
}
