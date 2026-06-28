// 职责：验证刷怪器按波次配置产出敌人并遵守波间间隔。
using System;
using System.Collections;
using NUnit.Framework;

namespace TowerDefense.Tests
{
    public sealed class WaveSpawnerTests
    {
        [Test]
        public void SpawnsByConfigAndWaitsBetweenWaves()
        {
            Type configType = Type.GetType("TowerDefense.Core.WaveConfig, TowerDefense");
            Type waveType = Type.GetType("TowerDefense.Core.WaveInfo, TowerDefense");
            Type spawnerType = Type.GetType("TowerDefense.Enemy.WaveSpawner, TowerDefense");
            Assert.NotNull(configType, "WaveConfig type should exist.");
            Assert.NotNull(waveType, "WaveInfo type should exist.");
            Assert.NotNull(spawnerType, "WaveSpawner type should exist.");

            Array waves = Array.CreateInstance(waveType, 2);
            waves.SetValue(Activator.CreateInstance(waveType, 2, 1f, 2f), 0);
            waves.SetValue(Activator.CreateInstance(waveType, 1, 1f, 0f), 1);
            object config = Activator.CreateInstance(configType, waves);
            object spawner = Activator.CreateInstance(spawnerType, config);
            ArrayList spawnedWaves = new ArrayList();
            spawnerType.GetEvent("OnSpawn").AddEventHandler(spawner, (Action<int>)(wave => spawnedWaves.Add(wave)));

            Tick(spawner, 1f);
            Tick(spawner, 1f);
            Tick(spawner, 1.9f);
            Assert.AreEqual(2, spawnedWaves.Count);

            Tick(spawner, 0.1f);
            Assert.AreEqual(3, spawnedWaves.Count);
            Assert.AreEqual(0, spawnedWaves[0]);
            Assert.AreEqual(0, spawnedWaves[1]);
            Assert.AreEqual(1, spawnedWaves[2]);
            Assert.True((bool)spawnerType.GetProperty("IsCompleted").GetValue(spawner));
        }

        private static void Tick(object spawner, float deltaTime)
        {
            spawner.GetType().GetMethod("Tick").Invoke(spawner, new object[] { deltaTime });
        }
    }
}
