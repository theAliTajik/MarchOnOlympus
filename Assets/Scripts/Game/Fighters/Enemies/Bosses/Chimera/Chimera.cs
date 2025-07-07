using System;
using Game;
using System.Collections;
using System.Collections.Generic;
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


    [SerializeField] private Transform m_lionPosition;
    [SerializeField] private Transform m_serpentPosition;
    [SerializeField] private Transform m_goatPosition;

    private Dictionary<Type, Transform> m_HeadPositions = new Dictionary<Type, Transform>();
    
    public List<ChimeraHead> Heads => m_heads;
    public Dictionary<Type, Transform> HeadPositions => m_HeadPositions;
    
    
    private ChimeraHead m_TargetedHead;

    public Chimera()
    {
        m_lion = new ChimeraLion(this);
        m_serpent = new ChimeraSerpent(this);
        m_goat = new ChimeraGoat(this);
        
        m_heads.Add(m_lion);
        m_heads.Add(m_serpent);
        m_heads.Add(m_goat);
    }
    
    protected override void Awake()
    {
        base.Awake();
        
        m_HeadPositions = new Dictionary<Type, Transform>()
        {
            { typeof(ChimeraLion), m_lionPosition },
            { typeof(ChimeraSerpent), m_serpentPosition },
            { typeof(ChimeraGoat), m_goatPosition },
        };
        
        ConfigFighterHP();

        foreach (var head in m_heads)
        {
            head.Config();
        }
        
        GameplayEvents.ColliderSelected += OnColliderSelected;
        GameplayEvents.GamePhaseChanged += OnPhaseChange;
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
        
        m_TargetedHead = headHit;
        Debug.Log($"set head to: {headHit.GetType()}");
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(ANIM_02_WOUND);
        if (m_TargetedHead != null)
        {
            m_TargetedHead.TakeDamage(damage);
            m_TargetedHead = null;
        }
        else
        {
            Debug.Log("null target head");
        }

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
            head.ExecuteIntention(() => headFinished = true);
            yield return new WaitUntil(() => headFinished);
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