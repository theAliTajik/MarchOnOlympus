using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : BaseEnemy
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

    [SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] private PriestMovesData m_data;

    private Fighter m_warLvl2;
    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < m_movesDatas.Length; i++)
        {
            MoveData md = m_movesDatas[i];
            m_moves.Add(md, md.chance);
        }

        ConfigFighterHP();
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

    public override void DetermineIntention()
    {
        RandomIntentionPicker(m_moves);
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();
        switch (m_nextMove.clientID)
        {
            case "Restore":
                CallOnIntentionDetermined(Intention.BLOCK, m_nextMove.description);
                break;
            case "Rejuvenation":
            CallOnIntentionDetermined(Intention.BUFF, m_nextMove.description);
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
            case "Restore":
                m_animation.Play(ANIM_POISON_FX, finishCallback);
                Fighter lowestHP = FindLowestHP();
                if (lowestHP) GameActionHelper.HealFighter(lowestHP, m_data.Move1Restore);
                break;
            case "Rejuvenation":
                m_animation.Play(ANIM_POISON_FX, finishCallback);
                m_warLvl2 = FindWarriorLevel2();
                if (m_warLvl2)
                {
                    GameActionHelper.HealFighter(m_warLvl2, m_data.Move2Rejuvenation);
                    GameplayEvents.GamePhaseChanged += GamePhaseChanged;
                }
				break;
        }

    }

    private void GamePhaseChanged(EGamePhase obj)
    {
        if (obj != EGamePhase.ENEMY_TURN_START)
        {
            return;
        }
        
        if (m_warLvl2)
        {
            GameActionHelper.HealFighter(m_warLvl2, m_data.Move2Rejuvenation);
            GameplayEvents.GamePhaseChanged -= GamePhaseChanged;
        }
    }

    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

    private Fighter FindWarriorLevel2()
    {
        List<Fighter> allEnemies = GameInfoHelper.GetAllEnemies();
        return allEnemies.Find(enemy => enemy.GetType().Name == "WarriorLevel2");
    }

    private Fighter FindLowestHP()
    {
        List<Fighter> allEnemies = GameInfoHelper.GetAllEnemies();
        if (allEnemies == null || allEnemies.Count == 0)
        {
            Debug.Log("find lowset hp returned null");
            return null;
        }

        Fighter lowest = allEnemies[0];
        foreach (var enemy in allEnemies)
        {
            if ((float)enemy.HP.Current / enemy.HP.Max < (float)lowest.HP.Current / lowest.HP.Max)
                lowest = enemy;
        }

        return lowest;
    }
}