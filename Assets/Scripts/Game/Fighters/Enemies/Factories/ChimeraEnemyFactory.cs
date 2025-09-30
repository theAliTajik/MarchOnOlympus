
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

        
        SetupMind(mind, position, rotation);

        foreach (var head in mind.Heads)
        {
            SetupHead(head);
        }
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
        MechanicsList list = MechanicsManager.Instance.CreateMechanicsList(enemy);
        HUD.Instance.SpawnMechanicsDisplay(enemy, list);
        CombatManager.Instance.SubscribeToEnemyDamage(enemy);
        EnemiesManager.Instance.AddEnemey(enemy);
    }

    private void SetupHead(IHaveIntention headIntention)
    {
        if (headIntention == null)
        {
            Debug.Log("null head sent to setup");
            return;
        }
                    
        HUD.Instance.SpawnEnemyIntentionWidget(headIntention, headIntention.GetHeadPosition());

        if (headIntention is IHaveMechanics owner)
        {
            MechanicsList list = MechanicsManager.Instance.CreateMechanicsList(owner);
            
            if (owner is ChimeraSerpent serpent)
            {
                HUD.Instance.SpawnMechanicsDisplay(serpent, list);
                return;
            }
        
        }
    }
}
