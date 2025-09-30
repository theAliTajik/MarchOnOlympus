using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Harpy : BaseEnemy
{
    #region Animations

    private const string ANIM_001_FLAY = "001_Flay";
    private const string ANIM_002_DAMAGE = "002_Damage";
    private const string ANIM_003_FLYUP = "003_FlyUP";
    private const string ANIM_004_ATTACK = "004_Attack";
    private const string ANIM_005_ATTACK_CRIT = "005_Attack_Crit";
    private const string ANIM_006_SCREEM = "006_Screem";
    private const string ANIM_DEATH = "Death";
    private const string ANIM_FLY2 = "Fly2";
    private const string ANIM_FLY_SETUP = "Fly[setup]";
    
    #endregion

    [SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] protected MoveData[] m_movesDatasAirbone;
    [SerializeField] private HarpyMovesData m_data;

    [SerializeField] private Vector3 m_airborneOffset;
    private Vector3 m_spawnPos;

    private List<HarpyMinion> m_minions = new List<HarpyMinion>();
    private int m_turnCounter = 0, m_inAirboneTurnCounter = 0;
    private bool m_isOnAirbone = false;

    protected override void Awake()
    {
        base.Awake();

        ConfigFighterHP();

        SetMoves(m_movesDatas);
	}

    private void Start()
    {
        HP.SetTrigger(m_data.Phase1HPPercentageTrigger);
        HP.SetTrigger(m_data.Phase2HPPercentageTrigger);
        HP.OnPercentageTrigger += OnHPPercentageTriggred;

        GameplayEvents.GamePhaseChanged += OnPhaseChange;
        m_spawnPos = transform.position;
	}

	private void OnDestroy()
	{
		GameplayEvents.GamePhaseChanged -= OnPhaseChange;
		HP.OnPercentageTrigger -= OnHPPercentageTriggred;
	}

	private void OnPhaseChange(EGamePhase phase)
	{
        switch (phase)
        {
            case EGamePhase.PLAYER_TURN_END:
                GameActionHelper.ReduceMechanicStack(this, 1, MechanicType.DOUBLEDAMAGE);
                break;
        }
	}

	private void OnHPPercentageTriggred(FighterHP.TriggerPercentage percentage)
    {
        if (percentage == m_data.Phase1HPPercentageTrigger)
        {
            Debug.Log("Airbone at 66");
            SetAirbone(true);
        }

        if (percentage == m_data.Phase2HPPercentageTrigger)
        {
            Debug.Log("Screech at 33");
            Screech();
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
        m_animation.Play(ANIM_002_DAMAGE);
    }

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_DEATH);
    }

    public override void DetermineIntention()
    {
		if (m_isOnAirbone && m_inAirboneTurnCounter < 3)
		{
			m_nextMove = m_moves[0];
			ShowIntention();
            return;
		}

		RandomIntentionPicker();
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();

        switch (m_nextMove.clientID)
        {
            case "HitBleed":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "ScreechDaze":
                CallOnIntentionDetermined(Intention.CAST_DEBUFF, m_nextMove.description);
                break;

            case "Airbone":
                CallOnIntentionDetermined(Intention.BLOCK, m_nextMove.description);
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

        bool finishedAnim = false;
        switch (m_nextMove.clientID)
        {
            case "HitBleed":
                m_animation.Play(ANIM_004_ATTACK, () => finishedAnim = true);
                Fighter player = GameInfoHelper.GetPlayer();
                GameActionHelper.DamageFighter(player, this, m_data.Move1Damage);
                GameActionHelper.AddMechanicToFighter(player, m_data.Move1Bleed, MechanicType.BLEED);
				break;
            case "ScreechDaze":
                Screech();
				m_animation.Play(ANIM_006_SCREEM, () => finishedAnim = true);
                break;

            case "Airbone":
				m_animation.Play(ANIM_003_FLYUP, () => finishedAnim = true);
				break;
        }

        yield return new WaitUntil(() => finishedAnim);
        finishCallback?.Invoke();

		if (m_isOnAirbone)
		{
			m_inAirboneTurnCounter++;
			Debug.Log("--- Airbone-Turn : " + m_inAirboneTurnCounter);

			if (m_inAirboneTurnCounter > 2)
				SetAirbone(false);
		}
		else
		{
			m_turnCounter++;
			Debug.Log("--- Turn : " + m_turnCounter);

			if (m_turnCounter > 2)
			{
				SetAirbone(true);
				m_turnCounter = 0;
			}
		}
	}

    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

	private void Screech()
	{
		Fighter player = GameInfoHelper.GetPlayer();
		GameActionHelper.AddMechanicToFighter(player, m_data.Move2Daze, MechanicType.DAZE);
	}

	public void ReleaseAnimal()
    {
       if (m_minions.Count > 0)
            return;

        Debug.Log("--- ReleaseAnimals");

        for (int i = 0; i < 2; i++)
        {
            HarpyMinion minion = EnemiesManager.Instance.SpawnBoss("HarpyMinion")[0] as HarpyMinion;
            minion.OnDead += RemoveFromList;

            if (minion != null)
            {
                m_minions.Add(minion);
                minion.DetermineIntention();
            }
        }
    }

    public void RemoveFromList(HarpyMinion hm)
    {
        if (m_minions.Contains(hm))
        {
            m_minions.Remove(hm);
			StartCoroutine(DestroyDeadAfterDelay(hm, 2));
		}

        if (m_minions.Count == 0)
            SetAirbone(false);
	}

	private IEnumerator DestroyDeadAfterDelay(Fighter fighter, float seconds)
	{
		yield return new WaitForSeconds(seconds);
		EnemiesManager.Instance.RemoveDeadEnemyBody(fighter);
	}

	private void SetAirbone(bool value)
    {
        Debug.Log("----------- Set Aitbone: " + value);
		m_inAirboneTurnCounter = 0;

		m_isOnAirbone = value;
        SetCanBeTarget(!value);
        SetMoves(value ? m_movesDatasAirbone : m_movesDatas);

        DetermineIntention();
		ShowIntention();

		if (value)
		{
			ReleaseAnimal();
			MoveUp();
		}
		else
		{
			AirbonDone();
			MoveDown();
		}
	}

	private void MoveUp()
	{
		transform.DOMove(m_spawnPos + m_airborneOffset, 0.5f);
	}

	private void MoveDown()
	{
		transform.DOMove(m_spawnPos, 0.5f);
	}
    
    private void AirbonDone()
    {
	    
        if (IsMinionsDead()) //100% Damage
        {
	        Debug.Log("minions are dead added Double damage");
            GameActionHelper.AddMechanicToFighter(this, 1, MechanicType.DOUBLEDAMAGE);
            return;
        }

        //40 Damage to player
        Fighter player = GameInfoHelper.GetPlayer();
        GameActionHelper.DamageFighter(player, this, 40);
    }

	private bool IsMinionsDead()
	{
        /*bool IsMinionsDead = true;

		foreach (HarpyMinion m in m_minions)
		{
			if (!m.isDead)
				IsMinionsDead = false;
		}

		return IsMinionsDead;*/

        return m_minions.Count == 0;
	}
}