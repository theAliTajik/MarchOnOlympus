using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarHero : BaseEnemy
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
    
    [SerializeField] protected MoveData[] m_movesDatasSimple;
    [SerializeField] protected MoveData[] m_movesDatasWithPanic;
	[SerializeField] private WarHeroMovesData m_data;
	private List<FearMinion> m_minions = new List<FearMinion>();
    
    protected override void Awake()
    {
        base.Awake();
        ConfigFighterHP();
        SetMoves(m_movesDatasSimple);
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
			Debug.Log("--> [War Hero] 33% Permanent Forfity on Player");
            GameActionHelper.AddMechanicToFighter(GameInfoHelper.GetPlayer(), 10, MechanicType.FORTIFIED);
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
        m_animation.Play(ANIM__WOUND_COMONWARRIOR);
    }

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
            return;

        base.OnDeath();
        m_animation.Play(ANIM_DEAD_COMONWARRIOR);
    }
    
    public override void DetermineIntention()
    {
		int panics = GameInfoHelper.GetMechanicStack(GameInfoHelper.GetPlayer(), MechanicType.PANIC);
		SetMoves(panics > 0 ? m_movesDatasWithPanic : m_movesDatasSimple);
        Debug.Log($"---> [War Hero] Player has panic: {panics > 0}");

		RandomIntentionPicker(m_moves);
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
            case "Panic":
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
					yield return WaitForAnimation(ANIM_ATTACK_COMONWARRIOR);
					GameActionHelper.DamageFighter(player, this, m_data.Move1Damage);

                    if (!GameInfoHelper.CheckIfFighterHasMechanic(player, MechanicType.BLOCK))
                    {
						Debug.Log("---> [War Hero] + Haunt 2 per Attack that hits Player");
						GameActionHelper.AddMechanicToFighter(GameInfoHelper.GetPlayer(), m_data.Move1Haunt, MechanicType.HAUNT);
					}
				}
				finishCallback?.Invoke();
				break;

            case "Panic":
				int panics = GameInfoHelper.GetMechanicStack(player, MechanicType.PANIC);

				if (panics >= 2)
                {
                    yield return WaitForAnimation(ANIM_ATTACK_COMONWARRIOR);
                    GameActionHelper.DamageFighter(player, this, m_data.Move2Damage_PanicGreater2);
                    Debug.Log("---> [War Hero] Panic >= 2");
                }
                else if (panics >= 1)
                {
                    for (int i = 1; i <= panics; i++)
                    {
						yield return WaitForAnimation(ANIM_ATTACK_COMONWARRIOR);
						GameActionHelper.DamageFighter(player, this, panics * m_data.Move2Damage_PanicGreater1);
                        Heal(m_data.Move2Restore);
                        Debug.Log("---> [War Hero] Panic >= 1");
                    }
                }
                else
                {
					Debug.Log($"--> [War Hero] | No Panic stacks found, no damage dealt.");
                }
				finishCallback?.Invoke();
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
		EnemiesManager.Instance.RemoveDeadEnemy(fighter);
	}

	private bool IsMinionsDead()
	{
		return m_minions.Count == 0;
	}
}