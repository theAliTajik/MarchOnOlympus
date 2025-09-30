
using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct HydraHeadConfigData
{
    public BaseEnemy.MoveData[] Moves;
    public HydraHeadMovesData Data;

    public Hydra Mind;
    public int HP;
}

public class HydraHead : MonoBehaviour, IDamageable, IHaveHP, IDetermineIntention, IHaveIntention, IExecuteIntention, IColliderMatcher
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
    
    public event Action<HydraHead> OnDeath;
    public event Action<int> OnDamage;
    public event Action<Intention, string> OnIntentionDetermined;

    [FormerlySerializedAs("m_hp")] [SerializeField] private FighterHP m_fighterHP;
    [SerializeField] private AnimatorHelper m_animation;
    
    private Hydra m_mind;

    private IDetermineIntention m_intentionDeterminer = new RandomIntentionDeterminer();
    
    private BaseEnemy.MoveData[] m_moves;
    private HydraHeadMovesData m_data;

    private IDamageable m_damageable;

    [SerializeField] private Transform m_headPosition;
    [SerializeField] private Transform m_rootPosition;
    
    [SerializeField] private ColliderMatcher m_colliderMatcher;

    private BaseEnemy.MoveData? m_nextMove;

    public void Config(HydraHeadConfigData data)
    {
        SetMoves(data.Moves);
        m_data = data.Data;
        m_mind = data.Mind;
        ConfigFighterHP(data.HP);
        
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

    public BaseEnemy.MoveData? DetermineIntention()
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
            
            case "hitAcid":
                OnIntentionDetermined?.Invoke(Intention.ATTACK, m_nextMove.Value.description);
                break;
            case "acidDot":
                OnIntentionDetermined?.Invoke(Intention.CAST_DEBUFF, m_nextMove.Value.description);
                break;
        }
    }
    
    public IEnumerator ExecuteIntention(Action finishCallback)
    {
        switch (m_nextMove.Value.clientID)
        {
            case "hit":
                m_animation.Play(ANIM_05_ATTACK, finishCallback);
                int missingHeads = m_mind.GetMissingHeadCount();
                int damage = m_data.Move1Damage;
                damage += missingHeads * m_data.Move1DamageMultiplier;
                GameActionHelper.DamagePlayer(m_mind, damage);
                break;
            
            case "hitAcid":
                m_animation.Play(ANIM_05_ATTACK, finishCallback);
                IHaveMechanics player = GameInfoHelper.GetPlayer();
                
                int acidDotCount = GameInfoHelper.GetMechanicStack(player, MechanicType.ACIDICDOT);
                acidDotCount += GameInfoHelper.GetMechanicStack(player, MechanicType.ACIDICDOTTWO);

                CustomDebug.Log($"Acidic dot count of player was: {acidDotCount}", Categories.Fighters.Enemies.HydraHead);

                int d = acidDotCount * m_data.Move2AcidDotCountDamageMultiplier;
                GameActionHelper.DamagePlayer(m_mind, d);
                break;
            case "acidDot":
                m_animation.Play(ANIM_05_ATTACK, finishCallback);
                GameActionHelper.AddMechanicToPlayer(m_data.Move3AcidDotAmount, MechanicType.ACIDICDOT);
                break;
        }

        yield return new WaitForSeconds(1.5f);
        finishCallback?.Invoke();
        yield break;
    }

    public void OnHPDeath()
    {
        m_animation.Play(ANIM_04_DEATH);
        OnIntentionDetermined?.Invoke(Intention.SLEEP, "Dead");
        OnDeath?.Invoke(this);
    }

    public void Revive()
    {
        m_animation.Play(ANIM_01_IDLE);
        m_fighterHP.ResetHP();
        DetermineIntention();
        ShowIntention();
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
}

public interface IHaveHP
{
    FighterHP GetHP();
}
