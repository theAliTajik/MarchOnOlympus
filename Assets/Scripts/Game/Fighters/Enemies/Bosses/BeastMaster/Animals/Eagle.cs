using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : BaseAnimal
{
        #region Anims

        private const string ANIM_ATTACK = "Attack";
        private const string ANIM_DEATH = "Death";
        private const string ANIM_HOWL = "Howl";
        private const string ANIM_IDDLE = "Iddle";
        private const string ANIM_WOUND = "Wound";

    #endregion
    
    
    [SerializeField] protected MoveData[] m_movesDatas;
    
    [SerializeField] private EagleMovesData m_data;


 
    
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
        RandomIntentionPicker();
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();
        switch (m_nextMove.clientID)
        {
            case "Animal":
                CallOnIntentionDetermined(Intention.CAST_DEBUFF, m_nextMove.description);
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
            case "Animal":
                DoSpecialMove();
                m_animation.Play(ANIM_HOWL, finishCallback);
                break;
            case "Hit":
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move2Damage);
                m_animation.Play(ANIM_ATTACK, finishCallback);
                break;
        }
    }

    public void DoSpecialMove()
    {
        SpecialMove(null);
    }

    public override void SpecialMove(Fighter fighter)
    {
        GameActionHelper.AddMechanicToPlayer(1, m_data.Move1MechanicType);
    }
    
    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

}
