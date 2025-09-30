using System;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Minotaur : BaseEnemy
{
    #region Animations
    
    private const string ANIM_ATTACK = "Attack";
    private const string ANIM_ATTACK2 = "Attack2";
    private const string ANIM_DEAD = "Dead";
    private const string ANIM_GROW = "Grow";
    private const string ANIM_IDDLE = "Iddle";
    private const string ANIM_WOUND = "Wound";    
    
    #endregion
    
    [SerializeField] protected MoveData[] m_movesDatas;
    
    [SerializeField] protected MoveData m_hitTwentyMoveData;
    [SerializeField] protected MoveData m_shoutOneMoveData;
    [SerializeField] protected MoveData m_shoutTwoMoveData;
    [SerializeField] protected MoveData m_hitOneHundredMoveData;
    
    [SerializeField] private MinotaurMovesData m_data;

  
    protected override void Awake()
    {
        base.Awake();

        // Make conditional moves
        m_hitTwentyMoveData.Condition = IsSTRBiggerThanOne;
        m_shoutOneMoveData.Condition = IsPlayerHPMoreThan51Precent;
        m_shoutTwoMoveData.Condition = IsPlayerHPLessThan50Precent;
        
        List<MoveData> moves = new List<MoveData>();
        moves.Add(m_hitTwentyMoveData);
        moves.Add(m_shoutOneMoveData);
        moves.Add(m_shoutTwoMoveData);
        
        moves.AddRange(m_movesDatas);
        
        m_intentionPicker = new ConditionalRandomIntentionDeterminer(moves);
        
        
        ConfigFighterHP();
    }

    private bool IsPlayerHPLessThan50Precent()
    {
        int playerHpPrecentage = GameInfoHelper.GetPlayerHPPrecentage();
        bool isLess = playerHpPrecentage <= m_data.Move4PlayerHPPercentage;
        Debug.Log($"checked is player hp < than 50 returned {isLess}");
        return isLess;
    }

    private bool IsPlayerHPMoreThan51Precent()
    {
        int playerHpPrecentage = GameInfoHelper.GetPlayerHPPrecentage();
        bool isMore = playerHpPrecentage >= m_data.Move3PlayerHPPercentage;
        Debug.Log($"checked is player hp > 51 returned {isMore}");
        return isMore;
    }

    private bool IsSTRBiggerThanOne()
    {
        int str = GameInfoHelper.GetMechanicStack(this, MechanicType.STRENGTH);
        bool isBigger = str > 1;
        
        Debug.Log($"checked if str > 1 returned {isBigger}");
        return isBigger;
    }

    private bool IsSTRBiggerThanTwenty()
    {
        int strStack = GameInfoHelper.GetMechanicStack(this, MechanicType.STRENGTH);
        
        bool hasPassedThreshold = strStack >= m_data.Move5StrThreshold;

        return hasPassedThreshold;
    }

    private void Start()
	{
		HP.SetTrigger(m_data.GainStrHPPercentageTrigger);
		HP.SetTrigger(m_data.UseShoutsHPPercentageTrigger);
		HP.OnPercentageTrigger += OnHPPercentageTriggred;

        GameplayEvents.MechanicAddedToFighter += OnMechanicAdded;
    }

    private void OnMechanicAdded(Fighter fighter, BaseMechanic baseMechanic)
    {
        if (fighter != this)
        {
            return;
        }

        if (baseMechanic.GetMechanicType() != MechanicType.STRENGTH)
        {
            return;
        }

    }

	private void OnHPPercentageTriggred(FighterHP.TriggerPercentage percentage)
	{
		if (percentage == m_data.GainStrHPPercentageTrigger)
		{
			Debug.Log("at 66");
            GameActionHelper.AddMechanicToFighter(this, m_data.PercentageTriggerStrGain, MechanicType.STRENGTH);
		}

		if (percentage == m_data.UseShoutsHPPercentageTrigger)
		{
			Debug.Log("at 33");
            PerformShoutOne();
            PerformShoutTwo();
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
        m_animation.Play(ANIM_WOUND);
    }

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_DEAD);
        if (IsSTRBiggerThanTwenty())
        {
            GameActionHelper.AddPerk(m_data.OnDeathPerkName);
        }
    }

    public override void DetermineIntention()
    {
        if (IsSTRBiggerThanTwenty())
        {
            m_nextMove = m_hitOneHundredMoveData;
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
            case "hitFive":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "hitTwenty":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "shoutOne":
                CallOnIntentionDetermined(Intention.BUFF, m_nextMove.description);
                break;
            case "shoutTwo":
                CallOnIntentionDetermined(Intention.BLOCK, m_nextMove.description);
                break;
            case "strBigger":
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
        switch (m_nextMove.clientID)
        {
            case "hitFive":
                GameActionHelper.DamagePlayer(this, m_data.Move1Damage);
                yield return WaitForAnimation(ANIM_ATTACK, finishCallback);
                GameActionHelper.AddMechanicToFighter(this, m_data.Move1Str, MechanicType.STRENGTH);
                break;
            case "hitTwenty":
                m_animation.Play(ANIM_ATTACK2, finishCallback);
                int damage = m_data.Move2Damage;
                int strStack = GameInfoHelper.GetMechanicStack(this, MechanicType.STRENGTH);
                
                strStack = Mathf.Max(0, strStack);
                int finalDamage = damage + (strStack * m_data.Move2StrMultiplier);
                Debug.Log($"damage: {damage}, str: {strStack}, multiplyer: {m_data.Move2StrMultiplier}, finalDamage: {finalDamage}");
                GameActionHelper.DamagePlayer(this, finalDamage);
                break;
            case "shoutOne":
                m_animation.Play(ANIM_GROW, finishCallback);
                PerformShoutOne();
                break;
            case "shoutTwo":
                m_animation.Play(ANIM_GROW, finishCallback);
                PerformShoutTwo();
                break;
            case "strBigger":
                m_animation.Play(ANIM_ATTACK, finishCallback);
                GameActionHelper.DamagePlayer(this, m_data.Move5Damage);
                break;
        }

        yield return null;
    }

    private void PerformShoutOne()
    {
        GameActionHelper.AddMechanicToFighter(this, m_data.Move3Str, MechanicType.STRENGTH);
        GameActionHelper.ReduceMechanicStack(GameInfoHelper.GetPlayer(), m_data.Move3StrReduce, MechanicType.STRENGTH);
    }

    private void PerformShoutTwo()
    {
        GameActionHelper.HealFighter(this, m_data.Move4Restore);
    }

    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }
}