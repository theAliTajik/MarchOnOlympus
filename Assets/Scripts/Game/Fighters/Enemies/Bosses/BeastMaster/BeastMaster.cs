using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game;
using KaimiraGames;
using Spine.Unity;
using Spine;
using Unity.VisualScripting;


public class BeastMaster : BaseEnemy
{
    #region Anims

    private const string ANIM_01_IDDLE = "01_Iddle";
    private const string ANIM_02_WOUNDS = "02_Wounds";
    private const string ANIM_03_DAETH = "03_Daeth";
    private const string ANIM_04_ATTACK = "04_Attack";
    private const string ANIM_05_CAST = "05_Cast";

    #endregion
    
    [SerializeField] protected MoveData[] m_movesDatas;

    [SerializeField] private BeastMasterMovesData m_data;
    [SerializeField] private FighterHP.TriggerPercentage[] m_triggers;
    
    private Dictionary<EPhase, BaseAnimal> m_animals = new Dictionary<EPhase, BaseAnimal>();
    
    [Serializable]
    private enum EPhase
    {
        WOLF,
        EAGLE,
        BEAR,
        END
    }
    private EPhase m_phase = EPhase.WOLF;

    private int m_DamageBonus = 0;
    private bool m_firstTimeReaching33PrecentOfHP = true;
    
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

    private void Start()
    {
        for (var i = 0; i < m_triggers.Length; i++)
        {
            HP.SetTrigger(m_triggers[i]);
        }

        HP.OnPercentageTrigger += OnHPPercentageTriggred;
    }

    private void OnHPPercentageTriggred(FighterHP.TriggerPercentage percentage)
    {
        Debug.Log("percentage triggered: :" + percentage);
        ReleaseAnimal();
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(ANIM_02_WOUNDS);
    }

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_03_DAETH);
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
            case "Hit":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "HitAnimalCount":
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
            case "Hit":
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move1Damage);
                m_animation.Play(ANIM_04_ATTACK, finishCallback);
                break;
            case "HitAnimalCount":
                if (m_animals.Count > 0)
                {
                    GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move2Damage * m_animals.Count);
                    m_animation.Play(ANIM_04_ATTACK, finishCallback);
                }
                else
                {
                    finishCallback?.Invoke();
                }
                break;
        }
    }

    public List<BaseAnimal> GetAllBeasts()
    {
        return m_animals.Values.ToList();
    }

    public void ReleaseAnimal()
    {
        Debug.Log("animal released: " + m_phase.ToString());

        BaseAnimal animal = null;
        switch (m_phase)
        {
            case EPhase.BEAR:
                animal = EnemiesManager.Instance.SpawnBoss("Bear")[0] as BaseAnimal;
                break;
            case EPhase.EAGLE:
                animal = EnemiesManager.Instance.SpawnBoss("Eagle")[0] as BaseAnimal;
                break;
            case EPhase.WOLF:
                animal = EnemiesManager.Instance.SpawnBoss("Wolf")[0] as BaseAnimal;
                break;
        }

        if (animal != null)
        {
            animal.SetMaster(this);
            m_animals.Add(m_phase, animal);
            animal.DetermineIntention();
        }
        m_phase = (EPhase)((int)m_phase + 1);
        if (m_phase == EPhase.END)
        {
            m_phase = (EPhase)0;
        }
    }
    
    
    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }
}