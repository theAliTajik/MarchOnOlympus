using System;
using System.Collections;
using UnityEngine;


public class Diomedes : BaseEnemy
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
    
    [SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] private DiomedesMovesData m_data;
    
    protected override void Awake()
    {
        base.Awake();


        ConfigFighterHP();
        
        SetMoves(m_movesDatas);
    }

    private void Start()
    {
        HP.SetTrigger(m_data.Phase1Trigger);

        HP.OnPercentageTrigger += OnHPPercentageTriggred;
    }

    private void OnHPPercentageTriggred(FighterHP.TriggerPercentage percentage)
    {
        Debug.Log("percentage triggered: :" + percentage.Percentage);
        GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Phase1Damage);
        OnPhase1?.Invoke();
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
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_DEAD_COMONWARRIOR);
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
            case "hit":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "hitBlock":
                CallOnIntentionDetermined(Intention.BLOCK, m_nextMove.description);
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
        switch (m_nextMove.clientID)
        {
            case "hit":
                m_animation.Play(ANIM_ATTACK_COMONWARRIOR, finishCallback);
                yield return new WaitForSeconds(1f);
                CombatManager.Instance.Player.TakeDamage(m_data.Move1Damage, this, true);
                break;
            case "hitBlock":
                bool hitAnimationDone = false;
                m_animation.Play(ANIM_ATTACK_COMONWARRIOR, () => hitAnimationDone = true);
                yield return new WaitForSeconds(1f);
                CombatManager.Instance.Player.TakeDamage(m_data.Move2Damage, this, true);
                yield return new WaitUntil(() => hitAnimationDone);
                MechanicsManager.Instance.AddMechanic(new BlockMechanic(m_data.Move2Block, this), this);
                m_animation.Play(ANIM_ABILITY_COMONWARRIOR, finishCallback);
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
