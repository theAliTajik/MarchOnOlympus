
using UnityEngine;

public class HydraEnemyFactory : MonoBehaviour, IEnemyFactory
{
    public string FactoryID { get; } = "Hydra";
    
    public BaseEnemy SpawnEnemy(EnemiesDb.BossInfo bossInfo, Transform parent)
    {
        BaseEnemy enemy = Instantiate(bossInfo.Prefab, Vector3.zero, Quaternion.identity, parent);
        return enemy;
    }

    public void SetupEnemy(BaseEnemy enemy, Vector3? position = null, Quaternion? rotation = null)
    {
        if (enemy is not Hydra mind)
        {
            Debug.Log("ERROR: enemy was either null or not of type Hydra");
            return;
        }
        
        SetupMind(enemy, position, rotation);
        var config = mind.HeadConfig;

        var iterator = mind.GetEnumerator();
        while (iterator.MoveNext())
        {
            SetupHead(iterator.Current, config, null, null);
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
            
            
                
        MechanicsList list = MechanicsManager.Instance.CreateMechanicsList(enemy);
        HUD.Instance.SpawnMechanicsDisplay(enemy, list);
        CombatManager.Instance.SubscribeToEnemyDamage(enemy);
        EnemiesManager.Instance.AddEnemey(enemy);
    }

    private void SetupHead(HydraHead head, HydraHeadConfigData data, Vector3? position = null, Quaternion? rotation = null)
    {
        if (head == null)
        {
            Debug.Log("null head sent to setup");
            return;
        }
        
        head.Config(data);

        HUD.Instance.SpawnEnemyIntentionWidget(head, head.GetHeadPosition());
        HUD.Instance.SpawnHPBar(head);
    }
}
