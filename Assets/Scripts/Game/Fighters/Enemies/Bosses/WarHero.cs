using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WarHero : BaseEnemy
{
    #region Animations
    
    private const string ANIM_01_IDLE = "01_Idle";
    private const string ANIM_02_WOUND = "02_Wound";
    private const string ANIM_03_DEATH = "03_Death";
    private const string ANIM_ATTACK = "Attack";
    private const string ANIM_ATTACK2 = "Attack2";
    private const string ANIM_ATTACK3_CRIT = "Attack3_Crit";
    private const string ANIM_ATTACK3_CRIT2 = "Attack3_Crit2";
    private const string ANIM_SHOUT2 = "Shout2";
    
    #endregion

    public Action OnPhase1;

    [SerializeField] protected MoveData[] m_movesData;
    
    [SerializeField] private MoveData m_hitHauntMoveData;
    [SerializeField] private MoveData m_panicOneMoveData;
    [SerializeField] private MoveData m_panicTwoMoveData;
    
	[SerializeField] private WarHeroMovesData m_data;
	private List<FearMinion> m_minions = new List<FearMinion>();
    
    protected override void Awake()
    {
        base.Awake();
        ConfigFighterHP();
        
		m_hitHauntMoveData.Condition = () => true;
		m_panicOneMoveData.Condition = IsOnePanic;
		
		List<MoveData> moves = new List<MoveData>();
		moves.Add(m_hitHauntMoveData);
		moves.Add(m_panicOneMoveData);
		moves.AddRange(m_movesData);

		m_panicTwoMoveData.Condition = IsAboveTwoPanic;
		m_intentionPicker = new WarHeroIntentionDeterminer(moves, m_panicTwoMoveData);
    }

    private bool IsAboveTwoPanic()
    {
	    int panic = GetPanic();
	    bool result = panic >= 2;

	    Debug.Log($"---> [War Hero] Panic >= 2 is :{result}");
	    return result;
    }

    private bool IsOnePanic()
    {
	    int panic = GetPanic();
	    bool result = panic == 1;

	    Debug.Log($"---> [War Hero] Panic == 1 is: {result}");
	    return result;
    }

    private int GetPanic()
    {
	    int panics = GameInfoHelper.GetMechanicStack(GameInfoHelper.GetPlayer(), MechanicType.PANIC);
	    return panics;
    }

    private bool DoesPlayerHaveBlock()
    {
		return GameInfoHelper.CheckIfFighterHasMechanic(GameInfoHelper.GetPlayer(), MechanicType.BLOCK);
    }

    private void Start()
	{
		HP.SetTrigger(m_data.Phase1HPPercentageTrigger);
		HP.SetTrigger(m_data.Phase2HPPercentageTrigger);
		HP.OnPercentageTrigger += OnHPPercentageTriggred;
	}

	private void OnHPPercentageTriggred(FighterHP.TriggerPercentage percentage)
	{
		if (percentage == m_data.Phase1HPPercentageTrigger)
		{
			Debug.Log("--> [War Hero] 66% Summon 2 Fear, if there are fear minions already, Apply Panic on Player");

            if (IsMinionsDead())
                ReleaseMinions();
            else
            {
				Debug.Log("--> [War Hero] Apply Panic on Player");
				GameActionHelper.AddMechanicToFighter(GameInfoHelper.GetPlayer(), 1, MechanicType.PANIC);
			}
		}

		if (percentage == m_data.Phase2HPPercentageTrigger)
		{
			Debug.Log("--> [War Hero] 33% Permanent Fortify");
            GameActionHelper.AddMechanicToFighter(this, 1, MechanicType.FORTIFIED, 1);
		}

		Debug.Log("percentage triggered: :" + percentage.Percentage);
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
            return;

        base.OnDeath();
        m_animation.Play(ANIM_03_DEATH);
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
            case "HitHunt":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "Panic1":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
			case "Panic2":
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

		Fighter player = GameInfoHelper.GetPlayer();

        switch (m_nextMove.clientID)
        {
            case "HitHunt":
                for (int i = 1; i <= m_data.Move1NumOfAttacks; i++)
                {
	                yield return WaitForAnimation(ANIM_ATTACK);
                    GameActionHelper.DamageFighter(player, this, m_data.Move1Damage);
                    
	                if (DoesPlayerHaveBlock())
	                {
		                Debug.Log("---> [War Hero] Player did have block");
						continue;
	                }

	                Debug.Log("---> [War Hero] Player did no have block");
                    GameActionHelper.AddMechanicToFighter(GameInfoHelper.GetPlayer(), m_data.Move1Haunt,
			                MechanicType.HAUNT);
                }
                finishCallback?.Invoke();
                break;

            case "Panic1":
				yield return WaitForAnimation(ANIM_ATTACK2, finishCallback);
				GameActionHelper.DamageFighter(player, this, m_data.Move2Damage_PanicGreater1); // 1 * m_data.Move2Damage_PanicGreater1
				Heal(m_data.Move2Restore);
				Debug.Log("---> [War Hero] Panic == 1");
                break;

			case "Panic2":
				yield return WaitForAnimation(ANIM_ATTACK3_CRIT2, finishCallback);
				GameActionHelper.DamageFighter(player, this, m_data.Move2Damage_PanicGreater2);
				Debug.Log("---> [War Hero] Panic >= 2");
				break;
		}

		//---> Always If no "Fear" then Summon 2x "Fear"
		if (IsMinionsDead())
			ReleaseMinions();

		yield return null;
    }
    
    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

	public void ReleaseMinions()
	{
		if (m_minions.Count > 0)
			return;

		Debug.Log("---> [War Hero] Release Fear Minions");

		for (int i = 0; i < 2; i++)
		{
			FearMinion minion = EnemiesManager.Instance.SpawnBoss("FearMinion")[0] as FearMinion;
			minion.OnDead += RemoveFromList;

			if (minion != null)
			{
				m_minions.Add(minion);
				minion.DetermineIntention();
			}
		}
	}

	public void RemoveFromList(FearMinion fm)
	{
		if (m_minions.Contains(fm))
		{
			m_minions.Remove(fm);
			StartCoroutine(DestroyDeadAfterDelay(fm, 2));
		}
	}

	private IEnumerator DestroyDeadAfterDelay(Fighter fighter, float seconds)
	{
		yield return new WaitForSeconds(seconds);
		EnemiesManager.Instance.RemoveDeadEnemyBody(fighter);
	}

	private bool IsMinionsDead()
	{
		return m_minions.Count == 0;
	}
}