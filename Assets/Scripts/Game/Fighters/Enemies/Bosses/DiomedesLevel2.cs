using System;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DiomedesLevel2 : BaseEnemy
{
    #region Animations
    
    private const string ANIM__WOUND = "_Wound";
    private const string ANIM_ABILITY = "Ability";
    private const string ANIM_ATTACK = "Attack";
    private const string ANIM_DEAD = "Dead";
    private const string ANIM_IDLE = "idle";
    private const string ANIM_WOUND = "Wound";
    private const string ANIM_WOUND_BLOCK = "Wound_Block";
    private const string ANIM_WOUND_IDDLE = "Wound_Iddle";
    
    #endregion
    
    [SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] private DiomedesLevel2MovesData m_data;
    
    protected override void Awake()
    {
        base.Awake();


        ConfigFighterHP();
        
SetMoves(m_movesDatas);
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        if (MechanicsManager.Instance.Contains(this, MechanicType.BLOCK))
        {
            m_animation.Play(ANIM_WOUND_BLOCK);
        }
        else
        {
            m_animation.Play(ANIM__WOUND);
        }
    }

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_DEAD);
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
            case "hitBleed":
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
            case "hitBleed":
                m_animation.Play(ANIM_ATTACK, finishCallback);
                yield return new WaitForSeconds(0.8f);
                CombatManager.Instance.Player.TakeDamage(m_data.Move1Damage, this, true);
                MechanicsManager.Instance.AddMechanic(new BleedMechanic(m_data.Move1Bleed, CombatManager.Instance.Player), this);
                break;
            case "hitBlock":
                bool attackAnimFinished = false;
                m_animation.Play(ANIM_ATTACK, () => attackAnimFinished = true);
                yield return new WaitForSeconds(0.8f);
                CombatManager.Instance.Player.TakeDamage(m_data.Move2Damage, this, true);
                yield return new WaitUntil(() => attackAnimFinished);
                MechanicsManager.Instance.AddMechanic(new BlockMechanic(m_data.Move2Block, this), this);
                Heal(m_data.Move2Restore);
                m_animation.Play(ANIM_ABILITY, finishCallback);
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

