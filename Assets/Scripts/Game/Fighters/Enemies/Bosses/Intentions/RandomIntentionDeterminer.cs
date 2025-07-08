
using KaimiraGames;
using UnityEngine;

public class RandomIntentionDeterminer : IDetermineIntention
{
    private WeightedList<BaseEnemy.MoveData> m_moves = new();
    private BaseEnemy.MoveData m_nextMove;
    private BaseEnemy.MoveData? m_previousMove;
    private int m_moveRepeats = 1;

    public RandomIntentionDeterminer(BaseEnemy.MoveData[] moves)
    {
        SetMoves(moves);
    }

    private void SetMoves(BaseEnemy.MoveData[] moves)
    {
        if (m_moves == null || moves.Length == 0)
        {
            Debug.Log("WARNING: empty moves passed set for enemy");
            return;
        }
                
        for (int i = 0; i < moves.Length; i++)
        {
            BaseEnemy.MoveData md = moves[i];
            m_moves.Add(md, md.chance);
        } 
                
    }
    
    public BaseEnemy.MoveData DetermineIntention()
    {
        m_nextMove = m_moves.Next();
        if (m_previousMove.HasValue && m_nextMove.clientID == m_previousMove.Value.clientID)
        {
            m_moveRepeats++;
            //Debug.Log("Move repeat detected. num of repeats: " + m_moveRepeats);
        }
        else if (m_previousMove.HasValue)
        {
            if (m_moveRepeats > 1)
            {
                //Debug.Log("Move repeat streak broken. move: " + m_previusMove.Value.clientID + " has it's weight set to: " + m_previusMove.Value.chance);    
            }

            if (m_moves.Contains(m_previousMove.Value))
            {
                m_moves.SetWeight(m_previousMove.Value, m_previousMove.Value.chance);
            }
            m_moveRepeats = 1;
        }

        int moveRepeatsClamped;
        int weight = m_nextMove.chance;
        
        if (m_nextMove.probabilities.Length > 0)
        {
            moveRepeatsClamped = Mathf.Clamp(m_moveRepeats, 1, m_nextMove.probabilities.Length);
        
            weight = Mathf.RoundToInt(m_nextMove.chance * m_nextMove.probabilities[moveRepeatsClamped-1]); 
        }
        
        m_moves.SetWeight(m_nextMove, weight);

        m_previousMove = m_nextMove;
        
        return m_nextMove;
    }
}
