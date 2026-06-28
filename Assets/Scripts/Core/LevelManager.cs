// 职责：集中持有关卡运行时系统，并协调敌人死亡、到达终点和胜负状态。
using System;
using UnityEngine;
using TowerDefense.Enemy;
using TowerDefense.Tower;
using EnemyBehaviour = TowerDefense.Enemy.Enemy;

namespace TowerDefense.Core
{
    public sealed class LevelManager : MonoBehaviour
    {
        [SerializeField] private int initialGold = 150;
        [SerializeField] private int initialHealth = 10;
        [SerializeField] private int endpointDamage = 1;

        private HealthSystem healthSystem;
        private GoldSystem goldSystem;
        private VictoryChecker victoryChecker;
        private GridSystem gridSystem;
        private BuildService buildService;
        private int aliveEnemies;

        public event Action GoldChanged;
        public event Action HealthChanged;
        public event Action GameOver;
        public event Action Victory;

        public static LevelManager Instance { get; private set; }

        public int Gold { get { return goldSystem.Balance; } }

        public int Health { get { return healthSystem.CurrentHealth; } }

        public bool IsGameOver { get; private set; }

        public WaveSpawner WaveSpawner { get; private set; }

        public GridSystem GridSystem { get { return gridSystem; } }

        private void Awake()
        {
            Instance = this;
            InitializeSystems();
        }

        private void InitializeSystems()
        {
            goldSystem = new GoldSystem(initialGold);
            healthSystem = new HealthSystem(initialHealth);
            victoryChecker = new VictoryChecker();
            gridSystem = new GridSystem();
            RebuildBuildService();
            WaveSpawner = new WaveSpawner(WaveConfig.CreateDefault());
            healthSystem.OnDead += HandleGameOver;
            victoryChecker.OnVictory += HandleVictory;
        }

        private void Update()
        {
            if (IsGameOver || WaveSpawner == null)
            {
                return;
            }

            WaveSpawner.Tick(Time.deltaTime);
            victoryChecker.Evaluate(WaveSpawner.IsCompleted, aliveEnemies);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void RegisterEnemy(EnemyBehaviour enemy)
        {
            if (IsGameOver)
            {
                Destroy(enemy.gameObject);
                return;
            }

            aliveEnemies++;
            enemy.ReachedEnd += HandleEnemyReachedEnd;
            enemy.Killed += HandleEnemyKilled;
        }

        public bool TryBuild(float x, float y, TowerData data)
        {
            if (IsGameOver)
            {
                return false;
            }

            bool built = buildService.TryBuild(x, y, data.Cost);
            if (built)
            {
                RaiseGoldChanged();
            }

            return built;
        }

        public void AwardGold(int amount)
        {
            goldSystem = new GoldSystem(goldSystem.Balance + amount);
            RebuildBuildService();
            RaiseGoldChanged();
        }

        public bool TryUpgradeTower(TowerInstance tower, TowerUpgrade towerUpgrade)
        {
            if (IsGameOver)
            {
                return false;
            }

            bool upgraded = towerUpgrade.Upgrade(tower, goldSystem);
            if (upgraded)
            {
                RaiseGoldChanged();
            }

            return upgraded;
        }

        private void HandleEnemyReachedEnd(EnemyBehaviour enemy)
        {
            aliveEnemies = Math.Max(0, aliveEnemies - 1);
            if (IsGameOver)
            {
                Destroy(enemy.gameObject);
                return;
            }

            healthSystem.ApplyDamage(endpointDamage);
            RaiseHealthChanged();
            Destroy(enemy.gameObject);
        }

        private void HandleEnemyKilled(EnemyBehaviour enemy, int reward)
        {
            aliveEnemies = Math.Max(0, aliveEnemies - 1);
            AwardGold(reward);
        }

        private void RebuildBuildService()
        {
            buildService = new BuildService(goldSystem, gridSystem);
        }

        private void HandleGameOver()
        {
            if (IsGameOver)
            {
                return;
            }

            IsGameOver = true;
            if (GameOver != null)
            {
                GameOver.Invoke();
            }
        }

        private void HandleVictory()
        {
            if (IsGameOver)
            {
                return;
            }

            IsGameOver = true;
            if (Victory != null)
            {
                Victory.Invoke();
            }
        }

        private void RaiseGoldChanged()
        {
            if (GoldChanged != null)
            {
                GoldChanged.Invoke();
            }
        }

        private void RaiseHealthChanged()
        {
            if (HealthChanged != null)
            {
                HealthChanged.Invoke();
            }
        }
    }
}
