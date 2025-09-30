using System;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scylla : BaseEnemy
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
    [SerializeField] private MoveData m_restoreMove;
    [SerializeField] private ScyllaMovesData m_data;
    [SerializeField] private List<ScyllaTentacle> m_tentacles;
    
    private ITurnCounter m_turnCounter;

    private List<ScyllaTentacle> m_deadTentacles = new List<ScyllaTentacle>();
    private int m_bossPhase = 1;
    private int m_cycleMaxTurn = 3;
    private ScyllaTentacle m_targetTentacle;

    public IEnumerator<ScyllaTentacle> GetTentacles()
    {
	    return m_tentacles.GetEnumerator();
    }
    
    protected override void Awake()
    {
        base.Awake();

        m_restoreMove.Condition = IsTurnPassedThreshold;
        
        SetMoves(m_movesDatas);

        m_turnCounter = new CyclicalEnemyTurnCounter(m_cycleMaxTurn);
        ConfigFighterHP();
       
        GameplayEvents.ColliderSelected += OnColliderSelected;
        m_damageable = new ScyllaDamageBehaviour(this);
        m_damageable.OnDamage += m_fighterHP.TakeDamage;
        
        foreach (var tentacle in m_tentacles)
        {
	        tentacle.OnDeath += OnTentacleDeath;
        }
    }

    private void OnDestroy()
    {
	    GameplayEvents.ColliderSelected -= OnColliderSelected;
    }

    private void OnTentacleDeath(ScyllaTentacle tentacle)
    {
		m_tentacles.Remove(tentacle);
		m_deadTentacles.Add(tentacle);

		if (m_targetTentacle == tentacle)
		{
			SetTargetedTentacle(null);
		}
    }

    private void OnColliderSelected(Collider2D targetCollider)
    {
        ScyllaTentacle tentacleHit = MatchColliderToHead(targetCollider);
        SetTargetedTentacle(tentacleHit);
        // Debug.Log($"set head to: {headHit.GetType()}");
    }

    private ScyllaTentacle MatchColliderToHead(Collider2D targetCollider)
    {
        foreach (var tentacle in m_tentacles)
        {
            if (tentacle.IsMyCollider(targetCollider))
                return tentacle;
        }
        
        return null;
    }
    
    private void SetTargetedTentacle(ScyllaTentacle tentacle)
    {
        ScyllaDamageBehaviour damageable = m_damageable as ScyllaDamageBehaviour;
        damageable.SetTargetedTentacle(tentacle);
        m_targetTentacle = tentacle;
    }
    
    private bool IsTurnPassedThreshold()
    {
	    int threshold = 3;
		bool result = m_turnCounter.GetRelativeTurn() > threshold;

		Debug.Log($"passed threshold resutl: {result}, turn{m_turnCounter.GetRelativeTurn()}");
	    return result;
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
			Debug.Log("at 66");
			if (m_tentacles.Count > 1)
			{
				GameActionHelper.AddMechanicToOwner(this, m_data.Phase1Fortify, MechanicType.FORTIFIED);
			}
			else
			{
				GameActionHelper.DamagePlayer(this, m_data.Phase1Damage);
			}
			m_bossPhase = 2;
		}

		if (percentage == m_data.Phase2HPPercentageTrigger)
		{
			Debug.Log("at 33");
			if (m_tentacles.Count > 1)
			{
				ResotreAllTentacles(m_data.Phase2Restore);
				Heal(m_data.Phase2Restore);
			}
			else
			{
				GameActionHelper.AddMechanicToOwner(this, m_data.Phase2Fortify, MechanicType.FORTIFIED);
				ReviveAllTentacles();
			}
			m_bossPhase = 3;
		}

		Debug.Log("percentage triggered: :" + percentage.Percentage);
	}

	private void ReviveAllTentacles()
	{
		foreach (var tentacle in m_deadTentacles)
		{
			tentacle.Revive();
		}
	}

	public int GetBossPhase()
	{
		return m_bossPhase;
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

    private bool firstTimeRestore = true;
    public override void DetermineIntention()
    {
        RandomIntentionPicker();
        foreach (var tentacle in m_tentacles)
        {
	        tentacle.DetermineIntention();
        }
        ShowIntention();
        
	    m_turnCounter.NextTurn();
	    bool isActionTurn = m_turnCounter.GetRelativeTurn() >= m_cycleMaxTurn;
	    if (isActionTurn && firstTimeRestore)
	    {
		    firstTimeRestore = false;
		    return;
	    }

	    if (isActionTurn)
	    {
		    m_nextMove = m_restoreMove;
	    }
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
            case "Block":
                CallOnIntentionDetermined(Intention.BLOCK, m_nextMove.description);
                break;
			case "RestoreHit":
				CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
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

		Fighter player = GameInfoHelper.GetPlayer();

		switch (m_nextMove.clientID)
        {
			case "Hit":
				for (int i = 1; i <= m_data.Move1NumOfAttacks; i++)
				{
					yield return WaitForAnimation(ANIM_05_ATTACK);
					GameActionHelper.DamageFighter(player, this, m_data.Move1Damage);
				}
				break;
			case "Block":
				for (int i = 1; i <= m_data.Move2NumOfBlocks; i++)
				{
					yield return WaitForAnimation(ANIM_05_ATTACK);
					GameActionHelper.AddMechanicToFighter(this, m_data.Move2Block, MechanicType.BLOCK);
				}
				break;
			case "RestoreHit":
				if (m_tentacles.Count > 0)
				{
					yield return WaitForAnimation(ANIM_CAST_CLON);
					ResotreAllTentacles(m_data.Move3Restore);
				}

				yield return WaitForAnimation(ANIM_05_ATTACK);
				int d = m_data.Move3Damage * m_deadTentacles.Count;
				GameActionHelper.DamageFighter(player, this, d);
				break;
		}

		foreach (var tentacle in m_tentacles)
		{
			bool finished = false;
			StartCoroutine(tentacle.ExecuteIntention(() => finished = true));
			yield return new WaitUntil(() => finished);
		}
		
		finishCallback?.Invoke();

        yield return null;
    }

    private void ResotreAllTentacles(int amount)
    {
	    foreach (var tentacle in m_tentacles)
	    {
		    tentacle.GetHP().Heal(amount);
	    }
    }

    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }
}