using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Spine;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : Fighter
{

    #region Animations
    
    private const string ANIM_01_FIER_COMAND = "01_Fier Comand";
    private const string ANIM_01_IDDLE_NORMAL = "01_Iddle_Normal";
    private const string ANIM_01_SOLO_COMAND = "01_Solo_Comand";
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

    
    [SerializeField] private AnimatorHelper m_animator;
    [SerializeField] private int m_health;
    private PlayerInventory m_PlayerInventory;
    

    public float PlayAttackAnimation(Action finishCallBack)
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
