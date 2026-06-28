// 职责：验证关卡管理器的金币升级和终止状态。
using System;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using TowerDefense.Core;
using TowerDefense.Tower;

namespace TowerDefense.Tests
{
    public sealed class LevelManagerTests
    {
        [Test]
        public void VictoryEndsLevelAndBlocksBuilding()
        {
            GameObject gameObject = new GameObject("LevelManagerTest");
            try
            {
                LevelManager manager = gameObject.AddComponent<LevelManager>();
                typeof(LevelManager)
                    .GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(manager, null);

                TowerInstance tower = new TowerInstance(TowerData.CreateDefaults()[0]);
                Assert.True(manager.TryUpgradeTower(tower, new TowerUpgrade()));
                Assert.AreEqual(110, manager.Gold);
                Assert.AreEqual(2, tower.Level);

                int victoryCount = 0;
                manager.Victory += () => victoryCount++;

                typeof(LevelManager)
                    .GetMethod("HandleVictory", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(manager, null);
                typeof(LevelManager)
                    .GetMethod("HandleVictory", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(manager, null);

                Assert.True(manager.IsGameOver);
                Assert.AreEqual(1, victoryCount);
                Assert.False(manager.TryBuild(0f, 0f, TowerData.CreateDefaults()[0]));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(gameObject);
            }
        }
    }
}
