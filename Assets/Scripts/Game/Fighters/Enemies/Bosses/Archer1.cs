using System;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer1 : BaseEnemy
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
    [SerializeField] private Archer1MovesData m_data;
    
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
            case "Str":
                CallOnIntentionDetermined(Intention.BLOCK, m_nextMove.description);
                break;
        }
    }

    public override void ExecuteAction(Action finishCallback)
    {
        base.ExecuteAction(finishCallback);

        // Debug.Log("this action is played: " + m_nextMove.clientID);
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
            case "Hit":
				yield return WaitForAnimation(ANIM_05_SHOOT, finishCallback);
				Fighter player = GameInfoHelper.GetPlayer();
				GameActionHelper.DamageFighter(player, this, m_data.Move1Damage);
				break;
            case "Str":
                yield return WaitForAnimation(ANIM_05_SHOOT);
                List<Fighter> enemies = GameInfoHelper.GetAllEnemies();
                foreach (Fighter enemy in enemies)
                {
                    GameActionHelper.AddMechanicToFighter(enemy, m_data.Move2Str, MechanicType.STRENGTH);
                }
                finishCallback?.Invoke();
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
