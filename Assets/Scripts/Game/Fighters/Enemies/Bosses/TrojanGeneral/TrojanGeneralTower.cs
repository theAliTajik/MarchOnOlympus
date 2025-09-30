using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrojanGeneralTower : BaseEnemy
{
    #region Anims


    #endregion
    
    [SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] protected MoveData[] m_specialMovesDatas;


    [SerializeField] private TrojanGeneralTowerMovesData m_data;

    
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
    }
    

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
    }
    

    public override void DetermineIntention()
    {
        if (m_nextMove.clientID == "Hit")
        {
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
            case "Pump":
                CallOnIntentionDetermined(Intention.BUFF, m_nextMove.description);
                break;
            case "Hit":
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
            case "Pump":
                m_nextMove = m_specialMovesDatas[0];
                break;
            case "Hit":
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move2Damage);
                m_nextMove = m_movesDatas[0];
                break;
        }
        finishCallback?.Invoke();
    }
    

    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

}
