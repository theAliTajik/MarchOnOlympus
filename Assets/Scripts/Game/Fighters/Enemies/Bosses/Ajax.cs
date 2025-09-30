using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using KaimiraGames;
using Spine.Unity;
using Spine;



public class Ajax : BaseEnemy
{
    private const string ANIM_01_IDDLE = "01_Iddle";
    private const string ANIM_06_ATTACKFULL2 = "06_AttackFull2";
    private const string ANIM_02_DAMAGE = "02_Damage";
    private const string ANIM_08_DEATH2 = "08_Death2";
    private const string ANIM_03_DEF = "03_Def";

    [SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] protected MoveData[] m_specialMovesDatas;

    [SerializeField] private AjaxMovesData m_data;

    private int m_DamageBonus = 0;
    private bool m_hasDamageBonus = false;
    private bool m_firstTimeReaching33PrecentOfHP = true;
    
    protected override void Awake()
    {
        base.Awake();

        ConfigFighterHP();

        SetMoves(m_movesDatas);
        
        HP.SetTrigger(m_data.Phase1PercentageUnTrigger);
        HP.SetTrigger(m_data.Phase1PercentageTrigger);
        HP.SetTrigger(m_data.Phase2PercentageTrigger);

        HP.OnPercentageTrigger += OnHPPercentageTriggred;
    }

    private void OnHPPercentageTriggred(FighterHP.TriggerPercentage percent)
    {
        if (percent == m_data.Phase1PercentageTrigger && !m_hasDamageBonus)
        {
            Debug.Log("at 66");
            //m_DamageBonus = m_data.DamageBonusAtPhase1;
            GameActionHelper.AddMechanicToFighter(this, m_data.DamageBonusAtPhase1, MechanicType.STRENGTH, 10);
            m_hasDamageBonus = true;
        }
        
        if (percent == m_data.Phase2PercentageTrigger)
        {
            Debug.Log("at 33");
            MechanicsManager.Instance.AddMechanic(new BlockMechanic(m_data.BlockGainedAtPhase2, this), this);
            m_firstTimeReaching33PrecentOfHP = false;
        }
        
        if(percent == m_data.Phase1PercentageUnTrigger && m_hasDamageBonus)
        {
            GameActionHelper.RemoveMechanicGuard(this, MechanicType.STRENGTH);
            GameActionHelper.ReduceMechanicStack(this, m_data.DamageBonusAtPhase1, MechanicType.STRENGTH);
            m_hasDamageBonus = false;
        }
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(ANIM_02_DAMAGE);
    }

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_08_DEATH2);
    }
    

    public override void DetermineIntention()
    {
        if (m_nextMove.clientID == "fortified")
        {
            ShowIntention();
            return;
        }
        //remove fortified from moves:
        
        RandomIntentionPicker();
        ShowIntention();
        
    }

    public override void ShowIntention()
    {
        base.ShowIntention();
        switch (m_nextMove.clientID)
        {
            case "pump":
                CallOnIntentionDetermined(Intention.BUFF, m_nextMove.description);
                break;
            case "fortified":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "2hit":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "hit10":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "block":
                CallOnIntentionDetermined(Intention.BLOCK, m_nextMove.description);
                break;
            case "StandStill":
                CallOnIntentionDetermined(Intention.STUNED, m_nextMove.description);
                break;
        }
    }


    public override void ExecuteAction(Action finishCallback)
    {
        // play intention
        base.ExecuteAction(finishCallback);

        //Debug.Log("this action is played: " + m_nextMove.clientID);
        StartCoroutine(WaitAndExecute(finishCallback));
    }
    

    private IEnumerator WaitAndExecute(Action finishCallback)
    {
        if (m_stuned)
        {
            m_stuned = false;
            finishCallback?.Invoke();
            yield break;
        }
        switch (m_nextMove.clientID)
        {
            case "pump":
                m_nextMove = m_specialMovesDatas[0];
                m_animation.Play(ANIM_03_DEF, finishCallback);
                break;
            case "fortified":
                MechanicsManager.Instance.AddMechanic(new FortifiedMechanic(m_data.FortifiedStackAmount, this), this);
                CombatManager.Instance.Player.TakeDamage(m_data.Move1Damage + m_DamageBonus, this, true);
                
                m_nextMove = m_movesDatas[0];
                m_animation.Play(ANIM_03_DEF, finishCallback);

                break;
            case "2hit":
                CombatManager.Instance.Player.TakeDamage(m_data.Hit2xDamage + m_DamageBonus, this, true);
                yield return WaitForAnimation(ANIM_06_ATTACKFULL2);
                CombatManager.Instance.Player.TakeDamage(m_data.Hit2xDamage + m_DamageBonus, this, true);
                m_animation.Play(ANIM_06_ATTACKFULL2, finishCallback);
                break;
            case "hit10":
                CombatManager.Instance.Player.TakeDamage(m_data.Move3Damage + m_DamageBonus, this, true);
                MechanicsManager.Instance.AddMechanic(new VulnerableMechanic(m_data.vulnerableStackAmount, CombatManager.Instance.Player), this);
                m_animation.Play(ANIM_06_ATTACKFULL2, () => finishCallback?.Invoke());
                break;
            case "block":
                MechanicsManager.Instance.AddMechanic(new BlockMechanic(m_data.BlockStackAmount, this), this);
                m_animation.Play(ANIM_03_DEF, finishCallback);
                break;
            case "StandStill":
                finishCallback?.Invoke();
                break;
        }
    }
    
    

    
    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }
}