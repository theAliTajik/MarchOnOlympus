using UnityEngine;

public class ScyllaEnemyFactory : MonoBehaviour, IEnemyFactory
{
    public string FactoryID { get; } = "Scylla";
    
    public BaseEnemy SpawnEnemy(EnemiesDb.BossInfo bossInfo, Transform parent)
    {
        BaseEnemy enemy = Instantiate(bossInfo.Prefab, Vector3.zero, Quaternion.identity, parent);
        return enemy;
    }

    public void SetupEnemy(BaseEnemy enemy, Vector3? position = null, Quaternion? rotation = null)
    {
        if (enemy is not Scylla mind)
        {
            Debug.Log("ERROR: enemy was either null or not of type Scylla");
            return;
        }
        
        SetupMind(enemy, position, rotation);

        var iterator = mind.GetTentacles();
        while (iterator.MoveNext())
        {
            SetupHead(mind, iterator.Current, null, null);
        }
    }
    
    private void SetupMind(BaseEnemy enemy, Vector3? position = null, Quaternion? rotation = null)
    {
        if (!enemy)
        {
            Debug.Log("null enemy sent to setup");
            return;
        }
            
        if (position != null)
        {
            enemy.transform.position = position.Value;
        }
            
            
                
        HUD.Instance.SpawnHPBar(enemy);
        HUD.Instance.SpawnDamageIndicator(enemy);
        HUD.Instance.SpawnEnemyIntentionWidget(enemy);
        MechanicsList list = MechanicsManager.Instance.CreateMechanicsList(enemy);
        HUD.Instance.SpawnMechanicsDisplay(enemy, list);
        CombatManager.Instance.SubscribeToEnemyDamage(enemy);
        EnemiesManager.Instance.AddEnemey(enemy);
    }

    private void SetupHead(Scylla mind, ScyllaTentacle head, Vector3? position = null, Quaternion? rotation = null)
    {
        if (!head)
        {
            Debug.Log("null head sent to setup");
            return;
        }
        
        head.Config(mind);

        HUD.Instance.SpawnEnemyIntentionWidget(head, head.GetHeadPosition());
        HUD.Instance.SpawnHPBar(head);
        HUD.Instance.SpawnDamageIndicator(head);
    }
}
