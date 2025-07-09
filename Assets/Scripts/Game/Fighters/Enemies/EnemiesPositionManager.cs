
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesPositionManager : Singleton<EnemiesPositionManager>
{
    protected override void Init()
    {
    }
    
    
    [Serializable]
    public class SpawnPoint
    {
        public Transform Point;
        [NonSerialized] public List<BaseEnemy> Occupant = new List<BaseEnemy>();
    }
    
    [SerializeField] private List<SpawnPoint> m_spawnPoints = new List<SpawnPoint>();
    
    private int m_currentSpawnPoint = -1;


    public Vector3 AddOccupantToNextSpawnPoint(BaseEnemy enemy)
    {
        SpawnPoint sp = GetNextSpawnPoint();
        sp.Occupant.Add(enemy);
        
        return sp.Point.position;
    }
    
    private SpawnPoint GetNextSpawnPoint()
    {
        SpawnPoint emptySpawnPoint = null;
        foreach (SpawnPoint point in m_spawnPoints)
        {
            if (point.Occupant == null || point.Occupant.Count == 0)
            {
                emptySpawnPoint = point;
                break;
            }
        }

        if (emptySpawnPoint == null)
        {
            emptySpawnPoint = GetLeastCroudedSpawnPoint();
        }

        return emptySpawnPoint;
    }

    private SpawnPoint GetLeastCroudedSpawnPoint()
    {
        SpawnPoint leastCroudedSpawnPoint = m_spawnPoints[0];
        foreach (SpawnPoint point in m_spawnPoints)
        {
            if (point.Occupant == null || point.Occupant.Count < leastCroudedSpawnPoint.Occupant.Count)
            {
                leastCroudedSpawnPoint = point;
                break;
            }
        }
        
        return leastCroudedSpawnPoint;
    }

    public void OccupySpawnPoint(Transform spawnPoint, BaseEnemy enemy)
    {
        SpawnPoint keyPoint = m_spawnPoints.Find(x => x.Point == spawnPoint);

        if (keyPoint == null)
        {
            Debug.Log("did not find spawn point to occupy");
            return;
        }
        
        keyPoint.Occupant.Add(enemy);
    }

    public void RemoveEnemyFromSpawnPoint(Fighter enemy)
    {
        SpawnPoint keyPoint = FindSpawnPointOfEnemy(enemy);
        keyPoint.Occupant.Remove((BaseEnemy)enemy);
    }

    public SpawnPoint FindSpawnPointOfEnemy(Fighter enemy)
    {
        SpawnPoint keyPoint = null;
        foreach (SpawnPoint point in m_spawnPoints)
        {
            foreach (BaseEnemy occupant in point.Occupant)
            {
                if (occupant == enemy)
                {
                    keyPoint = point;
                    break;
                }
            }

            if (keyPoint != null)
            {
                break;
            }
        }

        return keyPoint;
    }

    public BaseEnemy FindFrontmostEnemy()
    {
        BaseEnemy frontmost = null;
        float lowestX = float.MaxValue;

        foreach (var point in m_spawnPoints)
        {
            foreach (var enemy in point.Occupant)
            {
                if (enemy.transform.position.x < lowestX)
                {
                    lowestX = enemy.transform.position.x;
                    frontmost = enemy;
                }
            }
        }

        return frontmost;
    }

    public BaseEnemy FindFrontmostEnemyOfType(Type t, bool isAlive = true)
    {
        if (t == null)
        {
            return null;
        }
        
        BaseEnemy frontmost = null;
        float lowestX = float.MaxValue;

        foreach (var point in m_spawnPoints)
        {
            foreach (var enemy in point.Occupant)
            {
                if (!t.IsInstanceOfType(enemy))
                {
                    continue;
                }

                if (enemy.HP.Current <= 0)
                {
                    continue;
                }
                
                if (enemy.transform.position.x < lowestX)
                {
                    lowestX = enemy.transform.position.x;
                    frontmost = enemy;
                }
            }
        }

        return frontmost;
    }
}
