using System;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherWave : BaseEnemy
{
    #region Animations
    
    private const string ANIM_01_IDLE = "01_idle";
    private const string ANIM_02_DAMMAGE = "02_Dammage";
    private const string ANIM_03_WOUND = "03_Wound";
    private const string ANIM_04_DEAD = "04_dead";
    private const string ANIM_05_SHOOT = "05_Shoot";
    private const string ANIM_06_CRITICAL_SHOT = "06_Critical_Shot";

    #endregion
    
    [SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] private ArcherWaveMovesData m_data;

    public override bool IsRequiredForCombatCompletion => false;

    private int m_remainigTurnsCountDown = -1;
    
    
    protected override void Awake()
    {
        base.Awake();

        SetMoves(m_movesDatas);
        
        ConfigFighterHP();
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(ANIM_02_DAMMAGE);
    }

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_04_DEAD);
    }
    

    public override void DetermineIntention()
    {
        if (m_remainigTurnsCountDown < 0)
        {
            m_remainigTurnsCountDown = m_data.Move1NumOfTurns;
        }

        if (m_remainigTurnsCountDown > 0)
        {
            m_nextMove = m_moves[0];
        }

        if (m_remainigTurnsCountDown == 0)
        {
            m_nextMove = m_moves[1];
        }
        
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();
        switch (m_nextMove.clientID)
        {
            case "countDown":
                CallOnIntentionDetermined(Intention.SLEEP,  string.Format(m_nextMove.description, m_remainigTurnsCountDown));
                break;
            case "hit":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
        }
    }

    public override void ExecuteAction(Action finishCallback)
    {
        base.ExecuteAction(finishCallback);

        Debug.Log("this action is played: " + m_nextMove.clientID);
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
            case "countDown":
                if (m_remainigTurnsCountDown > 0)
                {
                    m_remainigTurnsCountDown--;
                }

                if (m_remainigTurnsCountDown == 0)
                {
                    m_nextMove = m_movesDatas[1];
                }
                finishCallback?.Invoke();
                break;
            case "hit":
                m_animation.Play(ANIM_05_SHOOT, finishCallback);
                yield return new WaitForSeconds(1);
                Fighter player = GameInfoHelper.GetPlayer();
                GameActionHelper.DamageFighter(player, this, m_data.Move1Damage);
                m_nextMove = m_movesDatas[0];
                break;
        }

        yield return null;
    }
    
    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }
}
