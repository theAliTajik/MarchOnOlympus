using System;
using Game;
using UnityEngine;

public class PlayerController : Fighter
{

    #region Animations
    
    private const string ANIM_01_FIER_COMAND = "01_Fier Comand";
    private const string ANIM_01_IDDLE_NORMAL = "01_Iddle_Normal";
    private const string ANIM_01_WOUND = "01_Wound";
    private const string ANIM_02_ATTACK = "02_Attack";
    private const string ANIM_02_ATTACK2 = "02_Attack2";
    private const string ANIM_02_IDDLEWOUND = "02_IddleWound";
    private const string ANIM_03_IDDLEBLOCK = "03_IddleBlock";
    private const string ANIM_04_DEATH = "04_Death";
    private const string ANIM_VOLLEY = "Volley";
    private const string ANIM_VOLLEY2 = "Volley2";
    private const string ANIM_VOLLEY3 = "Volley3";
    private const string ANIM_VOLLEY4 = "Volley4";    
    
    #endregion

    
    [SerializeField] protected AnimatorHelper m_animator;
    [SerializeField] private int m_health;
    
    private PlayerInventory m_PlayerInventory;
    private const string AnimHasBlockBoolName = "HasBlock";
    
    protected virtual void Start()
    {
        m_mechanicsList = MechanicsManager.Instance.GetMechanicsList(this);
        if (m_mechanicsList == null)
        {
            CustomDebug.LogError("Player is missing mechanic list", Categories.Fighters.Player.Root);
            return;
        }
        m_mechanicsList.OnMechanicUpdated += OnMechanicUpdated;
        m_mechanicsList.OnMechanicRemoved += OnMechanicRemoved;
    }

    private void OnMechanicRemoved(MechanicType obj)
    {
        if (obj != MechanicType.BLOCK) return;
        
        m_animator.SetBool(AnimHasBlockBoolName, false);
    }

    private void OnMechanicUpdated(MechanicType obj)
    {
        if (obj != MechanicType.BLOCK) return;

        m_animator.SetBool(AnimHasBlockBoolName, true);
    }

    public virtual float PlayAttackAnimation(Action finishCallBack)
    {
        int rand = UnityEngine.Random.Range(0, 2);
        if (GameInfoHelper.CheckIfLastCardPlayedWas("Dual Strike", true) || GameInfoHelper.CheckIfLastCardPlayedWas("Obliterate", true) && CombatManager.Instance.CurrentStance == Stance.BERSERKER)
        {
            m_animator.Play(ANIM_02_ATTACK2, finishCallBack);
            return 1.6f;
        }
        switch (rand)
        {
            case 0:
                m_animator.Play(ANIM_02_ATTACK, finishCallBack);
                return 2.4f;
                break;
            case 1:
                m_animator.Play(ANIM_01_FIER_COMAND, finishCallBack);
                return 1.5f;
                break;
        }
        return 0.5f;
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        base.OnTookDamage(damage, isCritical);
        m_animator.Play(ANIM_01_WOUND);
        GameplayEvents.SendGamePhaseChanged(EGamePhase.PLAYER_DAMAGED);
    }

    public override void Heal(int heal)
    {
        base.Heal(heal);
        GameplayEvents.SendGamePhaseChanged(EGamePhase.PLAYER_HEALED);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        m_animator.Play(ANIM_04_DEATH);
    }
    
    public override Vector3 GetRootPosition()
    {
        return m_root.position;
    }

    public override Vector3 GetHeadPosition()
    {
        return m_head.position;
    }
    
    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_health);
        m_fighterHP.ResetHP();
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage(10, this, false);
        }
    }
#endif
}
