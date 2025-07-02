using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemiesDb", menuName = "Bosses/Enemies Database")]
public class EnemiesDb : GenericData<EnemiesDb>
{
    [System.Serializable]
    public class EnemyInfo
    {
        public string clientID;
        public BossInfo[] bosses;
    }
    
    [System.Serializable]
    public struct BossInfo
    {
        public BaseEnemy Prefab;
        public string Factory;
    }

    [FormerlySerializedAs("allNewEnemies")] public EnemyInfo[] allEnemies;
    
    public EnemyInfo[] PlayableEnemies;
    
#if UNITY_EDITOR
    [ContextMenu("Generate Boss Info")]
    private void GenerateBossInfo()
    {

    }
#endif
    
    public EnemyInfo FindById(string clientId)
    {
        for (int i = 0; i < allEnemies.Length; i++)
        {
            EnemyInfo enemy = allEnemies[i];
            if (enemy.clientID == clientId)
            {
                return enemy;
            }
        }

        return null;
    }
    
}
