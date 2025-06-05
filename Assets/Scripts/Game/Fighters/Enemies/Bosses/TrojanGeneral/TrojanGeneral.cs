using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class TrojanGeneral : BaseEnemy
{
    #region Anims

    private const string ANIM_ATTACK = "Attack";
    private const string ANIM_ATTACK2 = "Attack2";
    private const string ANIM_CAST = "Cast";
    private const string ANIM_DEATH = "Death";
    private const string ANIM_IDDLE = "Iddle";
    private const string ANIM_WOUND = "Wound";

    #endregion
    
    [SerializeField] protected MoveData[] m_movesDatas;

    [SerializeField] private TrojanGeneralMovesData m_data;

    
    
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
        RandomIntentionPicker(m_moves);
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();

        switch (m_nextMove.clientID)
        {
            case "Hit2":
                CallOnIntentionDetermined(Intention.MULTI_ATTACK, m_nextMove.description);
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

        // play block move
        
        switch (m_nextMove.clientID)
        {
            case "Hit2":
                for (int i = 0; i < m_data.Move1DamageTimes; i++)
                {
                    bool attackDone = false;
                    m_animation.Play(ANIM_ATTACK, () => {attackDone = true;});
                    GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move1Damage);
                    yield return new WaitUntil(() => attackDone);
                }
                
                finishCallback?.Invoke();
                GameActionHelper.AddMechanicToPlayer(m_data.Move1Bleed, MechanicType.BLEED);
                break;
            case "Hit":
                m_animation.Play(ANIM_ATTACK, finishCallback);
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move2Damage);
                GameActionHelper.AddMechanicToFighter(this, m_data.Move2Fortify, MechanicType.FORTIFIED);
                break;
        }
    }

    public void SetTower(TrojanGeneralTower tower)
    {
        tower.Death += OnTowerDeath;
    }

    public void SetFortify()
    {
        GameActionHelper.AddMechanicToFighter(this, 1, MechanicType.FORTIFIED, true, 1);
    }
    
    private TrojanGeneralTower FindTower()
    {
        List<Fighter> allEnemies = GameInfoHelper.GetAllEnemies();
        TrojanGeneralTower tower = allEnemies.Find(enemy => enemy.GetType().Name == "TrojanGeneralTower") as TrojanGeneralTower;
        tower.Death += OnTowerDeath;
        return tower;
    }

    private void OnTowerDeath(Fighter tower)
    {
        GameActionHelper.RemoveMechanicGuard(this, MechanicType.FORTIFIED);
        GameActionHelper.ReduceMechanicStack(this, 1, MechanicType.FORTIFIED);
    }


    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

}
