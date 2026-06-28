// 职责：验证炮塔升级与出售返还规则。
using System;
using System.Collections;
using NUnit.Framework;

namespace TowerDefense.Tests
{
    public sealed class TowerUpgradeTests
    {
        [Test]
        public void UpgradeSpendsGoldAndSellRefundsHalfOfTotalSpent()
        {
            Type dataType = Type.GetType("TowerDefense.Tower.TowerData, TowerDefense");
            Type instanceType = Type.GetType("TowerDefense.Tower.TowerInstance, TowerDefense");
            Type upgradeType = Type.GetType("TowerDefense.Tower.TowerUpgrade, TowerDefense");
            Type goldType = Type.GetType("TowerDefense.Core.GoldSystem, TowerDefense");
            Assert.NotNull(dataType, "TowerData type should exist.");
            Assert.NotNull(instanceType, "TowerInstance type should exist.");
            Assert.NotNull(upgradeType, "TowerUpgrade type should exist.");
            Assert.NotNull(goldType, "GoldSystem type should exist.");

            IList towers = (IList)dataType.GetMethod("CreateDefaults").Invoke(null, null);
            object tower = Activator.CreateInstance(instanceType, towers[0]);
            object upgrade = Activator.CreateInstance(upgradeType);
            object gold = Activator.CreateInstance(goldType, 100);

            Assert.True((bool)upgradeType.GetMethod("Upgrade").Invoke(upgrade, new[] { tower, gold }));
            Assert.AreEqual(60, goldType.GetProperty("Balance").GetValue(gold));
            Assert.AreEqual(2, instanceType.GetProperty("Level").GetValue(tower));
            Assert.AreEqual(10, instanceType.GetProperty("Damage").GetValue(tower));

            int refund = (int)upgradeType.GetMethod("SellRefund").Invoke(upgrade, new[] { tower });
            Assert.AreEqual(45, refund);
            Assert.True((bool)instanceType.GetProperty("IsSold").GetValue(tower));
        }
    }
}
