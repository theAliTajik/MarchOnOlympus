using System;
using Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Chimera : BaseEnemy
{
	#region Animations

	private const string ANIM_01_IDLE = "01_Idle";
	private const string ANIM_02_WOUND = "02_Wound";
	private const string ANIM_03_IDLE_WOUND = "03_Idle_Wound";
	private const string ANIM_04_LION_HEAD_ATTACK = "04_Lion_head_Attack";
	private const string ANIM_05_LION_HEAD_GROW = "05_Lion_head_Grow";
	private const string ANIM_06_GOAT_HEAD_SHOUT_CRIT = "06_Goat_Head_Shout_Crit";
	private const string ANIM_06_GOAT_HEAD_SHOUT_NORMAL = "06_Goat_Head_Shout_Normal";
	private const string ANIM_08_SERPENT_HEAD = "08_Serpent_Head";
	private const string ANIM_09_DEATH_ = "09_Death_";

	#endregion
    

    [SerializeField] private ChimeraMovesData m_data;

    private List<ChimeraHead> m_heads = new List<ChimeraHead>();

    [SerializeField] private ChimeraLion m_lion;
    [SerializeField] private ChimeraSerpent m_serpent;
    [SerializeField] private ChimeraGoat m_goat;

    private Dictionary<Type, Transform> m_HeadPositions = new Dictionary<Type, Transform>();
    
    public List<ChimeraHead> Heads => m_heads;
    public Dictionary<Type, Transform> HeadPositions => m_HeadPositions;
    
    
    private ChimeraHead m_TargetedHead;
    
    [SerializeField] private int m_poisonHitMultiplier;
    [SerializeField] private BaseCardData m_poisonCard;

    public Chimera()
    {
        m_lion = new ChimeraLion(this);
        m_serpent = new ChimeraSerpent(this);
        m_goat = new ChimeraGoat(this);
        
        m_heads.Add(m_lion);
        m_heads.Add(m_goat);
        m_heads.Add(m_serpent);
    }
    
    protected override void Awake()
    {
        base.Awake();
        
        ConfigFighterHP();

        m_damageable = new ChimeraDamageBehaviour();
        m_damageable.OnDamage += m_fighterHP.TakeDamage;
        
        foreach (var head in m_heads)
        {
            head.Config();
        }
        
        GameplayEvents.ColliderSelected += OnColliderSelected;
        GameplayEvents.GamePhaseChanged += OnPhaseChange;
        
        HP.SetTrigger(m_data.PoisonPercentageTrigger);
        HP.SetTrigger(m_data.TauntPercentageTrigger);

        HP.OnPercentageTrigger += OnHPPercentageTriggered;
    }

    private void OnHPPercentageTriggered(FighterHP.TriggerPercentage percent)
    {
        if (percent == m_data.PoisonPercentageTrigger)
        {
            Debug.Log("poison triggered");
            HitPoison();
        }

        if (percent == m_data.TauntPercentageTrigger)
        {
            Debug.Log("taunt triggered");
            m_animation.Play(ANIM_05_LION_HEAD_GROW);
            TauntHeads();
        }
        
    }

    private void OnPhaseChange(EGamePhase phase)
    {
        switch (phase)
        {
            case EGamePhase.ENEMY_TURN_END:
                foreach (var head in m_heads)
                {
                    head.TurnEnded();
                }
                break;
            case EGamePhase.ENEMY_TURN_START:
                foreach (var head in m_heads)
                {
                    head.TurnStarted();
                }

                break;
        }
    }

    private void OnDestroy()
    {
        GameplayEvents.ColliderSelected -= OnColliderSelected;
    }

    private void OnColliderSelected(Collider2D targetCollider)
    {
        ChimeraHead headHit = MatchColliderToHead(targetCollider);
        if (headHit == null)
        {
            return;
        }
        
        ChimeraDamageBehaviour damageable = m_damageable as ChimeraDamageBehaviour;
        damageable.SetTargetedHead(headHit);
        m_TargetedHead = headHit;
        // Debug.Log($"set head to: {headHit.GetType()}");
    }
    
    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(ANIM_02_WOUND);
    }
    
    private ChimeraHead MatchColliderToHead(Collider2D targetCollider)
    {
        foreach (var head in m_heads)
        {
            if (head.IsMyCollider(targetCollider))
                return head;
        }
        
        return null;
    }

    private void HitPoison()
    {
        
        int numOfPoisonCards = GameInfoHelper.CountCardsWithName(m_poisonCard.name, CardStorage.ALL);
                
        if (numOfPoisonCards <= 0)
        {
            return;
        }

        int damage = numOfPoisonCards * m_poisonHitMultiplier;
        GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, damage);
    }

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_09_DEATH_);
    }

    public override void DetermineIntention()
    {
        foreach (var head in m_heads)
        {
            head.DetermineIntention();
        }
        ShowIntention();
    }

    public override void ShowIntention()
    {
        foreach (var head in m_heads)
        {
            head.ShowIntention();
        }
    }

    public override void ExecuteAction(Action finishCallback)
    {
        // play intention
        base.ExecuteAction(finishCallback);

        StartCoroutine(WaitAndExecute(finishCallback));
    }

    private IEnumerator WaitAndExecute(Action finishCallback)
    {
        foreach (var head in m_heads)
        {
            bool headFinished = false;
            bool animationFinished = true;
            
            string animName = head.GetAnimation();
            if (!string.IsNullOrEmpty(animName))
            {
                animationFinished = false;
                StartCoroutine(WaitForAnimation(animName, () => { animationFinished = true; }));
            }
            
            head.ExecuteIntention(() => headFinished = true);
            
            yield return new WaitUntil(() => headFinished);
            yield return new WaitUntil(() => animationFinished);
        }
        
        finishCallback?.Invoke();
    }

    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

    public void TauntHeads()
    {
        foreach (var head in m_heads)
        {
            head.ReceiveTaunt();
        }
    }
}