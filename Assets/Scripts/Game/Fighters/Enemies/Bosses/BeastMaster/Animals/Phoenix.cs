using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Phoenix : BaseEnemy
{
    #region Animations

    private const string ANIM_ATTACK_V1 = "Attack_V1";
    private const string ANIM_ATTACK_V5 = "Attack_V5";
    private const string ANIM_ATTACK_V6 = "Attack_V6";
    private const string ANIM_DEATH = "Death";
    private const string ANIM_IDDLE = "Iddle";
    private const string ANIM_VFX_SHELD_LOOP = "VFX_Sheld_Loop";
    private const string ANIM_VFX_SHELDCAST = "VFX_SheldCast";
    private const string ANIM_WOUND = "Wound";
    
    #endregion

    [SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] private MoveData m_startPumpMove;
    [SerializeField] private MoveData m_pumpMove;
    [SerializeField] private MoveData m_afterPumpMove;
    
    [SerializeField] private PhoenixMovesData m_data;

    private Animator m_unityAnimator;
    private const string m_animatorBoolName = "HasBlock";
    
    private ITurnCounter m_turnCounter;
    private const int m_numOfPumps = 3;

    protected override void Awake()
    {
        base.Awake();

        SetMoves(m_movesDatas);
        ConfigFighterHP();

        GameplayEvents.MechanicAddedToFighter += OnMechanicsChanged;
        m_unityAnimator = GetComponent<Animator>();
    }

    private void OnMechanicsChanged(Fighter fighter, BaseMechanic mechanic)
    {
        if(fighter != this) return;
        if(mechanic.GetMechanicType() != MechanicType.BLOCK) return;
        
        m_unityAnimator.SetBool(m_animatorBoolName, true);
        m_animation.Play(ANIM_VFX_SHELDCAST);
        mechanic.OnEnd += OnBlockMechanicEnd;
        CustomDebug.Log("Block added", Categories.Fighters.Enemies.Phoenix, DebugTag.ANIMATION);
    }

    private void OnBlockMechanicEnd(MechanicType mechanicType)
    {
        m_unityAnimator.SetBool(m_animatorBoolName, false);
        CustomDebug.Log("Block removed", Categories.Fighters.Enemies.Phoenix, DebugTag.ANIMATION);
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
            CustomDebug.Log("66% Triggered", Categories.Fighters.Enemies.Phoenix, DebugTag.LOGIC);
			GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.At66Damage);
            Heal(m_data.At66Restore);
		}

		if (percentage == m_data.Phase2HPPercentageTrigger)
        {
            CustomDebug.Log("33% Triggered", Categories.Fighters.Enemies.Phoenix, DebugTag.LOGIC);
			GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.At33Damage);
            m_nextMove = m_startPumpMove;
            ShowIntention();
        }

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
        m_animation.Play(ANIM_DEATH);
    }

    public override void DetermineIntention()
    {
        m_turnCounter?.NextTurn();

        CustomDebug.Log($"Turn counter is null: {m_turnCounter == null}", Categories.Fighters.Enemies.Phoenix, DebugTag.LOGIC);
        if (m_turnCounter == null)
        {
            RandomIntentionPicker();
            ShowIntention();
            return;
        }

        if (m_turnCounter.GetRelativeTurn() == 3)
        {
            m_nextMove = m_afterPumpMove;
            ShowIntention();
            return;
        }

        m_nextMove = m_pumpMove;
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();

        switch (m_nextMove.clientID)
        {
            case "BurnDaze":
                CallOnIntentionDetermined(Intention.CAST_DEBUFF, m_nextMove.description);
                break;
            case "BurnRestore":
                CallOnIntentionDetermined(Intention.CAST_DEBUFF, m_nextMove.description);
                break;
            case "BlockRestore":
                CallOnIntentionDetermined(Intention.BLOCK, m_nextMove.description);
                break;
            case "startPump":
                CallOnIntentionDetermined(Intention.BLOCK, m_nextMove.description);
                break;
            case "pump":
                string formatedDesc = string.Format(m_nextMove.description, m_numOfPumps - m_turnCounter.GetRelativeTurn());
                CallOnIntentionDetermined(Intention.BUFF, formatedDesc);
                break;
            case "afterPump":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
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
        switch (m_nextMove.clientID)
        {
            case "BurnDaze":
                m_animation.Play(PickRandomAttackAnim(), finishCallback);
                GameActionHelper.AddMechanicToFighter(GameInfoHelper.GetPlayer(), m_data.Move1Burn, MechanicType.BURN);
				GameActionHelper.AddMechanicToFighter(GameInfoHelper.GetPlayer(), m_data.Move1Daze, MechanicType.DAZE);
				break;
            case "BurnRestore":
                m_animation.Play(PickRandomAttackAnim(), finishCallback);
				GameActionHelper.AddMechanicToFighter(GameInfoHelper.GetPlayer(), m_data.Move2Burn, MechanicType.BURN);
                Heal(m_data.Move2Restore);
                break;
            case "BlockRestore":
                m_animation.Play(PickRandomAttackAnim(), finishCallback);
				GameActionHelper.AddMechanicToFighter(this, m_data.Move3Block, MechanicType.BLOCK);
				Heal(m_data.Move3Restore);
				break;
            case "startPump":
                m_animation.Play(PickRandomAttackAnim(), finishCallback);
                GameActionHelper.AddMechanicToFighter(this, m_data.Move4Block, MechanicType.BLOCK);
                m_turnCounter = new CyclicalEnemyTurnCounter(m_numOfPumps);
                CustomDebug.Log("Started pump", Categories.Fighters.Enemies.Phoenix, DebugTag.MOVE_CHOICE);
                break;
            case "pump":
                CustomDebug.Log("Pump", Categories.Fighters.Enemies.Phoenix, DebugTag.MOVE_CHOICE);
                // m_animation.Play(ANIM_VFX_SHELDCAST, finishCallback);
                break;
            case "afterPump":
                CustomDebug.Log("After pump", Categories.Fighters.Enemies.Phoenix, DebugTag.MOVE_CHOICE);
                m_animation.Play(PickRandomAttackAnim(), finishCallback);
                m_turnCounter = null;
                int blockStack = GameInfoHelper.GetMechanicStack(this, MechanicType.BLOCK);
                if (blockStack > 0)
                {
                    GameActionHelper.HealFighter(this, m_data.Move4Restore);
                }
                else
                {
                    GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move4Damage);
                    GameActionHelper.DamageFighter(this, this, m_data.Move4Damage, doesReturnToSender:false);
                    finishCallback?.Invoke();
                }
                break;
        }

        yield return new WaitForSeconds(2.6f);
        finishCallback?.Invoke();
    }

    private string PickRandomAttackAnim()
    {
        int rand = Random.Range(0, 2);

        switch (rand)
        {
            case 0:
                return ANIM_ATTACK_V1;
            case 1:
                return ANIM_ATTACK_V5;
            case 2:
                return ANIM_ATTACK_V6;
            default:
                CustomDebug.Log("Random Attack anim returned null", Categories.Fighters.Enemies.Phoenix, DebugTag.ANIMATION);
                return null;
        }
    }
    
    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }
}