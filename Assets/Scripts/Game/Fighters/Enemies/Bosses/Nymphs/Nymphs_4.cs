using System;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Nymphs_4 : BaseNymph
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
    
    [SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] private MoveData m_submergedMoveData;
    [SerializeField] private Nymphs_4_MovesData m_data;

  
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
        m_animation.Play(ANIM_02_WOUNDED);
    }

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_04_DEATH);
    }

    public override void DetermineIntention()
    {
        if (!m_isTarget)
        {
            m_nextMove = m_submergedMoveData;
            ShowIntention();
            return;
        }
        
        RandomIntentionPicker();
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();
        
        if (m_isLastAlive)
        {
            CallOnIntentionDetermined(Intention.ATTACK, m_lastNymphMoveData.Description);
            return;
        }
        
        switch (m_nextMove.clientID)
        {
            case "Restore":
                CallOnIntentionDetermined(Intention.BLOCK, m_nextMove.description);
                break;
            case "submerged":
                CallOnIntentionDetermined(Intention.STUNED, m_nextMove.description);
                break;
        }
    }

    public override void ExecuteAction(Action finishCallback)
    {
        // play intention
        base.ExecuteAction(finishCallback);

        // Debug.Log("this action is played: " + m_nextMove.clientID);
        StartCoroutine(WaitAndExecute(finishCallback));
    }

    private int m_numOfCasts;
    private IEnumerator WaitAndExecute(Action finishCallback)
    {
        if (m_stuned)
        {
            m_stuned = false;
            finishCallback?.Invoke();
            yield break;
        }

        if (m_isLastAlive)
        {
            m_lastNymphAction.Execute(m_lastNymphMoveData, this);
            m_animation.Play(ANIM_05_ATTACK, finishCallback);
            yield break;
        }

        m_numOfCasts++;
        bool animFinished = false;
        switch (m_nextMove.clientID)
        {
            case "Restore":
                m_animation.Play(ANIM_05_ATTACK, () => { animFinished = true; } );
                List<Fighter> enemies = GameInfoHelper.GetAllEnemies();
                foreach (Fighter fighter in enemies)
                    fighter.Heal(m_data.Move1Restore);
				break;
            case "submerged":
                finishCallback?.Invoke();
                break;
        }

        yield return new WaitUntil(() => animFinished);

        if (m_doesCastTwice && m_numOfCasts < 2)
        {
            StartCoroutine(WaitAndExecute(finishCallback));
            yield break;
        }
        m_numOfCasts = 0;
        finishCallback?.Invoke();
    }
    
    public override void SetCanBeTarget(bool isTarget)
    {
        base.SetCanBeTarget(isTarget);
        DetermineIntention();
    }

    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }
}