using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class EnemiesManager : Singleton<EnemiesManager>
{
    public event Action OnAllEnemiesDestroyed;

    [SerializeField] private EnemiesDb enemiesDb;
    [SerializeField] private string[] m_enemiesToLoad;
    [SerializeField] private Transform[] m_spawnPoint;

    
    [SerializeField] private Dictionary<string, IEnemyFactory> m_factories = new Dictionary<string, IEnemyFactory>();
    
    private List<Fighter> m_enemies = new List<Fighter>();
    
    private List<Fighter> m_enemiesToPlayTwice = new List<Fighter>();
    
    private int m_currentSpawnPoint = -1;

    private void Start()
    {
        IEnemyFactory[] factories = GetComponents<IEnemyFactory>();
        for (var i = 0; i < factories.Length; i++)
        {
            m_factories[factories[i].FactoryID] = factories[i];
        }
        if (!string.IsNullOrEmpty(GameSessionParams.enemyClientId))
        {
            LoadEnemies(GameSessionParams.enemyClientId);
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

            SetNextSpawnPoint();
            factory.SetupEnemy(enemy, m_spawnPoint[m_currentSpawnPoint].position);
            if (determineIntention)
            {
                enemy.DetermineIntention();
            }
        }


        return spawnedEnemeis;
    }

    public void AddEnemey(BaseEnemy enemy)
    {
        enemy.Death += OnEnemyDied;
        m_enemies.Add(enemy);
    }

    public void SetNextSpawnPoint()
    {
        if (m_currentSpawnPoint >= m_spawnPoint.Length -1)
        {
            m_currentSpawnPoint = 0;
        }
        else
        {
            m_currentSpawnPoint++;
        }
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
        GameplayEvents.SendGamePhaseChanged(EGamePhase.ENEMY_KILLED);
        if (EnemiesAreDead())
        {
            OnAllEnemiesDestroyed?.Invoke();
        }
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

            yield return new WaitUntil(() => isActionComplete);
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
        if (m_enemies.Count <= 0)
        {
            return true;
        }

        return false;
    }

    protected override void Init()
    {
    }

    public int GetNumOfEnemies()
    {
        return m_enemies.Count;
    }
}
