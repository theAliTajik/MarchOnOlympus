using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class EnemiesManager : Singleton<EnemiesManager>
{
    public event Action OnAllEnemiesDestroyed;

    [SerializeField] private EnemiesDb enemiesDb;
    [SerializeField] private string[] m_enemiesToLoad;
    
    

    
    [SerializeField] private Dictionary<string, IEnemyFactory> m_factories = new Dictionary<string, IEnemyFactory>();
    
    private List<Fighter> m_enemies = new List<Fighter>();
    private List<Fighter> m_deadEnemies = new List<Fighter>();
    
    private List<Fighter> m_enemiesToPlayTwice = new List<Fighter>();
    
    
    private int m_currentWave = 0;
    private CombatWavesDb.CombatWaveSet m_combatWaveSet;

    private void Start()
    {
        IEnemyFactory[] factories = GetComponents<IEnemyFactory>();
        for (var i = 0; i < factories.Length; i++)
        {
            m_factories[factories[i].FactoryID] = factories[i];
        }
        
        if (!string.IsNullOrEmpty(GameSessionParams.EnemyClientId))
        {
            LoadEnemies(GameSessionParams.EnemyClientId);
        }
        else if(!string.IsNullOrEmpty(GameSessionParams.WaveClientId))
        {
            m_combatWaveSet = CombatWavesDb.Instance.FindById(GameSessionParams.WaveClientId);
            if (m_combatWaveSet == null)
            {
                Debug.Log("wave set was not found by id");
                return;
            }
            SpawnWave();

        }
        else
        {
            for (int i = 0; i < m_enemiesToLoad.Length; i++)
            {
                LoadEnemies(m_enemiesToLoad[i]);
            }
        }

        DetermineAllEnemiesIntentions();

        GameplayEvents.SpawnBoss += SpawnBossAdaptor;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameplayEvents.SpawnBoss -= SpawnBossAdaptor;
    }

    private void SpawnWave()
    {
        if (m_combatWaveSet == null)
        {
            Debug.Log("combat wave set is null");
            return;
        }


        List<EnemiesDb.BossInfo> enemiesToSpawn = m_combatWaveSet.Waves[m_currentWave].Enemies;
        foreach (var boss in enemiesToSpawn)
        {
            SpawnBoss(boss, true);
        }
    }
    
    public void LoadEnemies(params string[] enemyIds)
    {
        for (int i = 0; i < enemyIds.Length; i++)
        {
            SpawnBoss(enemyIds[i]);
        }
    }

    private void SpawnBossAdaptor(string enemyID)
    {
        SpawnBoss(enemyID);
    }

    public List<BaseEnemy> SpawnBoss(string enemyId, bool determineIntention = false)
    {
        EnemiesDb.EnemyInfo info = enemiesDb.FindById(enemyId);
        IEnemyFactory factory;
        List<BaseEnemy> spawnedEnemeis = new List<BaseEnemy>();

        for (var i = 0; i < info.bosses.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(info.bosses[i].Factory))
            {
                factory = m_factories["Default"];
            }
            else
            {
                factory = m_factories[info.bosses[i].Factory];
            }
            
            if (factory == null)
            {
                Debug.Log("default factory not found");
                continue;
            }

            BaseEnemy enemy = factory.SpawnEnemy(info.bosses[i], transform);
            spawnedEnemeis.Add(enemy);

            Vector3 pos = EnemiesPositionManager.Instance.AddOccupantToNextSpawnPoint(enemy);
            factory.SetupEnemy(enemy, pos);
            if (determineIntention)
            {
                enemy.DetermineIntention();
            }
        }


        return spawnedEnemeis;
    }


    public List<BaseEnemy> SpawnBoss(EnemiesDb.BossInfo boss, bool determineIntention = false)
    {
        IEnemyFactory factory;
        List<BaseEnemy> spawnedEnemeis = new List<BaseEnemy>();

        if (string.IsNullOrWhiteSpace(boss.Factory))
        {
            factory = m_factories["Default"];
        }
        else
        {
            factory = m_factories[boss.Factory];
        }
        
        if (factory == null)
        {
            Debug.Log("null factory when enemy spawning");
            return null;
        }

        BaseEnemy enemy = factory.SpawnEnemy(boss, transform);
        spawnedEnemeis.Add(enemy);

        Vector3 pos = EnemiesPositionManager.Instance.AddOccupantToNextSpawnPoint(enemy);
        factory.SetupEnemy(enemy, pos);
        if (determineIntention)
        {
            enemy.DetermineIntention();
        }
        return spawnedEnemeis;
    }

    public void AddEnemey(BaseEnemy enemy)
    {
        enemy.Death += OnEnemyDied;
        m_enemies.Add(enemy);
    }

    
    public BaseEnemy SetupEnemy(BaseEnemy enemy, Vector3? position = null)
    {
        if (position != null)
        {
            enemy.transform.position = position.Value;
        }
        
        enemy.Death += OnEnemyDied;
        enemy.ConfigFighterHP();
        
            
        HUD.Instance.SpawnHPBar(enemy);
        HUD.Instance.SpawnDamageIndicator(enemy);
        HUD.Instance.SpawnEnemyIntentionWidget(enemy);
        MechanicsManager.Instance.CreateMechanicsList(enemy);
        CombatManager.Instance.SubscribeToEnemyDamage(enemy);
        m_enemies.Add(enemy);
        enemy.DetermineIntention();
        return enemy;
    }
    
    public void RemoveDeadEnemy(Fighter enemy)
    {
        if (enemy == null) return;

        enemy.Death -= OnEnemyDied;

        // Remove UI elements
        HUD.Instance.RemoveHPBar(enemy);
        HUD.Instance.RemoveDamageIndicator(enemy);
        HUD.Instance.RemoveEnemyIntentionWidget(enemy);
        HUD.Instance.RemoveMechanicsDisplay(enemy);

        // Clean up mechanics and combat manager references
        MechanicsManager.Instance.RemoveMechanicsList(enemy);
        CombatManager.Instance.UnsubscribeFromEnemyDamage(enemy);

        // Remove from enemies list
        m_enemies.Remove(enemy);
        EnemiesPositionManager.Instance.RemoveEnemyFromSpawnPoint(enemy);
        // Destroy enemy GameObject
        Destroy(enemy.gameObject);
    }


    public List<Fighter> GetAllEnemies()
    {
        return m_enemies;
    }

    public Fighter GetRandomEnemy()
    {
        return m_enemies[UnityEngine.Random.Range(0, m_enemies.Count)];
    }

    public void OnEnemyDied(Fighter enemy)
    {
        m_enemies.Remove(enemy);
        m_deadEnemies.Add(enemy);
        GameplayEvents.SendGamePhaseChanged(EGamePhase.ENEMY_KILLED);
        if (EnemiesAreDead())
        {
            if (NextWaveExists())
            {
                m_currentWave++;
                ClearEnemies();
                SpawnWave();
                return;
            }
            OnAllEnemiesDestroyed?.Invoke();
        }
    }

    private void ClearEnemies()
    {
        foreach (var enemy in m_deadEnemies)
        {
            RemoveDeadEnemy(enemy);
        }
    }


    private bool NextWaveExists()
    {
        if (m_combatWaveSet == null)
        {
            return false;
        }

        if (m_currentWave >= m_combatWaveSet.Waves.Count-1)
        {
            return false;
        }

        return true;
    }


    public void DetermineAllEnemiesIntentions()
    {
        foreach (BaseEnemy enemy in m_enemies)
        {
            enemy.DetermineIntention();
        }
    }
    
    public void PlayEnemiesTurns(Action finishCallBack)
    {
        StartCoroutine(PlayEnemiesTurnsSequentially(finishCallBack));
    }

    private IEnumerator PlayEnemiesTurnsSequentially(Action FinishCallBack)
    {
        int enemyCount = m_enemies.Count; // becasue enemy count might change
        for (int i = enemyCount -1; i >= 0 ; i--)
        {
            bool isActionComplete = false;
            BaseEnemy enemy = (BaseEnemy)m_enemies[i];

            if (DetermineIfEnemyPlaysTwice(enemy))
            {
                enemy.ExecuteAction(() => isActionComplete = true);
                yield return new WaitUntil(() => isActionComplete);
                isActionComplete = false;
                enemy.ExecuteAction(() => isActionComplete = true);
            }
            else
            {
                enemy.ExecuteAction(() => isActionComplete = true);
            }

            // Debug.Log($"enemy play turn: {enemy.name}");
            yield return new WaitUntil(() => isActionComplete);
            // Debug.Log($"enemy played turn: {enemy.name}");
        }
        
        yield return new WaitForSeconds(1f);
        FinishCallBack?.Invoke();
        //Debug.Log("All enemies have taken their turns.");
    }

    public void MakeEnemyPlayTwice(Fighter fighter)
    {
        m_enemiesToPlayTwice.Add(fighter);
    }
    
    private bool DetermineIfEnemyPlaysTwice(Fighter enemy)
    {
        if (m_enemiesToPlayTwice.Contains(enemy))
        {
            m_enemiesToPlayTwice.Remove(enemy);
            return true;
        }
        
        return false;
    }

    public bool EnemiesAreDead()
    {
        foreach (BaseEnemy enemy in m_enemies.OfType<BaseEnemy>())
        {
            if (enemy.IsRequiredForCombatCompletion)
            {
                return false;
            }
        }

        return true;
    }

    protected override void Init()
    {
    }

    public int GetNumOfEnemies()
    {
        return m_enemies.Count;
    }

    public Fighter FindEnemyOfType(Type type)
    {
        Fighter e = m_enemies.Find(e => type.IsInstanceOfType(e));
        return e;
    }
}
