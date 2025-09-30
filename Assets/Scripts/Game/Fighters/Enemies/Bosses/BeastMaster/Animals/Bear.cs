using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class Bear : BaseAnimal
{

    #region Anims

    private const string ANIM_ATTACK = "Attack";
    private const string ANIM_DEATH = "Death";
    private const string ANIM_IDDLE = "Iddle";
    private const string ANIM_ROAR = "Roar";
    private const string ANIM_WOUND = "Wound";

    #endregion
    
    
    [SerializeField] protected MoveData[] m_movesDatas;
    
    [SerializeField] private BearMovesData m_data;


 
    
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
            case "Animal":
                Debug.Log("gave armor");
                DoSpecialMove();
                m_animation.Play(ANIM_ROAR, finishCallback);
                break;
            case "Hit":
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move2Damage);
                m_animation.Play(ANIM_ATTACK, finishCallback);
                break;
        }
    }

    public void DoSpecialMove()
    {
        if (m_master != null)
        {
            List<BaseAnimal> animals = m_master.GetAllBeasts();
            for (int i = 0; i < animals.Count; i++)
            {
                SpecialMove(animals[i]);
            }
            SpecialMove(m_master);
        }
        else
        {
            SpecialMove(this);
        }
    }

    public override void SpecialMove(Fighter fighter)
    {
        GameActionHelper.AddMechanicToFighter(fighter, m_data.Move1Armor, MechanicType.BLOCK);
    }
    
    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }
}
