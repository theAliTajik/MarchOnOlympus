using System;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalRandomIntentionDeterminer : IDetermineIntention
{
    private RandomIntentionDeterminer m_RandomIntentionDeterminer;
    private List<BaseEnemy.MoveData> m_conditionalMoves;
    
    
    public ConditionalRandomIntentionDeterminer(List<BaseEnemy.MoveData> conditionalMoves)
    {
        m_RandomIntentionDeterminer = new RandomIntentionDeterminer();
        m_conditionalMoves = conditionalMoves;
    }
    
    public void SetMoves(BaseEnemy.MoveData[] moves)
    {
        Debug.Log("WARNING: cannot set Conditional Random Intention Determiners moves using set moves method. this results in no condition for all moves.");
        m_conditionalMoves.Clear();
        m_conditionalMoves.AddRange(moves);
    }

    public BaseEnemy.MoveData? DetermineIntention()
    {
        if (m_conditionalMoves.Count == 0)
        {
            Debug.Log("WARNING: No moves in conditional move determiner");
            return null;
        }

        List<BaseEnemy.MoveData> movePool = new List<BaseEnemy.MoveData>();
        foreach (var conditionalMove in m_conditionalMoves)
        {
            if (conditionalMove.Condition == null)
            {
                movePool.Add(conditionalMove);
                continue;
            }
            
            if (conditionalMove.Condition())
            {
                movePool.Add(conditionalMove);
            }
        }

        if (movePool.Count == 0)
        {
            Debug.Log("WARNING: no enemy moves met condition");
            return null;
        }
        
        m_RandomIntentionDeterminer.SetMoves(movePool.ToArray());
        return m_RandomIntentionDeterminer.DetermineIntention();    
    }
}
