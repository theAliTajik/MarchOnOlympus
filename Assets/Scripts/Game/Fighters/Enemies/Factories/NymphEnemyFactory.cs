
using UnityEngine;

public class NymphEnemyFactory : BaseEnemyFactory
{
    public override string FactoryID { get; } = "Nymph";

    public override BaseEnemy SpawnEnemy(EnemiesDb.BossInfo bossInfo, Transform parent)
    {
        if (!EnemiesManager.Instance.FindEnemyOfType(typeof(BaseNymph)))
        {
            NymphsCoordinator.Reset();
        }

        return base.SpawnEnemy(bossInfo, parent);
    }
    
}
