using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using KaimiraGames;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public abstract class BaseEnemy : Fighter, IGetStunned
{
    [System.Serializable]
    public struct MoveData
    {
        public string clientID;
        public string description;
        public int chance;
        public float[] probabilities;
        public Func<bool> Condition;

        public MoveData(string clientID)
        {
            this.clientID = clientID;
            description = null;
            chance = 0;
            probabilities = new float[] { };
            Condition = null;
        }
        
        public MoveData(string clientID, string description1, int chance = 10, float[] probabilities = null)
        {
            this.clientID = clientID;
            this.description = description1;
            this.chance = chance;
            this.probabilities = probabilities ?? new float[] {0.5f, 0};
            Condition = null;
        }
    }


    public event Action<Intention, string> OnIntentionDetermined;

    public virtual bool IsRequiredForCombatCompletion => true;
    public virtual bool IsInAllEnemiesList => true;

    [SerializeField] protected AnimatorHelper m_animation;
    
    protected WeightedList<MoveData> m_moves = new();
    protected IDetermineIntention m_intentionPicker;
    protected  MoveData m_nextMove;
    protected  MoveData? m_previusMove;
    protected  int m_moveRepeats = 1;
    protected bool m_stuned = false;
    protected bool m_isTarget = true;

    protected override void Awake()
    {
        base.Awake();
        m_intentionPicker = new RandomIntentionDeterminer();
        m_damageable = new EnemyDamageBehaviour(this);
        m_damageable.OnDamage += m_fighterHP.TakeDamage;
    }

    public virtual void DetermineIntention()
    {
        if (MechanicsManager.Instance.Contains(this, MechanicType.STUN))
        {
            m_stuned = true;
        }
    }
    
    public virtual void RandomIntentionPicker()
    {
        if (m_intentionPicker == null)
        {
            Debug.Log($"WARNING: Intention picker of enemy: {this.GetType()} was null");
            return;
        }
        var nextMove = m_intentionPicker.DetermineIntention();

        if (!nextMove.HasValue)
        {
            Debug.Log($"WARNING: Intention picker of enemy: {this.GetType()} returned null next move");
            return;
        }
        
        m_nextMove = nextMove.Value;
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


    public virtual void SetCanBeTarget(bool isTarget)
    {
        // Debug.Log($"enemy: {this.GetType()} set can be target: {isTarget}");
        Collider2D myCollider = GetComponent<Collider2D>();
        if (myCollider == null)
        {
            CustomDebug.LogError("Enemy did not find collider", Categories.Fighters.Enemies.Root);
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
        
        m_isTarget = isTarget;
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
        m_intentionPicker.SetMoves(moves);
    }

    public void Stun()
    {
        m_stuned = true;
        CallOnIntentionDetermined(Intention.STUNED, "stuned");
    }
}

