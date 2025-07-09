using System;
using UnityEngine;

[Serializable]
public abstract class ChimeraHead : IHaveIntention, IDamageable
{
    #region Animations

    
	private const string ANIM_02_WOUND = "02_Wound";
	private const string ANIM_03_IDLE_WOUND = "03_Idle_Wound";
	private const string ANIM_04_LION_HEAD_ATTACK = "04_Lion_head_Attack";
	private const string ANIM_05_LION_HEAD_GROW = "05_Lion_head_Grow";
	private const string ANIM_06_GOAT_HEAD_SHOUT_CRIT = "06_Goat_Head_Shout_Crit";
	private const string ANIM_06_GOAT_HEAD_SHOUT_NORMAL = "06_Goat_Head_Shout_Normal";
	private const string ANIM_08_SERPENT_HEAD = "08_Serpent_Head";
	private const string ANIM_09_DEATH_ = "09_Death_";

    #endregion
    public abstract event Action<Intention, string> OnIntentionDetermined;
    public event Action<int> OnDamage;
    
    [SerializeField] private Transform m_headPosition;
    [SerializeField] private Vector3 m_rootOffsetFromHead;
    [SerializeField] protected ColliderMatcher m_colliderMatcher = new ColliderMatcher();
    [SerializeField] protected BaseEnemy.MoveData[] m_movesData;

    protected IDamageable m_damageable;
    protected IDetermineIntention m_intentionDeterminer;
    protected IChimeraHeadStunBehaviour m_stun;
    protected ITauntBehaviour m_taunt;
    protected Chimera m_mind;

    protected BaseEnemy.MoveData? m_nextMoveData;

    public virtual void Config()
    {
    }

    public virtual int TakeDamage(int damage, Fighter sender, bool doesReturnToSender, bool isArmorPiercing = false)
    {
        if (m_damageable != null)
        {
            damage = m_damageable.TakeDamage(damage, sender, isArmorPiercing);
        }
        m_stun.Stun(damage, this);
        return damage;
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