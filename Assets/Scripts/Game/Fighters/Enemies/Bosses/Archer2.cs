using System;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer2 : BaseEnemy
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
    [SerializeField] private Archer2MovesData m_data;
    
    
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
        RandomIntentionPicker(m_moves);
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();
        switch (m_nextMove.clientID)
        {
            case "Thorns":
                CallOnIntentionDetermined(Intention.BLOCK, m_nextMove.description);
                break;
            case "Hit":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
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
            case "Thorns":
                yield return WaitForAnimation(ANIM_05_SHOOT, finishCallback);
                Fighter warlvl2 = FindWarriorLevel2();
                if (warlvl2) GameActionHelper.AddMechanicToFighter(warlvl2, m_data.Move1Thorns, MechanicType.THORNS);
                else Debug.Log(gameObject.name + ": WarriorLevel2 Not Exist", gameObject);
                break;
            case "Hit":
                yield return WaitForAnimation(ANIM_05_SHOOT, finishCallback);
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
    
	private Fighter FindWarriorLevel2()
	{
		List<Fighter> allEnemies = GameInfoHelper.GetAllEnemies();
		return allEnemies.Find(enemy => enemy.GetType().Name == "WarriorLevel2");
	}
}
