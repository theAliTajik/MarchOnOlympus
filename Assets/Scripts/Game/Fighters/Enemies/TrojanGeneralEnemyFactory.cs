using UnityEngine;

public class TrojanGeneralEnemyFactory : MonoBehaviour, IEnemyFactory
{
    public string FactoryID { get; } = "TrojanGeneral";


    public BaseEnemy SpawnEnemy(EnemiesDb.BossInfo bossInfo, Transform parent)
    {
        BaseEnemy enemy = Instantiate(bossInfo.Prefab, Vector3.zero, Quaternion.identity, parent);
        return enemy;
    }
    

    public void SetupEnemy(BaseEnemy enemy, Vector3? position = null, Quaternion? rotation = null)
    {
        TrojanGeneral general = enemy as TrojanGeneral;
        TrojanGeneralTower tower = enemy.GetComponentInChildren<TrojanGeneralTower>();
        
        SetupInternal(general, position, rotation);
        SetupInternal(tower);
        if (general != null)
        {
            general.SetTower(tower);
            general.SetFortify();
        }
    }

    private void SetupInternal(BaseEnemy enemy, Vector3? position = null, Quaternion? rotation = null)
    {
        if (enemy == null)
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
        MechanicsManager.Instance.CreateMechanicsList(enemy);
        CombatManager.Instance.SubscribeToEnemyDamage(enemy);
        EnemiesManager.Instance.AddEnemey(enemy);
    }
}