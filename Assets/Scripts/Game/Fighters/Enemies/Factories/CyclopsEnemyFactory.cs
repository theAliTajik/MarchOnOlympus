
using UnityEngine;

public class CyclopsEnemyFactory : BaseEnemyFactory
{
    public override string FactoryID { get; } = "Cyclops";

    public virtual BaseEnemy SpawnEnemy(EnemiesDb.BossInfo bossInfo, Transform parent)
    {
        BaseEnemy enemy = Instantiate(bossInfo.Prefab, Vector3.zero, Quaternion.identity, parent);

        return enemy;
    }

    public override void SetupEnemy(BaseEnemy enemy, Vector3? position = null, Quaternion? rotation = null)
    {
        base.SetupEnemy(enemy, position, rotation);

        if (enemy is not Cyclops cyclops)
        {
            Debug.Log("ERROR: Non cyclops enemy passed to cyclops enemy factory");
            return;
        }


        Debug.Log("tried to spawn hp bar for eye and club");
        HUD.Instance.SpawnHPBar(cyclops.Club);
        HUD.Instance.SpawnHPBar(cyclops.Eye);

    }
}
