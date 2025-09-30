using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WarriorLevel2 : BaseEnemy
{
    #region Animations
    
    private const string ANIM__WOUND_COMONWARRIOR = "_Wound_ComonWarrior";
    private const string ANIM_ABILITY_COMONWARRIOR = "Ability_ComonWarrior";
    private const string ANIM_ATTACK_COMONWARRIOR = "Attack_ComonWarrior";
    private const string ANIM_DEAD_COMONWARRIOR = "Dead_ComonWarrior";
    private const string ANIM_IDLE_COMONWARRIOR = "idle_ComonWarrior";
    private const string ANIM_WOUND_BLOCK_COMONWARRIOR = "Wound_Block_ComonWarrior";
    private const string ANIM_WOUND_COMONWARRIOR = "Wound_ComonWarrior";
    private const string ANIM_WOUND_IDDLE_COMONWARRIOR = "Wound_Iddle_ComonWarrior";
    
    #endregion

    public Action OnPhase1;
    
    [SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] private WarriorLevel2MovesData m_data;
    
    protected override void Awake()
    {
        base.Awake();

        ConfigFighterHP();
        
SetMoves(m_movesDatas);
    }

    private void OnDestroy()
    {
        Taunt(false);
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
        Taunt(false);
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
            case "Taunt":
                CallOnIntentionDetermined(Intention.BUFF, m_nextMove.description);
                break;
            case "Hit":
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
        Taunt(false);
        switch (m_nextMove.clientID)
        {
            case "Taunt":
                m_animation.Play(ANIM_ATTACK_COMONWARRIOR, finishCallback);
                Taunt(true);
				break;
            case "Hit":
                m_animation.Play(ANIM_ATTACK_COMONWARRIOR, finishCallback);
				yield return new WaitForSeconds(1f);
                int damage = m_data.Move2Damage;
                List<BaseEnemy> enemies = GetAllEnemiesExcludingSelf();
                
                // remove archer wave
                enemies.RemoveAll(e => e is ArcherWave);

                int numOfAlliesAlive = enemies.Count;
                numOfAlliesAlive = Mathf.Min(numOfAlliesAlive, m_data.Move2MaxNumOfAllies);
                damage -= numOfAlliesAlive * m_data.Move2AllyDamageMultiplier;
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, damage);
				break;
        }
        yield return null;
    }

    private void Taunt(bool activate)
    {
        List<BaseEnemy> otherEnemies = GetAllEnemiesExcludingSelf();
        if (otherEnemies == null || otherEnemies.Count == 0)
        {
            return;
        }
        
        foreach (BaseEnemy enemy in otherEnemies)
        {
            enemy.SetCanBeTarget(!activate);
        }
    }
    private List<BaseEnemy> GetAllEnemiesExcludingSelf()
    {
        List<Fighter> allEnemies = GameInfoHelper.GetAllEnemies();
        if (allEnemies == null || allEnemies.Count == 0)
        {
            return null;
        }
        List<BaseEnemy> enemies = allEnemies.Cast<BaseEnemy>().ToList();
        enemies.Remove(this);

        return enemies;
    }
    
    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }
}