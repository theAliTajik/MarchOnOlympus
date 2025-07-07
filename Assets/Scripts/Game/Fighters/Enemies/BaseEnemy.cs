using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using KaimiraGames;
using UnityEngine;
using UnityEngine.Serialization;



public abstract class BaseEnemy : Fighter
{
    [System.Serializable]
    public struct MoveData
    {
        public string clientID;
        public string description;
        public int chance;
        public float[] probabilities;

        public MoveData(string clientID, string description1, int chance = 10, float[] probabilities = null)
        {
            this.clientID = clientID;
            this.description = description1;
            this.chance = chance;
            this.probabilities = probabilities ?? new float[] {0.5f, 0};
        }
    }


    public event Action<Intention, string> OnIntentionDetermined;

    public virtual bool IsRequiredForCombatCompletion => true;

    [SerializeField] protected AnimatorHelper m_animation;
    
    protected WeightedList<MoveData> m_moves = new();
    protected  MoveData m_nextMove;
    protected  MoveData? m_previusMove;
    protected  int m_moveRepeats = 1;
    protected bool m_stuned = false;
    

    public virtual void DetermineIntention()
    {
        if (MechanicsManager.Instance.Contains(this, MechanicType.STUN))
        {
            m_stuned = true;
        }
    }

    public virtual void GetStuned()
    {
        m_stuned = true;
        CallOnIntentionDetermined(Intention.STUNED, "stuned");
    }
    
    public virtual void RandomIntentionPicker(WeightedList<MoveData> moves)
    {
        m_nextMove = moves.Next();
        if (m_previusMove.HasValue && m_nextMove.clientID == m_previusMove.Value.clientID)
        {
            m_moveRepeats++;
            //Debug.Log("Move repeat detected. num of repeats: " + m_moveRepeats);
        }
        else if (m_previusMove.HasValue)
        {
            if (m_moveRepeats > 1)
            {
                //Debug.Log("Move repeat streak broken. move: " + m_previusMove.Value.clientID + " has it's weight set to: " + m_previusMove.Value.chance);    
            }

            if (moves.Contains(m_previusMove.Value))
            {
                moves.SetWeight(m_previusMove.Value, m_previusMove.Value.chance);
            }
            m_moveRepeats = 1;
        }

        int moveRepeatsClamped = Mathf.Clamp(m_moveRepeats, 1, m_nextMove.probabilities.Length);
        
        int weight = Mathf.RoundToInt(m_nextMove.chance * m_nextMove.probabilities[moveRepeatsClamped-1]); 
        moves.SetWeight(m_nextMove, weight);

        m_previusMove = m_nextMove;
    }

    public virtual void ShowIntention()
    {
        //Debug.Log("enemy intends to do: " + m_nextMove.description);
    }
    
    public virtual void ExecuteAction(Action finishCallback)
    {
        if (MechanicsManager.Instance.Contains(this, MechanicType.STUN))
        {
            // skip turn
            finishCallback?.Invoke();
            return;
        }
    }
    
    protected IEnumerator WaitForAnimation(string animationName, Action finishCallback = null)
    {
        bool isDone = false;
        
        m_animation.Play(animationName, () =>
        {
            isDone = true;
            finishCallback?.Invoke();
        });

        while (!isDone)
        {
            yield return null;
        }
    }
    
    public override Vector3 GetRootPosition()
    {
        return m_root.position;
    }

    public override Vector3 GetHeadPosition()
    {
        return m_head.position;
    }


    public void SetCanBeTarget(bool isTarget)
    {
        Collider2D myCollider = GetComponent<Collider2D>();
        if (myCollider == null)
        {
            Debug.Log("enemy did not find collider");
            return;
        }
        
        if (isTarget)
        {
            myCollider.enabled = true;
        }
        else
        {
            myCollider.enabled = false;
        }
    }
    
    protected void CallOnIntentionDetermined(Intention intention, string description)
    {
        OnIntentionDetermined?.Invoke(intention, description);
    }

    protected void SetMoves(MoveData[] moves)
    {
        if (m_moves == null || moves.Length == 0)
        {
            Debug.Log("WARNING: empty moves passed set for enemy");
            return;
        }

        m_moves = new WeightedList<MoveData>();

		for (int i = 0; i < moves.Length; i++)
        {
            MoveData md = moves[i];
            m_moves.Add(md, md.chance);
        } 
    }
}

