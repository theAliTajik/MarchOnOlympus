using System;
using Game;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Spine.Unity.Examples;
using UnityEngine;


public class Assassin : BaseEnemy
{
    #region Animations
    
    private const string ANIM_01_IDLE = "01_idle";
    private const string ANIM_02_WOUNDED = "02_Wounded";
    private const string ANIM_03_WOUNDED_IDLE = "03_Wounded_idle";
    private const string ANIM_04_DEATH = "04_Death";
    private const string ANIM_05_ATTACK = "05_Attack";
    private const string ANIM_CAST_CLON = "Cast_Clon";
    private const string ANIM_IDLECLON = "idleClon";
    private const string ANIM_WOUNDED_CLON = "Wounded_Clon";
    private const string ANIM_WOUNDED_IDLE_CLON = "Wounded_idle_Clon";
    
    #endregion
    
    [SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] private AssassinMovesData m_data;
    [SerializeField] private string CloneEnemyId;
    [SerializeField] private Vector3[] m_clonesOffset;
    
    private List<AssassinClone> m_cloneList = new List<AssassinClone>();

    private bool m_hasClones = false;
    private bool m_firstTimeReaching66Precent = true;
    private bool m_firstTimeReaching33Precent = true;

    
    protected override void Awake()
    {
        base.Awake();


        SetMoves(m_movesDatas);
        
        ConfigFighterHP();
            
        HP.SetTrigger(m_data.Phase1PercentageTrigger);
        HP.SetTrigger(m_data.Phase2PercentageTrigger);

        HP.OnPercentageTrigger += OnHPPercentageTriggred;
    }

    private void OnHPPercentageTriggred(FighterHP.TriggerPercentage percent)
    {
        Debug.Log("perecent: " + percent);
        StartCoroutine(SpawnClones());
    }

    private void Start()
    {
        CombatManager.Instance.OnCombatPhaseChanged += PhaseChanged;
        GameplayEvents.MechanicAddedToFighter += OnMechanicAdded;
    }

    private void OnDestroy()
    {
        GameplayEvents.MechanicAddedToFighter -= OnMechanicAdded;
    }

    private void PhaseChanged(CombatPhase phase)
    {
        if (phase == CombatPhase.CARD_PLAYED)
        {
            CombatManager.Instance.Player.TakeDamage(m_data.EachCardUsedByPlayerDamageToPlayer, this, false);
        }
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(ANIM_02_WOUNDED);
    }

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_04_DEATH);
    }
    

    public override void DetermineIntention()
    {
        RandomIntentionPicker(m_moves);
        SetClonesIntentions();
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();
        switch (m_nextMove.clientID)
        {
            case "Hit20":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "Bleed3":
                CallOnIntentionDetermined(Intention.CAST_DEBUFF, m_nextMove.description);
                break;
            case "Send3Cards":
                CallOnIntentionDetermined(Intention.CAST_DEBUFF, m_nextMove.description);
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
            case "Hit20":
                CombatManager.Instance.Player.TakeDamage(m_data.Move1Damage, this, true);
                m_animation.Play(ANIM_05_ATTACK, finishCallback);
                break;
            case "Bleed3":
                MechanicsManager.Instance.AddMechanic(new BleedMechanic(m_data.Move2Bleed, CombatManager.Instance.Player), this);
                MechanicsManager.Instance.AddMechanic(new BlockMechanic(m_data.Move2Block, this), this);
                m_animation.Play(ANIM_05_ATTACK, finishCallback);
                break;
            case "Send3Cards":
                for (int i = 0; i < m_data.Move3NumOfCards; i++)
                {
                    CombatManager.Instance.SpawnCard(m_data.Move3Card, CardStorage.DRAW_PILE);
                }
                CombatManager.Instance.ShuffleDeckDrawPile();
                m_animation.Play(ANIM_05_ATTACK, finishCallback);
                break;
        }

        yield return null;
    }
    
    

    private IEnumerator SpawnClones()
    {
        yield return new WaitForSeconds(0.5f);
        yield return WaitForAnimation(ANIM_CAST_CLON, () => {});
        
        for (int i = 0; i < 2; i++)
        {
            //Vector3 clonePos = m_clonesOffset[i] + transform.position;
            AssassinClone clone = (AssassinClone)EnemiesManager.Instance.SpawnBoss(CloneEnemyId)[0];
            clone.Death += OnCloneDeath;
            m_cloneList.Add(clone);
        }
        SetClonesIntentions();
        m_hasClones = true;
        yield return null;
    }

    private void SetClonesIntentions()
    {
        if (m_cloneList == null || m_cloneList.Count <= 0)
        {
            return;
        }
        foreach (AssassinClone assassinClone in m_cloneList)
        {
            assassinClone.SetIntention(m_nextMove);
        }
    }

    private void OnCloneDeath(Fighter fighter)
    {
        m_cloneList.Remove(fighter as AssassinClone);
        StartCoroutine(DestroyCloneAfterDelay(fighter, 2));

        if (m_cloneList.Count == 0)
        {
            m_hasClones = false;
        }
    }

    private IEnumerator DestroyCloneAfterDelay(Fighter fighter, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        EnemiesManager.Instance.RemoveDeadEnemy(fighter);
    }

    private void OnMechanicAdded(Fighter fighter,BaseMechanic mechanic)
    {
        if (mechanic.GetMechanicType() != MechanicType.BLEED)
        {
            return;
        }

        if (fighter == this)
        {
            MechanicsManager.Instance.RemoveMechanic(this, MechanicType.BLEED);
        }
    }
    

    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

    
}
