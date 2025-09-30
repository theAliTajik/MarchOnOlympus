using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CannibalsHelper
{
    private static List<Cannibal> m_cannibals = new List<Cannibal>();

    public static void AddCannibal(Cannibal cannibal)
    {
        m_cannibals.Add(cannibal);
        cannibal.Death += OnCannibalDeath;
    }

    private static void OnCannibalDeath(Fighter cannibal)
    {
        Cannibal deadCannibal = (Cannibal)cannibal;
        m_cannibals.Remove(deadCannibal);

        if (m_cannibals.Count > 0)
        {
            Cannibal randCannibal = m_cannibals[Random.Range(0, m_cannibals.Count)];
            randCannibal.LevelUp(deadCannibal.Level, () =>
            {
                EnemiesManager.Instance.RemoveDeadEnemyBody(deadCannibal);
            });
        }
    }
    
    
}
