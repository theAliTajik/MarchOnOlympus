
using UnityEngine;

public class InvisibleEnemyFactory : MonoBehaviour, IEnemyFactory
{
    public string FactoryID { get; } = "Invisible";


    public BaseEnemy SpawnEnemy(EnemiesDb.BossInfo bossInfo, Transform parent)
    {
        BaseEnemy enemy = Instantiate(bossInfo.Prefab, Vector3.zero, Quaternion.identity, parent);
        return enemy;
    }
    

    public void SetupEnemy(BaseEnemy enemy, Vector3? position = null, Quaternion? rotation = null)
    {
         if (enemy == null)
         {
             Debug.Log("null enemy sent to setup");
             return;
         }
         
         if (position != null)
         {
             enemy.transform.position = position.Value - enemy.GetRootPosition();
         }
             
         EnemiesManager.Instance.AddEnemey(enemy);       
    }
}
