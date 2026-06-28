// 职责：验证金币系统的余额读取与扣款规则。
using System;
using NUnit.Framework;

namespace TowerDefense.Tests
{
    public sealed class GoldSystemTests
    {
        [Test]
        public void TracksInitialBalanceAndSpendRules()
        {
            Type type = Type.GetType("TowerDefense.Core.GoldSystem, TowerDefense");
            Assert.NotNull(type, "GoldSystem type should exist.");

            object gold = Activator.CreateInstance(type, 100);
            Assert.AreEqual(100, ReadBalance(gold));

            Assert.True((bool)type.GetMethod("Spend").Invoke(gold, new object[] { 40 }));
            Assert.AreEqual(60, ReadBalance(gold));

            Assert.False((bool)type.GetMethod("Spend").Invoke(gold, new object[] { 80 }));
            Assert.AreEqual(60, ReadBalance(gold));
        }

        private static int ReadBalance(object gold)
        {
            return (int)gold.GetType().GetProperty("Balance").GetValue(gold);
        }
    }
}
