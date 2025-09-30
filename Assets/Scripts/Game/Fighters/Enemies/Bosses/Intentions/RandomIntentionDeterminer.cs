
using System;
using KaimiraGames;
using UnityEngine;

public class RandomIntentionDeterminer : IDetermineIntention
{
    private WeightedList<BaseEnemy.MoveData> m_moves = new();
    private BaseEnemy.MoveData m_nextMove;
    private BaseEnemy.MoveData? m_previousMove;

    private BaseEnemy.MoveData m_removedRepeatedMove;
    private int m_moveRepeats = 1;
    private bool m_hasMovesChanged = false;

    public RandomIntentionDeterminer()
    {
        
    }

    public RandomIntentionDeterminer(BaseEnemy.MoveData[] moves)
    {
        SetMoves(moves);
    }

    public void SetMoves(BaseEnemy.MoveData[] moves)
    {
        if (m_moves == null || moves.Length == 0)
        {
            Debug.Log("WARNING: empty moves passed set for enemy");
            return;
        }

        // Debug.Log("Changing moves");
        m_hasMovesChanged = true;
        m_moves.Clear();
        for (int i = 0; i < moves.Length; i++)
        {
            BaseEnemy.MoveData md = moves[i];
            m_moves.Add(md, md.chance);
        }

        UpdateCurrentMoveWeight();

    }
    
    public BaseEnemy.MoveData? DetermineIntention()
    {
        if (m_moves == null || m_moves.Count == 0)
        {
            return null;
        }
    
        m_nextMove = m_moves.Next();

        HandleMoveRepetition();
        UpdateCurrentMoveWeight();

        m_previousMove = m_nextMove;
        return m_nextMove;
    }

    private void HandleMoveRepetition()
    {
        if (!m_previousMove.HasValue)
        {
            m_moveRepeats = 0;
            return;
        }

        if (m_moves.Count == 1)
        {
            return;
        }

        bool isRepeating = m_nextMove.clientID == m_previousMove.Value.clientID;

        if (isRepeating)
        {
            m_moveRepeats++;
            // Debug.Log($"Move repeat detected. Move: {m_nextMove.clientID}, repeats: {m_moveRepeats}");
            if (m_moveRepeats > 2)
            {
                Debug.LogWarning($"Move: {m_nextMove.clientID} repeated more than 2 times");
            }
        }
        else // Streak is broken
        {
            ResetPreviousMoveWeightOnStreakBreak();
            m_moveRepeats = 0; // Reset counter for the new move.
        }
    }

    private void ResetPreviousMoveWeightOnStreakBreak()
    {
        // Only log and reset if there was an actual streak (more than 1 repeat).
        if (m_moveRepeats <= 0) return;

        var move_to_reset = m_removedRepeatedMove;
    
        // Debug.Log($"Move streak broken.");

        if (m_hasMovesChanged)
        {
            m_hasMovesChanged = false;
            // Debug.Log("Moves has changed.");
            return;
        }

        if (m_moves.Contains(move_to_reset))
        {
            // Debug.Log("Moves already contained removed move.");
            m_moves.SetWeight(move_to_reset, move_to_reset.chance);
            return;
        }

        // Debug.Log($"Adding removed move: {move_to_reset.clientID}");
        m_moves.Add(move_to_reset, move_to_reset.chance);
    }

    private void UpdateCurrentMoveWeight()
    {
        if (string.IsNullOrEmpty(m_nextMove.clientID))
        {
            return;
        }

        if (m_moves.Count == 1)
        {
            return;
        }
        
        int weight = m_nextMove.chance;

        // Apply probability curve if it exists.
        if (m_nextMove.probabilities != null && m_nextMove.probabilities.Length > 0)
        {
            // Clamp repeat count to be a valid index for the probabilities array.
            int repeatsClamped = Mathf.Clamp(m_moveRepeats, 1, m_nextMove.probabilities.Length);
            float probabilityMultiplier = m_nextMove.probabilities[repeatsClamped - 1];
        
            weight = Mathf.RoundToInt(m_nextMove.chance * probabilityMultiplier);
        
            // Debug.Log($"Set weight of move {m_nextMove.clientID} to {weight}");
        }

        if (weight <= 0)
        {
            m_removedRepeatedMove = m_nextMove;
            // Debug.Log($"Removed the move: {m_removedRepeatedMove.clientID}.");
            m_moves.Remove(m_removedRepeatedMove);
            return;
        }

        m_moves.SetWeight(m_nextMove, weight);
    }}
