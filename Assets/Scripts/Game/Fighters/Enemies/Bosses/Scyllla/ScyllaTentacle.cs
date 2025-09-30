
using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Serialization;

public class ScyllaTentacle : MonoBehaviour, IDamageable, IHaveHP, IDetermineIntention, IHaveIntention, IExecuteIntention, IColliderMatcher
{
    #region Animations

    private const string ANIM_01_IDLE = "01_idle";
    private const string ANIM_02_WOUNDED = "02_Wounded";
    private const string ANIM_03_WOUNDED_IDLE = "03_Wounded_idle";
    private const string ANIM_04_DEATH = "04_Death";
    private const string ANIM_05_ATTACK = "05_Attack";
    private const string ANIM_CAST_CLON = "Cast_Clon";
    private const string ANIM_IDLECLON = "idleClon";
    private const string ANIM_WOUNDED_CLON = "Wounded_Clon";
    private const string ANIM_WOUNDED_IDLE_CLON = "Wounded_idle_Clon";
    
    #endregion
    
    public event Action<ScyllaTentacle> OnDeath;
    public event Action<int> OnDamage;
    public event Action<Intention, string> OnIntentionDetermined;

    [SerializeField] private FighterHP m_fighterHP;
    [SerializeField] private AnimatorHelper m_animation;
    [SerializeField] protected ScyllaTentaclesMovesData m_data;
    [SerializeField] private BaseEnemy.MoveData[] m_moves;
    
    protected Scylla m_mind;

    private IDetermineIntention m_intentionDeterminer = new RandomIntentionDeterminer();
    

    private IDamageable m_damageable;

    [SerializeField] private Transform m_headPosition;
    [SerializeField] private Transform m_rootPosition;
    
    [SerializeField] private ColliderMatcher m_colliderMatcher;

    private BaseEnemy.MoveData? m_nextMove;

    public void Config(Scylla mind)
    {
        SetMoves(m_moves);
        m_mind = mind;
        ConfigFighterHP(m_data.HP);
        
        m_damageable = new EnemyDamageBehaviour(m_mind);
        m_damageable.OnDamage += m_fighterHP.TakeDamage;
        m_fighterHP.Death += OnHPDeath;
        

        if (m_animation == null)
        {
            m_animation = GetComponent<AnimatorHelper>();
        }
    }

    public Fighter.DamageContext TakeDamage(int damage, Fighter sender, bool doesReturnToSender = true,
        bool isArmorPiercing = false,
        Fighter.DamageContext damageContext = null)
    {
        return m_damageable.TakeDamage(damage, sender, isArmorPiercing);
    }

    public void SetMoves(BaseEnemy.MoveData[] moves)
    {
        m_moves = moves;
        m_intentionDeterminer.SetMoves(m_moves);
    }

    public virtual BaseEnemy.MoveData? DetermineIntention()
    {
        m_nextMove = m_intentionDeterminer.DetermineIntention();
        ShowIntention();
        
        return m_nextMove;
    }

    public void ShowIntention()
    {
        switch (m_nextMove.Value.clientID)
        {
            case "hit":
                OnIntentionDetermined?.Invoke(Intention.ATTACK, m_nextMove.Value.description);
                break;
            case "block":
                OnIntentionDetermined?.Invoke(Intention.BLOCK, m_nextMove.Value.description);
                break;
            case "alive":
                OnIntentionDetermined?.Invoke(Intention.CAST_DEBUFF, m_nextMove.Value.description);
                break;
        }
    }
    
    public IEnumerator ExecuteIntention(Action finishCallback)
    {
        switch (m_nextMove.Value.clientID)
        {
            case "hit":
                yield return WaitForAnimation(ANIM_05_ATTACK, finishCallback);
                int damage = m_data.Move1Damage;
                damage += m_data.Move1Damage * m_mind.GetBossPhase();
                GameActionHelper.DamagePlayer(m_mind, damage);
                break;
            case "block":
                yield return WaitForAnimation(ANIM_CAST_CLON, finishCallback);
                int block = m_data.Move3Block;
                block += m_data.Move3Block * m_mind.GetBossPhase();
                GameActionHelper.AddMechanicToOwner(m_mind, block, MechanicType.BLOCK);
                break;
            case "alive":
                finishCallback?.Invoke();
                break;
        }
        yield break;
    }

    public virtual void OnHPDeath()
    {
        m_animation.Play(ANIM_04_DEATH);
        OnIntentionDetermined?.Invoke(Intention.SLEEP, "Dead");
        OnDeath?.Invoke(this);
    }

    public Vector3 GetRootPosition()
    {
        return m_rootPosition.position;
    }

    public Vector3 GetHeadPosition()
    {
        return m_headPosition.position;
    }

    public FighterHP GetHP()
    {
        return m_fighterHP;
    }

    public bool IsMyCollider(Collider2D collider)
    {
        return m_colliderMatcher.IsMyCollider(collider);
    }

    public void ConfigFighterHP(int maxHp)
    {
        m_fighterHP.SetMax(maxHp);
        m_fighterHP.ResetHP();
    }
    
    public virtual void Revive()
    {
        m_animation.Play(ANIM_01_IDLE);
        m_fighterHP.ResetHP();
        DetermineIntention();
        ShowIntention();
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
}
