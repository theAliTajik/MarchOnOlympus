using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class SoulMaster : BaseEnemy
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

    public Action<MoveData> OnSoulIntentionDetermined;
    
    [SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] protected MoveData[] m_attackMovesDatas;
    
    [SerializeField] private SoulMasterMovesData m_data;

    private int m_numOfTurnsWhereSoulsLessThanOne = 0;

    private int m_numOfSoulsAlive = 0;
    
    protected override void Awake()
    {
        base.Awake();


        for (int i = 0; i < m_attackMovesDatas.Length; i++)
        {
            MoveData md = m_attackMovesDatas[i];
            m_moves.Add(md, md.chance);
        }
        
        ConfigFighterHP();
        
        HP.SetTrigger(m_data.Phase1PercentageTrigger);
        HP.SetTrigger(m_data.Phase2PercentageTrigger);

        HP.OnPercentageTrigger += OnHPPercentageTriggred;
    }

    private void OnHPPercentageTriggred(FighterHP.TriggerPercentage percent)
    {
        SpawnSouls(2);
    }
    



    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(ANIM_WOUND1);
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

    public void OnSoulDeath()
    {
        m_numOfSoulsAlive--;
    }

    public override void DetermineIntention()
    {
        if (m_numOfSoulsAlive < 1)
        {
            m_nextMove = m_movesDatas[0];

            if (m_numOfTurnsWhereSoulsLessThanOne >= 2 && m_numOfTurnsWhereSoulsLessThanOne <= 3) 
            {
                m_nextMove = m_movesDatas[3];
            }
        }
        else
        {
            RandomIntentionPicker(m_moves);
        }
        OnSoulIntentionDetermined?.Invoke(m_nextMove);
        ShowIntention();
    }
    
    public override void ShowIntention()
    {
        base.ShowIntention();
        switch (m_nextMove.clientID)
        {
            case "SpawnSouls":
                CallOnIntentionDetermined(Intention.BUFF, m_nextMove.description);
                break;
            case "BuffRestore":
                CallOnIntentionDetermined(Intention.BUFF, m_nextMove.description);
                break;
            case "Spawn&Buff":
                CallOnIntentionDetermined(Intention.BUFF, m_nextMove.description);
                break;
            case "Bleed":
                CallOnIntentionDetermined(Intention.CAST_DEBUFF, m_nextMove.description);
                break;
            case "HitPiercing":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "HitDaze":
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


        switch (m_nextMove.clientID)
        {
            case "SpawnSouls":
                m_animation.Play(ANIM_POISON_FX, finishCallback);
                SpawnSouls(m_data.Move1NumOfSoulsToSpawn);
                break;
            case "BuffRestore":
                m_animation.Play(ANIM_POISON_FX, finishCallback);
                GameActionHelper.AddMechanicToFighter(this, m_data.Move2StrGain, MechanicType.STRENGTH);
                Heal(m_data.Move2Restore);
                break;
            case "Spawn&Buff":
                m_animation.Play(ANIM_POISON_FX, finishCallback);
                SpawnSouls(m_data.Move1NumOfSoulsToSpawn);
                GameActionHelper.AddMechanicToFighter(this, m_data.Move2StrGain, MechanicType.STRENGTH);
                Heal(m_data.Move2Restore);
                break;
            case "Bleed":
                m_animation.Play(ANIM_POISON_FX, finishCallback);
                GameActionHelper.AddMechanicToPlayer(m_data.Move3BleedGain, MechanicType.BLEED);
                break;
            case "HitPiercing":
                m_animation.Play(ANIM_ATTACK_1, finishCallback);
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move4Damage, true);
                break;
            case "HitDaze":
                m_animation.Play(ANIM_ATTACK_1, finishCallback);
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move5Damage);
                GameActionHelper.AddMechanicToPlayer(m_data.Move5DazeGain, MechanicType.DAZE);
                break;
        }
        
    }


    public void SpawnSouls(int numOfSouls)
    {
        for (int i = 0; i < numOfSouls; i++)
        {
            SpawnSoul();
        }
    }
    public void SpawnSoul()
    {
        GameplayEvents.SendSpawnBoss("Soul");
        m_numOfSoulsAlive++;
    }
    
    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

}
