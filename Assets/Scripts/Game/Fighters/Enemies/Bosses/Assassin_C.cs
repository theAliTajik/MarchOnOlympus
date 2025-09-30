using System;
using Game;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Assassin_C : BaseEnemy
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
    [SerializeField] private Assassin_C_MovesData m_data;

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
        RandomIntentionPicker();
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();
        switch (m_nextMove.clientID)
        {
            case "Thorns":
                CallOnIntentionDetermined(Intention.BUFF, m_nextMove.description); //No Vulnerable
				break;
            case "HitBlock":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description); //About Restore?
				break;
        }
    }

    public override void ExecuteAction(Action finishCallback)
    {
        // play intention
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
            case "Thorns":
                m_animation.Play(ANIM_05_ATTACK, finishCallback);
				List<Fighter> enemies = GameInfoHelper.GetAllEnemies();
                foreach (Fighter i in enemies)
					GameActionHelper.AddMechanicToFighter(i, m_data.Move1Thorns, MechanicType.THORNS);
                
				break;
            case "HitBlock":
                m_animation.Play(ANIM_05_ATTACK, finishCallback);
				Fighter player = GameInfoHelper.GetPlayer();
                GameActionHelper.DamageFighter(player, this, m_data.Move2Damage);
                GameActionHelper.AddMechanicToFighter(this, m_data.Move2Block, MechanicType.BLOCK);
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