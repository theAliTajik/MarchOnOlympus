using System;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Lamia : BaseEnemy
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
    [SerializeField] private LamiaMovesData m_data;

    
    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < m_movesDatas.Length; i++)
        {
            MoveData md = m_movesDatas[i];
            m_moves.Add(md, md.chance);
        }
        
        ConfigFighterHP();
    }

	private void Start()
	{
		HP.SetTrigger(m_data.Phase1HPPercentageTrigger);
		HP.SetTrigger(m_data.Phase2HPPercentageTrigger);
		HP.OnPercentageTrigger += OnHPPercentageTriggred;

		GameplayEvents.GamePhaseChanged += OnPhaseChange;
	}

	private void OnDestroy()
	{
		GameplayEvents.GamePhaseChanged -= OnPhaseChange;
		HP.OnPercentageTrigger -= OnHPPercentageTriggred;
	}

	private void OnHPPercentageTriggred(FighterHP.TriggerPercentage percentage)
	{
        if (percentage == m_data.Phase1HPPercentageTrigger)
        {
            Debug.Log("at 66");
            // 66% Removes all player block, Apply vulnerable 3 to playey
            Fighter player = GameInfoHelper.GetPlayer();
            GameActionHelper.ReduceMechanicStack(player, 100, MechanicType.BLOCK);
            GameActionHelper.AddMechanicToFighter(player, 3, MechanicType.VULNERABLE);
        }

		if (percentage == m_data.Phase2HPPercentageTrigger)
		{
			Debug.Log("at 33");
			// 33% Auto Petrify 10
			Fighter player = GameInfoHelper.GetPlayer();
            GameActionHelper.AddMechanicToFighter(player, 10, MechanicType.PETRIFY);

		}

		Debug.Log("percentage triggered: :" + percentage.Percentage);
	}

	private void OnPhaseChange(EGamePhase phase)
	{
		switch (phase)
		{
			case EGamePhase.PLAYER_TURN_START:
				Fighter player = GameInfoHelper.GetPlayer();
                var mList = player.GetMechanicsList();
                BaseMechanic bM = mList.GetMechanic(MechanicType.PETRIFY);
                if (bM.Stack == 10)
                {
                    // ---> END PLAYER TURN
                }
				break;
		}
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
        RandomIntentionPicker(m_moves);
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();
        switch (m_nextMove.clientID)
        {
            case "HitPetrify":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "BlockThorns":
                CallOnIntentionDetermined(Intention.BLOCK, m_nextMove.description);
                break;
			case "HitBlockPetrify":
				CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
				break;

			case "IfPetrifyHit":
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
			case "HitPetrify":
				for (int i = 1; i <= m_data.Move1NumOfAttacks; i++)
				{
					yield return WaitForAnimation(ANIM_05_ATTACK);
					GameActionHelper.DamageFighter(player, this, m_data.Move1Damage);
					GameActionHelper.AddMechanicToFighter(player, 1, MechanicType.PETRIFY);
				}
				finishCallback?.Invoke();
				break;
			case "BlockThorns":
                m_animation.Play(ANIM_CAST_CLON, finishCallback);
				GameActionHelper.AddMechanicToFighter(this, m_data.Move2Block, MechanicType.BLOCK);
				GameActionHelper.AddMechanicToFighter(this, m_data.Move2Thorns, MechanicType.THORNS);
				break;
			case "HitBlockPetrify":
				yield return WaitForAnimation(ANIM_05_ATTACK);
                GameActionHelper.DamageFighter(player, this, m_data.Move3Damage, isArmorPiercing: true);
				GameActionHelper.AddMechanicToFighter(player, m_data.Move3PetrifyMultiply, MechanicType.PETRIFY);
				break;
			case "IfPetrifyHit":
				// Only if petrify > 4, Hit 5xPetrify Stack
				var mList = player.GetMechanicsList();
				BaseMechanic bM = mList.GetMechanic(MechanicType.PETRIFY);
				if (bM.Stack > 4)
				{
                    GameActionHelper.DamageFighter(player, this, m_data.Move4Damage * bM.Stack);
				}
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