
using System;
using Game;
using Mono.CSharp;
using UnityEngine;

public class AkhilleusController : PlayerController
{
    #region Animations

    private const string ANIM_ATTACK_1 = "Attack_1";
    private const string ANIM_ATTACK_2 = "Attack_2";
    private const string ANIM_ATTACK_3 = "Attack_3";
    private const string ANIM_BLOCK_IDLE = "Block_Idle";
    private const string ANIM_CRITICAL_HIT = "Critical_Hit";
    private const string ANIM_DEATH = "Death";
    private const string ANIM_IDLE = "Idle";
    private const string ANIM_IDLE2 = "Idle2";
    private const string ANIM_IDLE_WOUNDED = "Idle_Wounded";
    private const string ANIM_WOUNDED = "Wounded";    
    
    #endregion

    private Animator m_unityAnimator;
    
    [Header("Sounds")]
    [SerializeField] private SoundEffectSO m_blockSound;
    [SerializeField] private SoundEffectSO m_criticalHitSound;
    [SerializeField] private SoundEffectSO m_march1;
    [SerializeField] private SoundEffectSO m_shieldBashSound;
    [SerializeField] private SoundEffectSO m_shout1;
    [SerializeField] private SoundEffectSO m_spear1;
    [SerializeField] private SoundEffectSO m_spearAttackBleed;
    [SerializeField] private SoundEffectSO m_spearMultiAttack;

    private const string m_bashCardName = "Shield Bash";
    private const string m_blockStrikeCardName = "Block";

    protected override void Start()
    {
        base.Start();
        if (m_mechanicsList == null)
        {
            CustomDebug.LogWarning("Mechanic list is null", Categories.Fighters.Player.Root);
            return;
        }
        
        m_mechanicsList.OnMechanicUpdated += OnMechanicAdded;
        m_mechanicsList.OnMechanicRemoved += OnMechanicRemoved;
        GameplayEvents.OnCardPlayed += OnCardPlayed;
    }

    private void OnCardPlayed(CardDisplay card)
    {
        if(card == null) return;
        var cardType = card.CardInDeck.GetCardType();
        var cardName = card.CardInDeck.GetCardName();

        if (cardType == CardType.TECH)
        {
            SoundEffectsEventBus.SendPlay(m_march1);
            return;
        }

        if (string.Equals(cardName?.Trim(), m_bashCardName, StringComparison.OrdinalIgnoreCase))
        {
            SoundEffectsEventBus.SendPlay(m_shieldBashSound);
        }
        
        if (string.Equals(cardName?.Trim(), m_blockStrikeCardName, StringComparison.OrdinalIgnoreCase))
        {
            SoundEffectsEventBus.SendPlay(m_shieldBashSound);
        }
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        base.OnTookDamage(damage, isCritical);
        m_animator.Play(ANIM_WOUNDED);
    }

    private bool m_blockSoundPlayed;
    private void OnMechanicAdded(MechanicType type)
    {
        if(m_blockSoundPlayed) return;
        if(type != MechanicType.BLOCK) return;
        
        m_blockSoundPlayed = true;
        string animName = m_animator.GetCurrentStateName();
        CustomDebug.Log($"current anim name when block added: {animName}. idle anim name: {". " + ANIM_IDLE}", Categories.Fighters.Player.Root, DebugTag.ANIMATION);

        if (animName == ". " + ANIM_IDLE.ToLower())
        {
            m_animator.Play(ANIM_BLOCK_IDLE);
        }
        SoundEffectsEventBus.SendPlay(m_blockSound);
    }

    private void OnMechanicRemoved(MechanicType type)
    {
        if(type != MechanicType.BLOCK) return;
        m_blockSoundPlayed = false;
    }

    public override float PlayAttackAnimation(Action finishCallBack)
    {
        int rand = UnityEngine.Random.Range(0, 3);
        CustomDebug.Log($"Asked To Play Animation {rand}", Categories.Fighters.Player.Root, DebugTag.ANIMATION);
        if (GameInfoHelper.CheckIfLastCardPlayedWas("Dual Strike", true) || GameInfoHelper.CheckIfLastCardPlayedWas("Obliterate", true) && CombatManager.Instance.CurrentStance == Stance.BERSERKER)
        {
            // m_unityAnimator.SetTrigger(PARAM_ATTACK3_T);
            m_animator.Play(ANIM_ATTACK_3, finishCallBack);
            SoundEffectsEventBus.SendPlay(m_spearMultiAttack);
            return 2f;
        }
        switch (rand)
        {
            case 0:
                // m_unityAnimator.SetTrigger(PARAM_ATTACK1_T);
                m_animator.Play(ANIM_ATTACK_1, finishCallBack);
                SoundEffectsEventBus.SendPlay(m_spear1);
                return 1.3f;
                break;
            case 1:
                // m_unityAnimator.SetTrigger(PARAM_ATTACK2_T);
                m_animator.Play(ANIM_ATTACK_2, finishCallBack);
                SoundEffectsEventBus.SendPlay(m_spearAttackBleed);
                return 1.3f;
                break;
            case 3:
                // m_unityAnimator.SetTrigger(PARAM_ATTACKCRIT_T);
                m_animator.Play(ANIM_CRITICAL_HIT, finishCallBack);
                SoundEffectsEventBus.SendPlay(m_criticalHitSound);
                return 1.6f;
                break;
        }
        return 0.5f;
    }
    
    protected override void OnDeath()
    {
        base.OnDeath();
        m_animator.Play(ANIM_DEATH);
    }
}
