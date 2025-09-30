using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using UnityEngine;

public class Paris : BaseEnemy
{
    #region Anims

    private const string ANIM_01_IDDLE = "01_Iddle";
    private const string ANIM_02_WOUND = "02_Wound";
    private const string ANIM_03_DEATH = "03_Death";
    private const string ANIM_04_ATTACK = "04_Attack";
    private const string ANIM_04_ATTACK2 = "04_Attack2";
    private const string ANIM_04_ATTACK3 = "04_Attack3";
    private const string ANIM_05_HEAL = "05_Heal";

    #endregion
    
    [SerializeField] protected MoveData[] m_movesDatas;
    
    [SerializeField] protected MoveData[] m_specialMovesData;
    
    [SerializeField] protected MoveData[] m_hectorsDeadMovesDatas;
    [SerializeField] private ParisMovesData m_data;
    
    private Hector m_hector;
    
    private bool m_isHectorDead;
    private bool m_hectorExisted;
    
    private bool m_runaway;
    
    protected override void Awake()
    {
        base.Awake();

        ConfigFighterHP();

SetMoves(m_movesDatas);
    }

    private void Start()
    {
        HP.SetTrigger(m_data.Phase1PercentageTrigger);

        HP.OnPercentageTrigger += OnHPPercentageTriggred;
    }

    private void OnHPPercentageTriggred(FighterHP.TriggerPercentage percentage)
    {
        Debug.Log("percentage triggered: :" + percentage.Percentage);
        m_runaway = true;
        m_nextMove = m_specialMovesData[0];
        ShowIntention();
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(ANIM_02_WOUND);
    }


    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_03_DEATH);
    }
    

    public override void DetermineIntention()
    {
        if (m_runaway == true)
        {
            return;
        }
        
        m_hector = FindHector();
        
        RandomIntentionPicker();
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();

        
        switch (m_nextMove.clientID)
        {
            case "Hit100":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "RestoreHector":
                CallOnIntentionDetermined(Intention.BUFF,  m_nextMove.description);
                break;
            case "Hit20":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "RunAway":
                CallOnIntentionDetermined(Intention.ESCAPE, m_nextMove.description);
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
            case "Hit100":
                m_animation.Play(ANIM_04_ATTACK2, finishCallback);
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move1Damage);
                break;
            case "RestoreHector":
                m_animation.Play(ANIM_05_HEAL, finishCallback);
                if (m_hector != null)
                {
                    m_hector.Heal(m_data.Move2RestoreHector);
                }
                break;
            case "Hit20":
                bool attackAnimFinished = false;
                m_animation.Play(ANIM_04_ATTACK, () => { attackAnimFinished = true; });
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move3Damage);
                yield return new WaitUntil(() => attackAnimFinished);
                GameActionHelper.DamageFighter(this, this, m_data.Move3DamageToSelf);
                finishCallback?.Invoke();
                break;
            case "RunAway":
                bool runAwayAnimFinished = false;
                m_animation.Play(ANIM_05_HEAL, () => { runAwayAnimFinished = true; });
                yield return new WaitUntil(() => runAwayAnimFinished);
                if (m_hector != null)
                {
                    GameActionHelper.AddMechanicToFighter(m_hector, m_data.Phase1StrGainToHector, MechanicType.STRENGTH);
                }
                
                yield return new WaitForSeconds(0.1f);
                MeshRenderer renderer = GetComponent<MeshRenderer>();
                renderer.enabled = false;
                
                finishCallback?.Invoke();
                
                EnemiesManager.Instance.RemoveDeadEnemyBody(this);
                yield return new WaitForSeconds(0.1f);
                Destroy(this);
                break;

        }
    }
    
    
    private Hector FindHector()
    {
        List<Fighter> allEnemies = GameInfoHelper.GetAllEnemies();
        Hector hector = allEnemies.Find(enemy => enemy.GetType().Name == "Hector") as Hector;
        if (hector != null)
        {
            hector.Death += OnHectorsDeath;
            m_hectorExisted = true;
        }
        return hector;
    }

    private void OnHectorsDeath(Fighter hector)
    {
        m_isHectorDead = true;
        m_moves.Clear();
        
        for (int i = 0; i < m_hectorsDeadMovesDatas.Length; i++)
        {
            MoveData md = m_hectorsDeadMovesDatas[i];
            m_moves.Add(md, md.chance);
        }
        DetermineIntention();
    }


    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

}
