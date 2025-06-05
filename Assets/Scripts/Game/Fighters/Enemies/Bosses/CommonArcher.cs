using System;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CommonArcher : BaseEnemy
{
    #region Animations
    
    private const string ANIM_01_IDLE = "01_idle";
    private const string ANIM_02_DAMMAGE = "02_Dammage";
    private const string ANIM_03_WOUND = "03_Wound";
    private const string ANIM_04_DEAD = "04_dead";
    private const string ANIM_05_SHOOT = "05_Shoot";
    private const string ANIM_06_CRITICAL_SHOT = "06_Critical_Shot";
    
    #endregion
    
    [SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] private AjaxMovesData m_data;
    
    protected override void Awake()
    {
        base.Awake();

        ConfigFighterHP();

        for (int i = 0; i < m_movesDatas.Length; i++)
        {
            MoveData md = m_movesDatas[i];
            m_moves.Add(md, md.chance);
        }
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(ANIM_02_DAMMAGE);
    }

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_04_DEAD);
    }
    

    public override void DetermineIntention()
    {
        if (m_nextMove.clientID == "fortified")
        {
            ShowIntention();
            return;
        }
        RandomIntentionPicker(m_moves);

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
        switch (m_nextMove.clientID)
        {
            case "pump":
                m_nextMove.clientID = "fortified";
                m_animation.Play(ANIM_05_SHOOT, finishCallback);
                break;
            case "fortified":
                MechanicsManager.Instance.AddMechanic(new FortifiedMechanic(m_data.FortifiedStackAmount, this), this);
                CombatManager.Instance.Player.TakeDamage(m_data.Move1Damage, this, true);
                
                m_nextMove.clientID = "pump";
                m_animation.Play(ANIM_05_SHOOT, finishCallback);

                break;
            case "2hit":
                CombatManager.Instance.Player.TakeDamage(m_data.Hit2xDamage, this, true);
                yield return WaitForAnimation(ANIM_06_CRITICAL_SHOT);
                CombatManager.Instance.Player.TakeDamage(m_data.Hit2xDamage, this, true);
                finishCallback?.Invoke();
                break;
            case "hit10":
                CombatManager.Instance.Player.TakeDamage(m_data.Move3Damage, this, true);
                MechanicsManager.Instance.AddMechanic(new VulnerableMechanic(m_data.vulnerableStackAmount, CombatManager.Instance.Player), this);
                m_animation.Play(ANIM_05_SHOOT, () => finishCallback?.Invoke());
                break;
            case "block":
                MechanicsManager.Instance.AddMechanic(new BlockMechanic(m_data.BlockStackAmount, this), this);
                m_animation.Play(ANIM_05_SHOOT, finishCallback);
                break;
        }
    }
    
    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }
    
}
