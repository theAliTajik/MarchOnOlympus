using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class Soul : BaseAnimal
{
        #region Anims

        private const string ANIM_ATTACK = "Attack";
        private const string ANIM_DEATH = "Death";
        private const string ANIM_HOWL = "Howl";
        private const string ANIM_IDDLE = "Iddle";
        private const string ANIM_WOUND = "Wound";

    #endregion
    
    
    [SerializeField] protected MoveData[] m_movesDatas;
    
    private MoveData[] m_movesToChooseFrom;
    
    [SerializeField] private SoulMasterMovesData m_data;
    [SerializeField] private SoulMovesData m_souldata;

    private bool m_soulMasterDeterminedIntention = false;
 
    private SoulMaster m_soulMaster;
    
    protected override void Awake()
    {
        base.Awake();

        ConfigFighterHP();

        SetMoves(m_movesDatas);

        m_soulMaster = FindSoulMaster();
        if (m_soulMaster != null)
        {
            m_soulMaster.OnSoulIntentionDetermined += OnSoulMasterIntentionDetermined;
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
        m_soulMaster.OnSoulDeath();
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
            case "Bleed":
                CallOnIntentionDetermined(Intention.CAST_DEBUFF, m_nextMove.description);
                break;
            case "HitPiercing":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "HitDaze":
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
            case "Bleed":
                m_animation.Play(ANIM_HOWL, finishCallback);
                GameActionHelper.AddMechanicToPlayer(m_data.Move3BleedGain, MechanicType.BLEED);
                break;
            case "HitPiercing":
                m_animation.Play(ANIM_ATTACK, finishCallback);
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move4Damage, true);
                break;
            case "HitDaze":
                m_animation.Play(ANIM_ATTACK, finishCallback);
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move5Damage);
                GameActionHelper.AddMechanicToPlayer(m_data.Move5DazeGain, MechanicType.DAZE);
                break;
        }
    }
    
        
    
    private SoulMaster FindSoulMaster()
    {
        List<Fighter> allEnemies = GameInfoHelper.GetAllEnemies();
        SoulMaster soulMaster = allEnemies.Find(enemy => enemy.GetType().Name == "SoulMaster") as SoulMaster;

       
        
        return soulMaster;
    }

    private void OnSoulMasterIntentionDetermined(MoveData moveData)
    {
        m_movesToChooseFrom = new MoveData[m_movesDatas.Length];

        int j = 0;
        for (var i = 0; i < m_movesDatas.Length; i++)
        {
            if (m_movesDatas[i].clientID == moveData.clientID)
            {
                continue;
            }
            
            m_movesToChooseFrom[j] = m_movesDatas[i];
            j++;
        }
        
        m_moves.Clear();
        
        for (int i = 0; i < m_movesToChooseFrom.Length; i++)
        {
            MoveData md = m_movesToChooseFrom[i];
            m_moves.Add(md, md.chance);
        }
    }
    
    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_souldata.HP);
        m_fighterHP.ResetHP();
    }

}
