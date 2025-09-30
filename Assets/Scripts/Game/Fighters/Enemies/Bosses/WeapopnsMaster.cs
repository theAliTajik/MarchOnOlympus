using System;
using Game;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class WeapopnsMaster : BaseEnemy
{
    #region Animations
    
    private const string ANIM_01_IDDLE = "01_Iddle";
    private const string ANIM_01_IDDLE2 = "01_Iddle2";
    private const string ANIM_01_IDDLE3 = "01_Iddle3";
    private const string ANIM_ATTACK_ONEHAND_SWORD = "Attack_oneHand_Sword";
    private const string ANIM_ATTACK_SWORD_SHELD = "Attack_Sword_Sheld";
    private const string ANIM_ATTACK_SWORD_SHELD_2 = "Attack_Sword_Sheld_2";
    private const string ANIM_ATTACKSPEAR = "AttackSpear";
    private const string ANIM_DEATH = "Death";
    private const string ANIM_SWAP_SHELD = "Swap_Sheld";
    private const string ANIM_SWAP_SPEAR_SWORDSHELD = "Swap_Spear_SwordSheld";
    private const string ANIM_SWAP_SWORD_SPEAR = "Swap_Sword_Spear";
    private const string ANIM_TAUNT_SPEAR = "Taunt_Spear";
    private const string ANIM_TAUNT_SWORDSHELD = "Taunt_SwordSheld";
    private const string ANIM_WOUNDE = "Wounde";
    private const string ANIM_WOUNDE2 = "Wounde2";
    private const string ANIM_WOUNDE3 = "Wounde3";

    #endregion
    
    [SerializeField] protected MoveData[] m_phase1movesDatas;
    [SerializeField] protected MoveData[] m_phase2movesDatas;
    [SerializeField] protected MoveData[] m_phase3movesDatas;

    [SerializeField] private WeaponsMasterMovesData m_data;

    public enum phase
    {
        ONE,
        TWO,
        THREE
    }
    
    public phase m_phase = phase.ONE;
    
    protected override void Awake()
    {
        base.Awake();


        SetMoves(m_phase1movesDatas);
        
        ConfigFighterHP();
        
        HP.SetTrigger(m_data.Phase2PercentageTrigger);
        HP.SetTrigger(m_data.Phase3PercentageTrigger);

        HP.OnPercentageTrigger += OnHPPercentageTriggred;
    }

    private void OnHPPercentageTriggred(FighterHP.TriggerPercentage percent)
    {
        Debug.Log("perecent: " + percent);
        if (percent == m_data.Phase2PercentageTrigger)
        {
            ChangePhase(phase.TWO);
        }
        
        if (percent == m_data.Phase3PercentageTrigger)
        {
            ChangePhase(phase.THREE);
        }
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(ANIM_WOUNDE3);
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

    private void ChangePhase(phase phase)
    {
        if (m_phase == phase)
        {
            return;
        }

        Debug.Log("phase changed to: " + phase);
        m_phase = phase;
        m_previusMove = null;
        MoveData[] m_phaseMoves = null;
        switch (phase)
        {
            case phase.ONE:
                m_phaseMoves = m_phase1movesDatas;
                break;
            case phase.TWO:
                m_phaseMoves = m_phase2movesDatas;
                break;
            case phase.THREE:
                m_phaseMoves = m_phase3movesDatas;
                break;
        }
        m_moves.Clear();
        for (int i = 0; i < m_phaseMoves.Length; i++)
        {
            MoveData md = m_phaseMoves[i];
            m_moves.Add(md, md.chance);
        }
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
            case "sendCard":
                CallOnIntentionDetermined(Intention.CAST_DEBUFF, m_nextMove.description);
                break;
            case "hitRandom":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "hitBlock":
                CallOnIntentionDetermined(Intention.BLOCK, m_nextMove.description);
                break;
            case "restoreBlock":
                CallOnIntentionDetermined(Intention.BLOCK, m_nextMove.description);
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
            case "hit":
                CombatManager.Instance.Player.TakeDamage(m_data.Move1Damage, this, true);
                m_animation.Play(ANIM_ATTACKSPEAR, finishCallback);
                break;
            case "sendCard":
                for (int i = 0; i < m_data.Move2NumOfCards; i++)
                {
                    CombatManager.Instance.SpawnCard(m_data.Move2Card, CardStorage.DRAW_PILE);
                }
                CombatManager.Instance.ShuffleDeckDrawPile();
                m_animation.Play(ANIM_TAUNT_SPEAR, finishCallback);
                break;
            case "hitRandom":
                int RandomChoice = UnityEngine.Random.Range(0, 5);
                CombatManager.Instance.Player.TakeDamage(m_data.Move3Damages[RandomChoice], this, true);
                m_animation.Play(ANIM_ATTACKSPEAR, finishCallback);
                break;
            case "hitBlock":
                CombatManager.Instance.Player.TakeDamage(m_data.Move4Damage, this, true);
                yield return WaitForAnimation(ANIM_ATTACKSPEAR);
                MechanicsManager.Instance.AddMechanic(new BlockMechanic(m_data.Move4Block, this), this);
                m_animation.Play(ANIM_TAUNT_SWORDSHELD, finishCallback);
                break;
            case "restoreBlock":
                this.Heal(m_data.Move5Restore);
                MechanicsManager.Instance.AddMechanic(new BlockMechanic(m_data.Move5Block, this), this);
                m_animation.Play(ANIM_TAUNT_SWORDSHELD, finishCallback);
                break;
        }
    }

    /*
    public override int TakeDamage(int damage, Fighter sender, bool doesReturnToSender)
    {
        int result = base.TakeDamage(damage, sender, doesReturnToSender);


        float phase1Precent = m_data.PhaseOneUntilThisPrecentOfHP / 100f;
        float phase2Precent = m_data.PhaseTwoUntilThisPrecentOfHP / 100f;
        
        if (m_fighterHP.Current <= m_fighterHP.Max * phase2Precent)
        {
            ChangePhase(phase.THREE);
        }
        else if (m_fighterHP.Current <= m_fighterHP.Max * phase1Precent)
        {
            ChangePhase(phase.TWO);
        }
        else
        {
            ChangePhase(phase.ONE);
        }


        return result;
    }
    */
    
    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }
    
#if UNITY_EDITOR
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            this.TakeDamage(10, this, false);
        }
    }

#endif
}
