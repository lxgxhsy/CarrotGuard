// 职责：验证生命系统扣血与死亡事件触发规则。
using System;
using NUnit.Framework;

namespace TowerDefense.Tests
{
    public sealed class HealthSystemTests
    {
        [Test]
        public void AppliesDamageAndRaisesDeathOnce()
        {
            Type type = Type.GetType("TowerDefense.Core.HealthSystem, TowerDefense");
            Assert.NotNull(type, "HealthSystem type should exist.");

            object health = Activator.CreateInstance(type, 10);
            int deadCount = 0;
            type.GetEvent("OnDead").AddEventHandler(health, (Action)(() => deadCount++));

            type.GetMethod("ApplyDamage").Invoke(health, new object[] { 4 });
            Assert.AreEqual(6, ReadCurrentHealth(health));
            Assert.AreEqual(0, deadCount);

            type.GetMethod("ApplyDamage").Invoke(health, new object[] { 7 });
            Assert.AreEqual(0, ReadCurrentHealth(health));
            Assert.AreEqual(1, deadCount);

            type.GetMethod("ApplyDamage").Invoke(health, new object[] { 3 });
            Assert.AreEqual(0, ReadCurrentHealth(health));
            Assert.AreEqual(1, deadCount);
        }

        private static int ReadCurrentHealth(object health)
        {
            return (int)health.GetType().GetProperty("CurrentHealth").GetValue(health);
        }
    }
}
