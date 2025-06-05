using System;
using Game;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class DaggerMaster : BaseEnemy
{
    #region Animations
    
    private const string ANIM_IDDLE_1 = "Iddle_1";
    private const string ANIM_ATTACK_1 = "Attack_1";
    private const string ANIM_ATTACK_2 = "Attack_2";
    private const string ANIM_ATTACK_3 = "Attack_3";
    private const string ANIM_ATTACK_4 = "Attack_4";
    private const string ANIM_ATTACK_5 = "Attack_5";
    private const string ANIM_ATTACK_6 = "Attack_6";
    private const string ANIM_DAGGER_APPEARNCE1 = "Dagger Appearnce1";
    private const string ANIM_DAGGER_APPEARNCE2 = "Dagger Appearnce2";
    private const string ANIM_DAGGER_APPEARNCE3 = "Dagger Appearnce3";
    private const string ANIM_DAGGER_APPEARNCE4 = "Dagger Appearnce4";
    private const string ANIM_DAGGER_APPEARNCE5 = "Dagger Appearnce5";
    private const string ANIM_DAGGER_APPEARNCE6 = "Dagger Appearnce6";
    private const string ANIM_DEATH = "Death";
    private const string ANIM_IDDLE_2 = "Iddle_2";
    private const string ANIM_IDDLE_3 = "Iddle_3";
    private const string ANIM_IDDLE_4 = "Iddle_4";
    private const string ANIM_IDDLE_5 = "Iddle_5";
    private const string ANIM_IDDLE_6 = "Iddle_6";
    private const string ANIM_POISION_ATTACK_1 = "Poision_Attack_1";
    private const string ANIM_POISION_ATTACK_2 = "Poision_Attack_2";
    private const string ANIM_POISION_ATTACK_3 = "Poision_Attack_3";
    private const string ANIM_POISION_ATTACK_4 = "Poision_Attack_4";
    private const string ANIM_POISION_ATTACK_5 = "Poision_Attack_5";
    private const string ANIM_POISION_ATTACK_6 = "Poision_Attack_6";
    private const string ANIM_POISON_FX = "Poison_Fx";
    private const string ANIM_WOUND1 = "Wound1";
    private const string ANIM_WOUND2 = "Wound2";
    private const string ANIM_WOUND3 = "Wound3";
    private const string ANIM_WOUND4 = "Wound4";
    private const string ANIM_WOUND5 = "Wound5";
    private const string ANIM_WOUND6 = "Wound6";
    
    #endregion


    #region Moves

    private const string Move_1 = "first";
    private const string Move_2 = "second";
    private const string Move_3 = "third";

    #endregion
    
    public class Dagger
    {
    }
    
    [SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] private DaggerMasterData m_data;

    private const int m_minDaggers = 0;
    
    private string m_currectDaggerAppearnceAnim;
    private string m_currectAttackAnim;
    private string m_currectWoundAnim;
    private string m_currectPoisonAnim;
    private float m_atackAnimDelayBeforeHit;
    
    private int m_currentMove = -1;
    private bool m_strNextTurn = false;
    private bool m_firstTurn = true;
    
    private List<Dagger> m_daggers = new List<Dagger>();
    
    protected override void Awake()
    {
        base.Awake();


        for (int i = 0; i < m_movesDatas.Length; i++)
        {
            MoveData md = m_movesDatas[i];
            m_moves.Add(md, md.chance);
        }
        
        ConfigFighterHP();
        
        HP.SetTrigger(m_data.Phase1PercentageTrigger);
        HP.SetTrigger(m_data.Phase2PercentageTrigger);

        HP.OnPercentageTrigger += OnHPPercentageTriggred;
    }

    private void OnHPPercentageTriggred(FighterHP.TriggerPercentage percent)
    {
        Debug.Log("perecent: " + percent);
        if (percent == m_data.Phase1PercentageTrigger)
        {
            m_strNextTurn = true;
        }

        if (percent == m_data.Phase2PercentageTrigger)
        {
            AddDaggers(m_data.AddedDaggersInPhase2);
        }
    }

    private void Start()
    {
        GameplayEvents.DaggerMasterCardPlayed += OnDaggerMasterCardPlayed;
    }

    private void OnDestroy()
    {
        GameplayEvents.DaggerMasterCardPlayed -= OnDaggerMasterCardPlayed;
    }

    private void OnDaggerMasterCardPlayed(DaggerMasterCard card)
    {
        RemoveDaggers(card.DaggerRemoveAmount);
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(m_currectWoundAnim);
    }

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_DEATH);
    }
    

    public override void DetermineIntention()
    {
        if (m_currentMove >= 2)
        {
            m_currentMove = 0;
        }
        else
        {
            m_currentMove++;
        }
        m_nextMove = m_movesDatas[m_currentMove];
        
        ShowIntention();
    }
    
    public override void ShowIntention()
    {
        base.ShowIntention();
        switch (m_nextMove.clientID)
        {
            case Move_1:
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case Move_2:
                CallOnIntentionDetermined(Intention.BUFF, m_nextMove.description);
                break;
            case Move_3:
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
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
        
        if ((m_nextMove.clientID == Move_1 || m_nextMove.clientID == Move_3) && m_strNextTurn)
        {
            m_strNextTurn = false;
         
            MechanicsManager.Instance.AddMechanic(new StrenghtMechanic(m_data.FirstPhaseStr, this), this);
        }

        AddDaggers(m_data.AddedDaggersEachTurn);
        
        switch (m_nextMove.clientID)
        {
            case Move_1:
                m_animation.Play(m_currectAttackAnim);
                yield return new WaitForSeconds(m_atackAnimDelayBeforeHit);
                CombatManager.Instance.Player.TakeDamage(m_data.Move1Damage, this, true);
                
                CombatManager.Instance.SpawnCard(m_data.Move1Card, CardStorage.DRAW_PILE);
                break;
            case Move_2:
                yield return WaitForAnimation(ANIM_POISON_FX);
                m_animation.Play(m_currectDaggerAppearnceAnim);
                int bleed = m_daggers.Count * m_data.Move2BleedPerDagger;
                GameActionHelper.AddMechanicToPlayer(bleed, MechanicType.BLEED);
                
                break;
            case Move_3:
                bool attackAnimDone = false;
                m_animation.Play(m_currectAttackAnim, () => attackAnimDone = true);
                yield return new WaitForSeconds(m_atackAnimDelayBeforeHit);
                CombatManager.Instance.Player.TakeDamage(m_data.Move3DamagePerDagger * m_daggers.Count, this, true);

                yield return new WaitUntil(() => attackAnimDone);
                RemoveAllDaggers();
                
                break;
        }


        m_firstTurn = false;
        yield return new WaitForSeconds(0.1f);
        finishCallback?.Invoke();
    }
    
    
    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

    private void AddDaggers(int amount)
    {
        if (m_daggers.Count > m_data.MaxDaggers)
        {
            Debug.Log("Max Daggers");
            return;
        }

        if (m_daggers.Count + amount > m_data.MaxDaggers)
        {
            Debug.Log("Amount makes daggers more than max setting daggers to max");
            amount = m_data.MaxDaggers - m_daggers.Count;
        }
        for (int i = 0; i < amount; i++)
        {
            m_daggers.Add(new Dagger());
        }
        SetAnimations(m_daggers.Count);
        m_animation.Play(m_currectDaggerAppearnceAnim);
        // Debug.Log("dagger count: " + m_daggers.Count);
    }

    private void RemoveDaggers(int amount)
    {
        if (m_daggers.Count <= m_minDaggers)
        {
            Debug.Log("Min Daggers");
            return;
        }
        
        if (m_daggers.Count - amount < m_minDaggers)
        {
            Debug.Log("Amount makes daggers less than min setting daggers to min");
            amount = m_daggers.Count - m_minDaggers;
        }
        m_daggers.RemoveRange(0, amount);
        SetAnimations(m_daggers.Count);
        m_animation.Play(m_currectDaggerAppearnceAnim);
        Debug.Log("dagger count: " + m_daggers.Count);
    }
    private void RemoveAllDaggers()
    {
        RemoveDaggers(m_data.MaxDaggers);
    }
    
    
    private void SetAnimations(int numOfDaggers)
    {
        switch (numOfDaggers)
        {
            case 0:
            case 1:
                m_currectDaggerAppearnceAnim = ANIM_DAGGER_APPEARNCE1;
                m_currectAttackAnim = ANIM_ATTACK_1;
                m_atackAnimDelayBeforeHit = 0.8f;
                m_currectWoundAnim = ANIM_WOUND1;
                m_currectPoisonAnim = ANIM_POISION_ATTACK_1;
                break;
            case 2:
                m_currectDaggerAppearnceAnim = ANIM_DAGGER_APPEARNCE2;
                m_currectAttackAnim = ANIM_ATTACK_2;
                m_atackAnimDelayBeforeHit = 0.9f;
                m_currectWoundAnim = ANIM_WOUND2;
                m_currectPoisonAnim = ANIM_POISION_ATTACK_2;
                break;
            case 3:
                m_currectDaggerAppearnceAnim = ANIM_DAGGER_APPEARNCE3;
                m_currectAttackAnim = ANIM_ATTACK_3;
                m_atackAnimDelayBeforeHit = 0.9f;
                m_currectWoundAnim = ANIM_WOUND3;
                m_currectPoisonAnim = ANIM_POISION_ATTACK_3;
                break;
            case 4:
                m_currectDaggerAppearnceAnim = ANIM_DAGGER_APPEARNCE4;
                m_currectAttackAnim = ANIM_ATTACK_4;
                m_atackAnimDelayBeforeHit = 0.9f;
                m_currectWoundAnim = ANIM_WOUND4;
                m_currectPoisonAnim = ANIM_POISION_ATTACK_4;
                break;
            case 5:
                m_currectDaggerAppearnceAnim = ANIM_DAGGER_APPEARNCE5;
                m_currectAttackAnim = ANIM_ATTACK_5;
                m_atackAnimDelayBeforeHit = 1.1f;
                m_currectWoundAnim = ANIM_WOUND5;
                m_currectPoisonAnim = ANIM_POISION_ATTACK_5;
                break;
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
                m_currectDaggerAppearnceAnim = ANIM_DAGGER_APPEARNCE6;
                m_currectAttackAnim = ANIM_ATTACK_6;
                m_atackAnimDelayBeforeHit = 1.3f;
                m_currectWoundAnim = ANIM_WOUND6;
                m_currectPoisonAnim = ANIM_POISION_ATTACK_6;
                break;
            default:
                Debug.Log("invalid num of daggers");
                break;
        }
    }
    
}
