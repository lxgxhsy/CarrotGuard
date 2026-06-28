// 职责：验证两种基础炮塔的数据配置。
using System;
using System.Collections;
using NUnit.Framework;

namespace TowerDefense.Tests
{
    public sealed class TowerDataTests
    {
        [Test]
        public void DefaultTowersExposeFastAndHeavyStats()
        {
            Type type = Type.GetType("TowerDefense.Tower.TowerData, TowerDefense");
            Assert.NotNull(type, "TowerData type should exist.");

            IList towers = (IList)type.GetMethod("CreateDefaults").Invoke(null, null);

            Assert.AreEqual(2, towers.Count);
            AssertTower(towers[0], "Rapid", 50, 5, 3f, 0.35f, 40);
            AssertTower(towers[1], "Heavy", 80, 15, 4f, 1.2f, 60);
        }

        private static void AssertTower(
            object tower,
            string typeName,
            int cost,
            int damage,
            float range,
            float fireInterval,
            int upgradeCost)
        {
            Type towerType = tower.GetType();
            Assert.AreEqual(typeName, towerType.GetProperty("Type").GetValue(tower).ToString());
            Assert.AreEqual(cost, towerType.GetProperty("Cost").GetValue(tower));
            Assert.AreEqual(damage, towerType.GetProperty("Damage").GetValue(tower));
            Assert.AreEqual(range, (float)towerType.GetProperty("Range").GetValue(tower), 0.001f);
            Assert.AreEqual(fireInterval, (float)towerType.GetProperty("FireInterval").GetValue(tower), 0.001f);
            Assert.AreEqual(upgradeCost, towerType.GetProperty("UpgradeCost").GetValue(tower));
        }
    }
}
