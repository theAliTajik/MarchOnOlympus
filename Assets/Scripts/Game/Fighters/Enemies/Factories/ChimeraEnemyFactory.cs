
using UnityEngine;

public class ChimeraEnemyFactory : MonoBehaviour, IEnemyFactory
{
    public string FactoryID { get; } = "Chimera";
    
    
    public BaseEnemy SpawnEnemy(EnemiesDb.BossInfo bossInfo, Transform parent)
    {
        BaseEnemy enemy = Instantiate(bossInfo.Prefab, Vector3.zero, Quaternion.identity, parent);
        return enemy;
    }
        
    
    public void SetupEnemy(BaseEnemy enemy, Vector3? position = null, Quaternion? rotation = null)
    {
        if (enemy is not Chimera mind)
        {
            Debug.Log("ERROR: enemy was either null or not of type chimera");
            return;
        }

        IHaveIntention lion = mind.Lion;
        IHaveIntention serpent = mind.Serpent;
        IHaveIntention goat = mind.Goat;
        
        SetupMind(mind, position, rotation);
        SetupHead(mind, lion, mind.LionPosition.position);
        SetupHead(mind, serpent, mind.SerpentPosition.position);
        SetupHead(mind, goat, mind.GoatPosition.position);

    }
    
    private void SetupMind(BaseEnemy enemy, Vector3? position = null, Quaternion? rotation = null)
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
        MechanicsManager.Instance.CreateMechanicsList(enemy);
        CombatManager.Instance.SubscribeToEnemyDamage(enemy);
        EnemiesManager.Instance.AddEnemey(enemy);
    }

    private void SetupHead(BaseEnemy mind, IHaveIntention headIntention, Vector3 position)
    {
        if (mind == null)
        {
            Debug.Log("null mind sent to setup");
            return;
        }
        
        if (headIntention == null)
        {
            Debug.Log("null head sent to setup");
            return;
        }
                    
        HUD.Instance.SpawnEnemyIntentionWidget(headIntention, position);
    }
}
