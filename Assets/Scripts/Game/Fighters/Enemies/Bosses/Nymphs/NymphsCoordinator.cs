using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NymphsCoordinator
{
    private static List<BaseNymph> m_nymphsAlive = new List<BaseNymph>();
    
    private const int m_standardNumberOfNymphs = 4;

    public static void Reset()
    {
        m_nymphsAlive.Clear();
    }
    
    public static void RegisterNymph(BaseNymph nymph)
    {
        // Debug.Log($"nymph registered {nymph.GetType()}");
        m_nymphsAlive.Add(nymph);
        nymph.Death += OnNymphDied;
            
        if (m_nymphsAlive.Count >= m_standardNumberOfNymphs)
        {
            EnemiesManager.Instance.StartCoroutine(DoInitialCoordination());
        }
    }

    public static IEnumerator DoInitialCoordination()
    {
        yield return new WaitForSeconds(1);
        BaseNymph frontNymph = GetFrontmostNymph();
        frontNymph.SetDoesCastTwice(true);
    }

    private static void OnNymphDied(Fighter fighter)
    {
        // Debug.Log("nymphDied");
        BaseNymph nymph = fighter as BaseNymph;
        if (nymph == null)
        {
            Debug.Log("ERROR: null nymph in on nymph died");
            return;
        }
        
        m_nymphsAlive.Remove(nymph);

        if (m_nymphsAlive.Count == 0)
        {
            return;
        }
        
        BaseNymph frontNymph = GetFrontmostNymph();
        frontNymph.SetDoesCastTwice(true);

        // Debug.Log($"nymphs alive {m_nymphsAlive.Count}");
        if (m_nymphsAlive.Count == 2)
        {
            // Debug.Log("nymphs equal 2");
            SetAllNymphsToCastTwice();
        }

        if (m_nymphsAlive.Count == 1)
        {
            // Debug.Log("only one nymph remains");
            m_nymphsAlive[0].SetDoesCastTwice(false);
            m_nymphsAlive[0].SetLastNymphAlive(true);
        }
    }

    private static void SetAllNymphsToCastTwice()
    {
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

        // Debug.Log($"frontmost nymph was: {frontNymph.GetType()}");

        return frontNymph;
    }

}
