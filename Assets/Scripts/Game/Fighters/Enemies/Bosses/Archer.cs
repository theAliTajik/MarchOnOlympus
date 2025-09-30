using System;
using Game;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Archer : BaseEnemy
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
    [SerializeField] private ArcherMovesData m_data;
    
    private Diomedes m_diomedes;
    
    private bool m_isPlayingMove = false;
    
    protected override void Awake()
    {
        base.Awake();


        ConfigFighterHP();
        
SetMoves(m_movesDatas);

    }

    private void Start()
    {
        m_diomedes = findDiomedes();
        if (m_diomedes != null)
        {
            m_diomedes.Death += OnDiomedesDeath;
            m_diomedes.OnPhase1 += OnPhase1OfDiomedes;
            GameActionHelper.AddMechanicToFighter(this, 1, MechanicType.FORTIFIED, 1);
        }
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
        if (m_diomedes != null)
        {
            m_diomedes.Death -= OnDiomedesDeath;
            m_diomedes.OnPhase1 -= OnPhase1OfDiomedes;
        }
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_04_DEAD);
    }
    

    public override void DetermineIntention()
    {
        RandomIntentionPicker();
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();
        switch (m_nextMove.clientID)
        {
            case "Hit":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "HitTwice":
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

        m_isPlayingMove = true;
        Debug.Log("animation started");
        switch (m_nextMove.clientID)
        {
            case "Hit":
                CombatManager.Instance.Player.TakeDamage(m_data.Move1Damage, this, true);
                MechanicsManager.Instance.AddMechanic(new BleedMechanic(m_data.Move1Bleed, CombatManager.Instance.Player), this);
                yield return WaitForAnimation(ANIM_05_SHOOT, finishCallback);
                break;
            case "HitTwice":
                CombatManager.Instance.Player.TakeDamage(m_data.Move2Damage, this, true);
                yield return WaitForAnimation(ANIM_05_SHOOT);
                CombatManager.Instance.Player.TakeDamage(m_data.Move2Damage, this, true);
                yield return WaitForAnimation(ANIM_05_SHOOT, finishCallback);
                break;
        }

        Debug.Log("animation actualy finished");
        m_isPlayingMove = false;
        yield return null;
    }
    
    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }
    
    private Diomedes findDiomedes()
    {
        List<Fighter> allEnemies = GameInfoHelper.GetAllEnemies();
        return allEnemies.Find(enemy => enemy.GetType().Name == "Diomedes") as Diomedes;
    }

    private void OnDiomedesDeath(Fighter fighter)
    {
        StartCoroutine(waitForDeath());
    }

    private IEnumerator waitForDeath()
    {
        Debug.Log("diomedes died");
        yield return new WaitForSeconds(1.2f);
        yield return new WaitUntil(() => !m_isPlayingMove);
        Debug.Log("animation finished");
        //remove diomedes
        EnemiesManager.Instance.RemoveDeadEnemyBody(m_diomedes);
        // spawn diomedes level 2 
        EnemiesManager.Instance.SpawnBoss(m_data.DiomedesLevel2Id, true);

        // sacrifice yourself
        int health = m_fighterHP.Current;
        TakeDamage(health * 10, this , false, true);
    }

    private void OnPhase1OfDiomedes()
    {
        //remove fortify
        //GameActionHelper.RemoveMechanicGuard(this, MechanicType.FORTIFIED);
        //GameActionHelper.ReduceMechanicStack(this,1, MechanicType.FORTIFIED);
        
        
        GameActionHelper.AddMechanicToFighter(this, m_data.StrRecievedWhenWarriorsDeath, MechanicType.STRENGTH);
    }
    
}
