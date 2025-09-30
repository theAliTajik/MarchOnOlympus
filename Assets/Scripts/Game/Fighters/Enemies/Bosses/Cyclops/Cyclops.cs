using System;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Cyclops : BaseEnemy
{
    private static readonly int s_HasClub = Animator.StringToHash(m_hasClubBoolID);

    #region Animations

    private const string ANIM_1 = "1";
    private const string ANIM_001_DANCE = "001_Dance";
    private const string ANIM_ATTACK = "Attack";
    private const string ANIM_ATTACK2 = "Attack2";
    private const string ANIM_ATTACK3 = "Attack3";
    private const string ANIM_ATTACK_4_1 = "Attack_4_1";
    private const string ANIM_ATTACK_4_2 = "Attack_4_2";
    private const string ANIM_DEATH = "Death";
    private const string ANIM_IDLE = "Idle";
    private const string ANIM_IDLE2 = "Idle2";
    private const string ANIM_WOUNDED1 = "Wounded1";
    private const string ANIM_WOUNDED2 = "Wounded2";
    private const string ANIM_WOUNDED3 = "Wounded3";
    private const string ANIM_WOUNDED4 = "Wounded4";    
    
	#endregion

	[SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] protected MoveData m_fourthTurnMoveData;
    [SerializeField] protected MoveData m_gettingClubMoveData;
    [SerializeField] private CyclopsMovesData m_data;
    [SerializeField] private CyclopsClub m_club;
    [SerializeField] private CyclopsEye m_eye;


    private List<IDamageable> m_damageableParts = new List<IDamageable>();
    
    private ITurnCounter m_turnCounter;
    private IDamageable m_TargetedPart;
    
    public CyclopsClub Club => m_club;
    public CyclopsEye Eye => m_eye;

    private Animator m_unityAnimator;
    private const string m_hasClubBoolID = "HasClub";

    protected override void Awake()
    {
        base.Awake();
        
        m_unityAnimator = GetComponent<Animator>();

        SetMoves(m_movesDatas);
        m_damageable = new CyclopsDamageBehaviour(this);
        m_damageable.OnDamage += m_fighterHP.TakeDamage;
        
        m_turnCounter = new CyclicalEnemyTurnCounter(4);
        // m_club = new CyclopsClub(m_data.ClubHP); m_eye = new CyclopsEye(m_data.EyeHP);
        
        MechanicsManager.Instance.CreateMechanicsList(m_club);
        MechanicsManager.Instance.CreateMechanicsList(m_eye);
        
        m_club.Config();
        m_eye.Config();
        
        m_damageableParts.Add(m_club);
        m_damageableParts.Add(m_eye);
        
        m_club.OnDeath += OnClubDeath;
        m_club.OnRevive += OnClubRevive;
        m_eye.OnDeath += OnEyeDeath;
        

        m_club.OnDamage += OnClubDamaged;
        m_eye.OnDamage += OnEyeDamaged;
        
        GameplayEvents.ColliderSelected += OnColliderSelected;
        
        ConfigFighterHP();
    }

    private void OnClubDamaged(int damage)
    {
        m_animation.Play(ANIM_WOUNDED2);
    }

    private void OnEyeDamaged(int damage)
    {
        m_animation.Play(ANIM_WOUNDED3);
    }

    private void OnDestroy()
    {
        GameplayEvents.ColliderSelected -= OnColliderSelected;
    }

    private void OnColliderSelected(Collider2D targetCollider)
    {
        IDamageable partHit = MatchColliderToHead(targetCollider);
        
        CyclopsDamageBehaviour damageable = m_damageable as CyclopsDamageBehaviour;
        damageable.SetTargetedHead(partHit);
        m_TargetedPart = partHit;
        // Debug.Log($"set head to: {headHit.GetType()}");
    }

    IDamageable MatchColliderToHead(Collider2D collider)
    {
        foreach (var part in m_damageableParts)
        {
            if (part is not IColliderMatcher partColliderMatcher)
            {
                continue;
            }
            
            bool matched = partColliderMatcher.IsMyCollider(collider);

            if (matched)
            {
                return part;
            }
        }
        
        return null;
    }

    private void OnEyeDeath()
    {
        m_fighterHP.Kill();
    }

    private void OnClubDeath()
    {
        Debug.Log("cyclops registered club dead");
        SetNextMoveToGettingClub();
        SetEyeState(isActive:true);
        SetAnimationState(hasClub:false);
        ShowIntention();
    }

    private void OnClubRevive()
    {
        SetAnimationState(hasClub:true);
    }

    private void SetAnimationState(bool hasClub)
    {
        m_unityAnimator.SetBool(s_HasClub,hasClub);
    }

    private void SetNextMoveToGettingClub()
    {
        m_nextMove = m_gettingClubMoveData;
    }

    private void SetEyeState(bool isActive)
    {
        m_eye.SetState(isActive);
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(GetCorrectWoundClip());
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
        m_turnCounter.NextTurn();
        if (m_turnCounter.GetRelativeTurn() == 4)
        {
            m_nextMove = m_fourthTurnMoveData;
            ShowIntention();
            return;
        }
        
        
        RandomIntentionPicker();
        ShowIntention();
    }

    
    public override void ShowIntention()
    {
        base.ShowIntention();
        switch (m_nextMove.clientID)
        {
            case "HitBuff":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "Hit":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "RemoveAllDebuffs":
                CallOnIntentionDetermined(Intention.BUFF, m_nextMove.description);
                break;
            case "getClub":
                CallOnIntentionDetermined(Intention.BUFF, m_nextMove.description);
                break;
            
            default:
                Debug.Log($"invalid next move client id:{m_nextMove.clientID} for showing intention");
                break;
        }
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

		Fighter player = GameInfoHelper.GetPlayer();
        
        if (m_club.isAlive())
        {
            Debug.Log("Club Alive: Added str");
            GameActionHelper.AddMechanicToFighter(this, m_data.StrGainIfClubAlive, MechanicType.STRENGTH);
        }

        switch (m_nextMove.clientID)
        {
            case "HitBuff":
                //Hit
                for (int i = 1; i <= m_data.Move1NumOfAttacks; i++)
                {
                    yield return WaitForAnimation(GetCorrectAttackClip());
                    GameActionHelper.DamageFighter(player, this, m_data.Move1Damage);
                }

                //Remove all buffs on player
                GameActionHelper.RemoveAllMechanicOfCategory(player, MechanicCategory.BUFF);

                finishCallback?.Invoke();
                break;
            case "Hit":
                for (int i = 1; i <= m_data.Move2NumOfAttacks; i++)
                {
                    yield return WaitForAnimation(GetCorrectAttackClip());
                    GameActionHelper.DamageFighter(player, this, m_data.Move2Damage);
                }
                finishCallback?.Invoke();
                break;
            case "RemoveAllDebuffs":
                //Every 4th turn, removes all debuffs on (This)
                m_animation.Play(ANIM_ATTACK_4_2, finishCallback);
                GameActionHelper.RemoveAllMechanicOfCategory(this, MechanicCategory.DEBUFF);
                
                //Fortify
                GameActionHelper.AddMechanicToFighter(this, m_data.Move3Fortify, MechanicType.FORTIFIED);
                break;
            
            case "getClub":
                SetEyeState(false);
                GetClub();
                Debug.Log("got new club");
                finishCallback?.Invoke();
                break;
        }

        yield return null;
    }

    private void GetClub()
    {
        m_club.Revive();
    }

    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

    private string GetCorrectAttackClip()
    {
        int rand = UnityEngine.Random.Range(0, 2);
        if (m_club.isAlive())
        {
            if (rand == 0)
            {
                return ANIM_ATTACK2;
            }

            return ANIM_ATTACK3;
        }

        if (rand == 0)
        {
            return ANIM_ATTACK_4_1;
        }

        return ANIM_ATTACK_4_2;
    }

    private string GetCorrectWoundClip()
    {
        bool hasClub = Club.isAlive();
        bool wasDamageToBody = m_TargetedPart == null;

        FighterHP clubHp = Club.GetHP();
        bool isClubDamaged = clubHp.Current < clubHp.Max * 0.5f;
        
        if (!hasClub)
        {
            return ANIM_WOUNDED4;
        }
        
        if (wasDamageToBody)
        {
            return ANIM_WOUNDED3;
        }

        if (isClubDamaged)
        {
            return ANIM_WOUNDED2;
        }

        return ANIM_WOUNDED1;
    }

}