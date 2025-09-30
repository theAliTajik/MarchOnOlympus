using System;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class Hydra : BaseEnemy
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
    
    [SerializeField] private HydraMovesData m_data;
    
    [SerializeField] private HydraHeadConfigData m_headConfig;

    [SerializeField] private List<HydraHead> m_heads = new List<HydraHead>();
    private List<HydraHead> m_deadHeads = new List<HydraHead>();

    public HydraHeadConfigData HeadConfig => m_headConfig;

    private HydraHead m_previousHeadHit;

    public IEnumerator<HydraHead> GetEnumerator()
    {
        return m_heads.GetEnumerator();
    }
  
    protected override void Awake()
    {
        base.Awake();

        ConfigFighterHP();

        m_damageable = new HydraDamageBehaviour(this);
        GameplayEvents.ColliderSelected += OnColliderSelected;

        foreach (var head in m_heads)
        {
            head.OnDeath += OnHeadDeath;
        }
    }

    private void OnDestroy()
    {
	    GameplayEvents.ColliderSelected -= OnColliderSelected;
    }
    
    private void OnHeadDeath(HydraHead head)
    {
        m_deadHeads.Add(head);
        m_heads.Remove(head);
        
        DamageAll(m_data.OnHeadDestroyedDamage);
        GiveDebuffToAll(m_data.MechanicToAdd, m_data.NumOfMechanic);
        
        AreHeadsLessThanTwo();
        IsOnlyOneHeadLeft();
        AreNoHeadsLeft();
    }

    private void AreNoHeadsLeft()
    {
        if(m_heads.Count != 0) return;
        
        m_fighterHP.Kill();
    }

    private bool firstTime = true;
    private void AreHeadsLessThanTwo()
    {
        if(!firstTime) return;
        
        if (m_heads.Count > 2) return;
        
        firstTime = false;

        CustomDebug.Log("Heads Are Less Than 2", Categories.Fighters.Enemies.Hydra);
        int maxRevives = Math.Max(m_data.NumOfRevives, m_deadHeads.Count);
        List<HydraHead> headsToRevive = new List<HydraHead>();
        headsToRevive.AddRange(m_deadHeads);
        for (int i = 0; i < maxRevives; i++)
        {
            ReviveHead(headsToRevive[i]);
        }
    }

    private void IsOnlyOneHeadLeft()
    {
        if (m_heads.Count != 1)
        {
            return;
        }
        
        GameActionHelper.AddMechanicToPlayer(m_data.BurnAmount, MechanicType.BURN);
    }

    private void ReviveHead(HydraHead head)
    {
        CustomDebug.Log($"Reviving Head: {head.gameObject.name}", Categories.Fighters.Enemies.Hydra);
        m_heads.Add(head);
        m_deadHeads.Remove(head);

        head.Revive();
    }

    private void DamageAll(int damage)
    {
        GameActionHelper.DamagePlayer(this, damage);

        foreach (var head in m_heads)
        {
            head.TakeDamage(damage, null, false);
            CustomDebug.Log($"Damaged head: {head.gameObject.name}", Categories.Fighters.Enemies.Hydra);
        }
    }

    private void GiveDebuffToAll(MechanicType debuffType, int amount)
    {
        CustomDebug.Log($"Gave {amount} of Debuff {debuffType.GetType()}", Categories.Fighters.Enemies.Hydra);
        GameActionHelper.AddMechanicToPlayer(amount, debuffType);

        GameActionHelper.AddMechanicToOwner(this, amount, debuffType);
    }


    private void OnColliderSelected(Collider2D targetCollider)
    {
        HydraHead headHit = MatchColliderToHead(targetCollider);
        if (headHit == null)
        {
            return;
        }
        
        SetTargetedHead(headHit);
        
        if(m_previousHeadHit != null && m_previousHeadHit == headHit) return;
        m_previousHeadHit = headHit;
        
        CustomDebug.Log($"Set target head to: {headHit.gameObject.name}", Categories.Fighters.Enemies.Hydra);
    }

    private HydraHead MatchColliderToHead(Collider2D targetCollider)
    {
        foreach (var head in m_heads)
        {
            if (head.IsMyCollider(targetCollider))
                return head;
        }
        
        return null;
    }

    private void SetTargetedHead(HydraHead head)
    {
        HydraDamageBehaviour damageable = m_damageable as HydraDamageBehaviour;
        damageable.SetTargetedHead(head);
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
        foreach (var head in m_heads)
        {
            head.DetermineIntention();
        }
    }

    public override void ShowIntention()
    {
    }

    public override void ExecuteAction(Action finishCallback)
    {
        // play intention
        base.ExecuteAction(finishCallback);

        // Debug.Log("this action is played: " + m_nextMove.clientID);
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
        List<HydraHead> aliveHead = new List<HydraHead>();
        aliveHead.AddRange(m_heads); // m_heads might change mid loop

        foreach (var head in aliveHead)
        {
            SetTargetedHead(head);
            bool animDone = false;
            StartCoroutine(head.ExecuteIntention(() => animDone = true));
            yield return new WaitUntil(() => animDone);
        }

        finishCallback?.Invoke();
        yield return null;
    }

    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(1);
        m_fighterHP.ResetHP();
    }

    public int GetMissingHeadCount()
    {
        return m_deadHeads.Count;
    }
}