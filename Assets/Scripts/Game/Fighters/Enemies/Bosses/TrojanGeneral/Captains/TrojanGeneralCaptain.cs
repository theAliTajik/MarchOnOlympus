using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Game;
using UnityEngine;
using UnityEngine.Serialization;

public class TrojanGeneralCaptain : BaseEnemy
{
    #region Anims
        
    private const string ANIM__WOUND_COMONWARRIOR = "_Wound_ComonWarrior";
    private const string ANIM_ABILITY_COMONWARRIOR = "Ability_ComonWarrior";
    private const string ANIM_ATTACK_COMONWARRIOR = "Attack_ComonWarrior";
    private const string ANIM_DEAD_COMONWARRIOR = "Dead_ComonWarrior";
    
    #endregion
    

    [SerializeField] protected MoveData[] m_movesDatas;
    
    [FormerlySerializedAs("m_SpecialmovesDatas")] [SerializeField] protected MoveData[] m_specialmovesDatas;

    protected int m_hp;
    protected int m_actionData;

    protected int m_numOfTurnsChanneledRemaining;
    
    /*
    [SerializeField] private Captain1MovesData m_data1;
    [SerializeField] protected MoveData[] m_movesDatas1;
    [SerializeField] private Captain2MovesData m_data2;
    [SerializeField] protected MoveData[] m_movesDatas2;
    [SerializeField] private Captain3MovesData m_data3;
    [SerializeField] protected MoveData[] m_movesDatas3;

    
    public enum CaptainSelected
    {
        ONE,
        TWO,
        THREE
    }

    */
    
    protected override void Awake()
    {
        base.Awake();

        ConfigFighterHP();

        for (int i = 0; i < m_movesDatas.Length; i++)
        {
            MoveData md = m_movesDatas[i];
            m_moves.Add(md, md.chance);
        }
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(ANIM__WOUND_COMONWARRIOR);
    }
    

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_DEAD_COMONWARRIOR);
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
            case "Channel":
                string formatedDesc = string.Format(m_nextMove.description, m_numOfTurnsChanneledRemaining);
                CallOnIntentionDetermined(Intention.BUFF, formatedDesc);
                break;
            case "Str":
                CallOnIntentionDetermined(Intention.BUFF, m_nextMove.description);
                break;
            case "Thorns":
                CallOnIntentionDetermined(Intention.BUFF, m_nextMove.description);
                break;
            case "Block":
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

        Fighter general = FindGeneral();
        switch (m_nextMove.clientID)
        {
            case "Channel":
                m_animation.Play(ANIM_ABILITY_COMONWARRIOR, finishCallback);
                m_numOfTurnsChanneledRemaining--;
                break;
            case "Str":
                if (general == null)
                {
                    finishCallback?.Invoke();
                    yield break;
                }

                m_animation.Play(ANIM_ABILITY_COMONWARRIOR, finishCallback);
                GameActionHelper.AddMechanicToFighter(general, m_actionData, MechanicType.STRENGTH);

                break;
            case "Thorns":
                if (general == null)
                {
                    finishCallback?.Invoke();
                    yield break;
                }

                m_animation.Play(ANIM_ABILITY_COMONWARRIOR, finishCallback);
                GameActionHelper.AddMechanicToFighter(general, m_actionData, MechanicType.THORNS);

                break;
            case "Block":
                if (general == null)
                {
                    finishCallback?.Invoke();
                    yield break;
                }

                m_animation.Play(ANIM_ABILITY_COMONWARRIOR, finishCallback);
                GameActionHelper.AddMechanicToFighter(general, m_actionData, MechanicType.BLOCK);
                break;
        }
    }


    private TrojanGeneral FindGeneral()
    {
        List<Fighter> allEnemies = GameInfoHelper.GetAllEnemies();
        TrojanGeneral general = allEnemies.Find(enemy => enemy.GetType().Name == "TrojanGeneral") as TrojanGeneral;
        
        return general;
    }

    /*
    public void SetCaptain(CaptainSelected captainSelected)
    {
        switch (captainSelected)
        {
            case CaptainSelected.ONE:
                Hp = m_data1.HP;
                data = m_data1.Move1Str;
                break;
            case CaptainSelected.TWO:
                Hp = m_data2.HP;
                data = m_data2.Move1Thorns;
                break;
            case CaptainSelected.THREE:
                Hp = m_data3.HP;
                data = m_data3.Move1Block;
                break;
        }
    }
    */
    
    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_hp);
        m_fighterHP.ResetHP();
    }

}
