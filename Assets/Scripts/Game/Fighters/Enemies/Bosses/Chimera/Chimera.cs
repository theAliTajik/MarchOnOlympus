using System;
using Game;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

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

    [SerializeField] private ChimeraLion m_lion;
    private ChimeraSerpent m_serpent = new ChimeraSerpent();
    private ChimeraGoat m_goat = new ChimeraGoat();
    
    [SerializeField] private Transform m_lionPosition;
    [SerializeField] private Transform m_serpentPosition;
    [SerializeField] private Transform m_goatPosition;

    public Chimera()
    {
        m_lion = new ChimeraLion(this);
    }


    public ChimeraLion Lion => m_lion;
    public ChimeraSerpent Serpent => m_serpent;
    public ChimeraGoat Goat => m_goat;
    
    public Transform LionPosition => m_lionPosition;
    public Transform SerpentPosition => m_serpentPosition;
    public Transform GoatPosition => m_goatPosition;

    protected override void Awake()
    {
        base.Awake();

        
        ConfigFighterHP();

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
        m_lion.DetermineIntention();
        ShowIntention();
    }

    public override void ShowIntention()
    {
        m_lion.ShowIntention();
    }

    public override void ExecuteAction(Action finishCallback)
    {
        // play intention
        base.ExecuteAction(finishCallback);

        StartCoroutine(WaitAndExecute(finishCallback));
    }

    private IEnumerator WaitAndExecute(Action finishCallback)
    {
        bool headFinished = false;
        m_lion.ExecuteIntention(() => headFinished = true);
        yield return new WaitUntil(() => headFinished);
        
        
        finishCallback?.Invoke();
    }

    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }
}