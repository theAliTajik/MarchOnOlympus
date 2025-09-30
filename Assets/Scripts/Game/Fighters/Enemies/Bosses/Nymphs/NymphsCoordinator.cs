using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NymphsCoordinator
{
    private static List<BaseNymph> m_nymphsAlive = new List<BaseNymph>();
    private const int m_standardNumberOfNymphs = 4;
    
    private static ITurnCounter m_turnCounter;

    private static readonly DebugCategory cat = Categories.Fighters.Enemies.Nymphs;

    public static void Reset()
    {
        CustomDebug.Log("Reset", cat);
        m_nymphsAlive.Clear();
    }
    
    public static void RegisterNymph(BaseNymph nymph)
    {
        CustomDebug.Log($"Nymph registered: {nymph.GetType()}", cat);
        m_nymphsAlive.Add(nymph);
        nymph.Death += OnNymphDied;
            
        if (m_nymphsAlive.Count >= m_standardNumberOfNymphs)
        {
            EnemiesManager.Instance.StartCoroutine(DoInitialCoordination());
        }
    }

    public static IEnumerator DoInitialCoordination()
    {
        CustomDebug.Log("Initial Coordination", cat);
        yield return new WaitForSeconds(1);
        BaseNymph frontNymph = GetFrontmostNymph();
        
        frontNymph.SetDoesCastTwice(true);
        m_turnCounter = new StandardEnemyTurnCounter();
        GameplayEvents.GamePhaseChanged += OnPhaseChanged;
        m_turnCounter.NextTurn();
        int turn = m_turnCounter.GetRelativeTurn();
        AlternateNymphsTargetability(turn);
    }

    private static void OnPhaseChanged(EGamePhase phase)
    {
        switch (phase)
        {
            case EGamePhase.PLAYER_TURN_START:
                m_turnCounter.NextTurn();
                int turn = m_turnCounter.GetRelativeTurn();
                AlternateNymphsTargetability(turn);
                break;
            case EGamePhase.COMBAT_END:
                CustomDebug.Log("Combat end for nymphs coordinator", cat);
                GameplayEvents.GamePhaseChanged -= OnPhaseChanged;
                break;
        }
        
    }

    private static void AlternateNymphsTargetability(int turn)
    {
        bool isOdd = turn % 2 != 0;
        CustomDebug.Log($"Alternated nymphs the turn is: {turn}, isOdd: {isOdd}", cat);
        List<BaseNymph> oddNymps = GetOddNymphs();
        List<BaseNymph> evenNymphs = GetEvenNymphs();

        SetNymphsTargetability(oddNymps, isOdd);
        SetNymphsTargetability(evenNymphs, !isOdd);
    }

    private static void SetNymphsTargetability(List<BaseNymph> nymphs, bool isTarget)
    {
        foreach (var nymph in nymphs)
        {
            nymph.SetCanBeTarget(isTarget);
        }
    }

    private static List<BaseNymph> GetOddNymphs()
    {
        List<BaseNymph> oddNymphs = new List<BaseNymph>();

        foreach (var nymph in m_nymphsAlive)
        {
            if (nymph is Nymphs_1)
            {
                oddNymphs.Add(nymph);
            }
            
            if (nymph is Nymphs_3)
            {
                oddNymphs.Add(nymph);
            }
        }
        
        return oddNymphs;
    }
    
    private static List<BaseNymph> GetEvenNymphs()
    {
        List<BaseNymph> evenNymphs = new List<BaseNymph>();

        foreach (var nymph in m_nymphsAlive)
        {
            if (nymph is Nymphs_2)
            {
                evenNymphs.Add(nymph);
            }
            
            if (nymph is Nymphs_4)
            {
                evenNymphs.Add(nymph);
            }
        }
        
        return evenNymphs;
    }

    private static void OnNymphDied(Fighter fighter)
    {
        BaseNymph nymph = fighter as BaseNymph;
        CustomDebug.Log($"Nymph Died: {nymph.GetType()}", cat);
        if (nymph == null)
        {
            CustomDebug.LogError("Null nymph passed on nymph died", cat);
            return;
        }
        
        m_nymphsAlive.Remove(nymph);

        if (m_nymphsAlive.Count == 0)
        {
            return;
        }
        
        BaseNymph frontNymph = GetFrontmostNymph();
        frontNymph.SetDoesCastTwice(true);

        CustomDebug.Log($"Nymphs alive {m_nymphsAlive.Count}", cat);
        if (m_nymphsAlive.Count == 2)
        {
            SetAllNymphsToCastTwice();
        }

        if (m_nymphsAlive.Count == 1)
        {
            CustomDebug.Log("Only one nymph remains", cat);
            m_nymphsAlive[0].SetDoesCastTwice(false);
            m_nymphsAlive[0].SetLastNymphAlive(true);
        }
    }

    private static void SetAllNymphsToCastTwice()
    {
        CustomDebug.Log("Set all nymphs to cast twice", cat);
        foreach (var nymph in m_nymphsAlive)
        {
            nymph.SetDoesCastTwice(true);
        }
    }

    private static BaseNymph GetFrontmostNymph()
    {
        BaseNymph frontNymph = EnemiesPositionManager.Instance.FindFrontmostEnemyOfType(typeof(BaseNymph)) as BaseNymph;

        if (frontNymph == null)
        {
            Debug.Log("ERROR: failed to find frontmost nymph");
            return null;
        }

        CustomDebug.Log($"Frontmost nymph was: {frontNymph.GetType()}", cat);

        return frontNymph;
    }

}
