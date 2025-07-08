using System;
using UnityEngine;

[Serializable]
public abstract class ChimeraHead : IHaveIntention
{
    public abstract event Action<Intention, string> OnIntentionDetermined;
    
    [SerializeField] private Transform m_headPosition;
    [SerializeField] private Vector3 m_rootOffsetFromHead;
    [SerializeField] protected ColliderMatcher m_colliderMatcher = new ColliderMatcher();
    [SerializeField] protected BaseEnemy.MoveData[] m_movesData;

    protected IDetermineIntention m_intentionDeterminer;
    protected IChimeraHeadStunBehaviour m_stun;
    protected ITauntBehaviour m_taunt;
    protected Chimera m_mind;

    protected BaseEnemy.MoveData? m_nextMoveData;

    public virtual void Config()
    {
    }

    public virtual void TakeDamage(int damage)
    {
        m_stun.Stun(damage, this);
    }

    public virtual void TurnStarted()
    {
        m_taunt.TurnChanged();
    }
    
    public virtual void TurnEnded()
    {
        m_stun.TurnChanged();
    }

    public virtual void Stun()
    {
        BaseEnemy.MoveData stun = new BaseEnemy.MoveData("Stunned");
        m_nextMoveData = stun;
        ShowIntention();
    }

    public virtual void EnemyTurnOver()
    {
        m_stun.TurnChanged();
    }

    public virtual void DetermineIntention()
    {
        m_nextMoveData = m_intentionDeterminer.DetermineIntention();
    }

    public abstract void ShowIntention();

    public abstract string GetAnimation();

    public abstract void ExecuteIntention(Action finishCallBack);

    public virtual void ReceiveTaunt()
    {
        m_taunt.ReceiveTaunt(m_colliderMatcher.Collider);
    }

    public bool IsMyCollider(Collider2D Targetcollider)
    {
        return m_colliderMatcher.IsMyCollider(Targetcollider);
    }

    public Vector3 GetRootPosition()
    {
        return m_headPosition.position + m_rootOffsetFromHead;
    }

    public Vector3 GetHeadPosition()
    {
        return m_headPosition.position;
    }
}