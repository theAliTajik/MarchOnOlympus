using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public interface IEnemyFactory
{
    public string FactoryID { get; }
    
    public BaseEnemy SpawnEnemy(EnemiesDb.BossInfo bossInfo, Transform parent);
    public void SetupEnemy(BaseEnemy enemy, Vector3? position = null, Quaternion? rotation = null);
}

public class BaseEnemyFactory : MonoBehaviour, IEnemyFactory
{
    public virtual string FactoryID { get; } = "Default";

    public virtual BaseEnemy SpawnEnemy(EnemiesDb.BossInfo bossInfo, Transform parent)
    {
        BaseEnemy enemy = Instantiate(bossInfo.Prefab, Vector3.zero, Quaternion.identity, parent);

        return enemy;
    }

    public virtual void SetupEnemy(BaseEnemy enemy, Vector3? position = null, Quaternion? rotation = null)
    {
        if (position != null)
        {
            enemy.transform.position = position.Value - enemy.GetRootPosition();
        }
        
        
            
        HUD.Instance.SpawnHPBar(enemy);
        HUD.Instance.SpawnDamageIndicator(enemy);
        HUD.Instance.SpawnEnemyIntentionWidget(enemy);
        MechanicsList list = MechanicsManager.Instance.CreateMechanicsList(enemy);
        HUD.Instance.SpawnMechanicsDisplay(enemy, list);
        CombatManager.Instance.SubscribeToEnemyDamage(enemy);
        EnemiesManager.Instance.AddEnemey(enemy);
    }
}