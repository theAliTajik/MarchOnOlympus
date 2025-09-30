using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class Hector : BaseEnemy
{
    #region Anims

    private const string ANIM_01_IDDLE = "01_Iddle";
    private const string ANIM_01_SPELL = "01_Spell";
    private const string ANIM_02_ATTACK = "02_Attack";
    private const string ANIM_03_BLOCK = "03_Block";
    private const string ANIM_04_WOUND = "04_Wound";
    private const string ANIM_05_DEATH = "05_Death";

    #endregion
    
    [SerializeField] protected MoveData[] m_movesDatas;

    [SerializeField] private HectorMovesData m_data;

    private int m_charge = 0;
    
    protected override void Awake()
    {
        base.Awake();

        ConfigFighterHP();

SetMoves(m_movesDatas);
    }

    private void Start()
    {
        HP.SetTrigger(m_data.Phase1PercentageTrigger);

        HP.OnPercentageTrigger += OnHPPercentageTriggred;
    }

    private void OnHPPercentageTriggred(FighterHP.TriggerPercentage percentage)
    {
        Debug.Log("percentage triggered: :" + percentage.Percentage);
        GameActionHelper.AddMechanicToFighter(this, m_data.Phase1StrGain, MechanicType.STRENGTH);
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(ANIM_04_WOUND);
    }

    public override void Heal(int heal)
    {
        base.Heal(heal);
        m_animation.Play(ANIM_01_SPELL);
    }

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_05_DEATH);
    }
    

    public override void DetermineIntention()
    {
        RandomIntentionPicker();
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();

        string blockMoveDesc = m_movesDatas[0].description;
        switch (m_nextMove.clientID)
        {
            case "Charge":
                CallOnIntentionDetermined(Intention.BUFF, blockMoveDesc +". " + m_nextMove.description);
                break;
            case "Hit":
                CallOnIntentionDetermined(Intention.ATTACK, blockMoveDesc +". " + m_nextMove.description);
                break;
            case "SendCards":
                CallOnIntentionDetermined(Intention.CAST_DEBUFF, blockMoveDesc +". " + m_nextMove.description);
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

        // play block move
        bool blockAnimDone = false;
        GameActionHelper.AddMechanicToFighter(this, m_data.Move1Block, MechanicType.BLOCK);
        m_animation.Play(ANIM_03_BLOCK, () => { blockAnimDone = true; });
        yield return new WaitUntil(() => blockAnimDone);
        
        switch (m_nextMove.clientID)
        {
            case "Charge":
                m_charge += m_data.Move2ChargeGain;
                if (m_charge > m_data.Move2ChargeThreshold)
                {
                    GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move2Damage);
                    m_charge = 0;   
                    m_animation.Play(ANIM_02_ATTACK, finishCallback);
                }
                else
                {
                    m_animation.Play(ANIM_01_SPELL, finishCallback);
                }
                break;
            case "Hit":
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move3Damage);
                GameActionHelper.AddMechanicToFighter(this, 1, m_data.Move3MechanicGain);
                m_animation.Play(ANIM_02_ATTACK, finishCallback);
                break;
            case "SendCards":
                for (int i = 0; i < m_data.Move4NumOfCards; i++)
                {
                    Debug.Log("make cards and send it");
                    GameActionHelper.SpawnCard(m_data.Move4Card, CardStorage.DRAW_PILE);
                }

                yield return new WaitForSeconds(0.5f);
                m_animation.Play(ANIM_01_SPELL, finishCallback);
                break;
        }
    }

    public override DamageContext TakeDamage(int damage, Fighter sender, bool doesReturnToSender,
        bool isArmorPiercing = false,
        DamageContext damageContext = null)
    {
        DamageContext result = base.TakeDamage(damage, sender, doesReturnToSender, isArmorPiercing, damageContext);
        m_charge -= m_data.Move2ChargeReduction;
        if (m_charge < 0)
        {
            m_charge = 0;
        }
        
        return result;
    }

    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

}
