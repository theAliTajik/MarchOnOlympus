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
    [SerializeField] private ScyllaMovesData m_data;

    
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
	}

	private void OnHPPercentageTriggred(FighterHP.TriggerPercentage percentage)
	{
		if (percentage == m_data.Phase1HPPercentageTrigger)
		{
			Debug.Log("at 66");
			// 66% Removes all player block, Apply vulnerable 3 to player	
		}

		if (percentage == m_data.Phase2HPPercentageTrigger)
		{
			Debug.Log("at 33");
			// 33% Auto Petrify 10
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
				finishCallback?.Invoke();
				break;
			case "BlockThorns":
				for (int i = 1; i <= m_data.Move2NumOfBlocks; i++)
				{
					yield return WaitForAnimation(ANIM_05_ATTACK);
					GameActionHelper.AddMechanicToFighter(this, m_data.Move2Block, MechanicType.BLOCK);
				}
				finishCallback?.Invoke();
				break;
			case "RestoreHitPhoenix":
                //Restore
				yield return WaitForAnimation(ANIM_05_ATTACK);

                //Hit 15 X Dead Tentacle Count
                int deadTentacleCount = 2; //Ex = 2
				for (int i = 1; i <= deadTentacleCount; i++)
				{
					yield return WaitForAnimation(ANIM_05_ATTACK);
					GameActionHelper.DamageFighter(player, this, m_data.Move3Damage);
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