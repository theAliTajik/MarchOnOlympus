using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior2 : BaseEnemy
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
    [SerializeField] private Warrior2MovesData m_data;
    
    protected override void Awake()
    {
        base.Awake();

        ConfigFighterHP();
        
SetMoves(m_movesDatas);
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
        RandomIntentionPicker();
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();
        switch (m_nextMove.clientID)
        {
            case "Fortify":
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
        switch (m_nextMove.clientID)
        {
            case "Fortify":
                m_animation.Play(ANIM_ATTACK_COMONWARRIOR, finishCallback);
                yield return new WaitForSeconds(1f);
				List<Fighter> enemies = GameInfoHelper.GetAllEnemies();
				foreach (Fighter i in enemies)
					GameActionHelper.AddMechanicToFighter(i, m_data.Move1Fortify, MechanicType.FORTIFIED);
				break;
            case "Hit":
                m_animation.Play(ANIM_ATTACK_COMONWARRIOR, finishCallback);
				yield return new WaitForSeconds(1f);
				Fighter player = GameInfoHelper.GetPlayer();
				GameActionHelper.DamageFighter(player, this, m_data.Move2Damage);
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