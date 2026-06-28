// 职责：根据波次配置按时间产出敌人生成信号。
using System;
using TowerDefense.Core;

namespace TowerDefense.Enemy
{
    public sealed class WaveSpawner
    {
        private readonly WaveConfig config;
        private int currentWaveIndex;
        private int spawnedInCurrentWave;
        private float timeUntilNextSpawn;

        public event Action<int> OnSpawn;

        public WaveSpawner(WaveConfig config)
        {
            this.config = config;
            timeUntilNextSpawn = config.Waves[0].SpawnInterval;
        }

        public bool IsCompleted { get; private set; }

        public void Tick(float deltaTime)
        {
            if (IsCompleted)
            {
                return;
            }

            timeUntilNextSpawn -= deltaTime;
            while (timeUntilNextSpawn <= 0.0001f && !IsCompleted)
            {
                SpawnCurrentWaveEnemy();
            }
        }

        private void SpawnCurrentWaveEnemy()
        {
            WaveInfo wave = config.Waves[currentWaveIndex];
            if (OnSpawn != null)
            {
                OnSpawn.Invoke(currentWaveIndex);
            }

            spawnedInCurrentWave++;
            if (spawnedInCurrentWave < wave.EnemyCount)
            {
                timeUntilNextSpawn += wave.SpawnInterval;
                return;
            }

            AdvanceWave(wave);
        }

        private void AdvanceWave(WaveInfo completedWave)
        {
            currentWaveIndex++;
            spawnedInCurrentWave = 0;
            if (currentWaveIndex >= config.Waves.Count)
            {
                IsCompleted = true;
                return;
            }

            timeUntilNextSpawn += completedWave.BreakAfterSeconds;
        }
    }
}
