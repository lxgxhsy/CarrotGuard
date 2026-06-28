// 职责：验证敌人受伤、死亡标记与击杀奖励规则。
using System;
using NUnit.Framework;

namespace TowerDefense.Tests
{
    public sealed class DamageCalculatorTests
    {
        [Test]
        public void AppliesDamageAndReturnsRewardOnFirstKill()
        {
            Type stateType = Type.GetType("TowerDefense.Enemy.EnemyState, TowerDefense");
            Type calculatorType = Type.GetType("TowerDefense.Enemy.DamageCalculator, TowerDefense");
            Assert.NotNull(stateType, "EnemyState type should exist.");
            Assert.NotNull(calculatorType, "DamageCalculator type should exist.");

            object enemy = Activator.CreateInstance(stateType, 10, 3);
            object[] firstHit = { enemy, 4 };
            int firstReward = (int)calculatorType.GetMethod("ApplyDamage").Invoke(null, firstHit);
            Assert.AreEqual(6, ReadHealth(enemy));
            Assert.False(ReadDead(enemy));
            Assert.AreEqual(0, firstReward);

            object[] killHit = { enemy, 6 };
            int killReward = (int)calculatorType.GetMethod("ApplyDamage").Invoke(null, killHit);
            Assert.AreEqual(0, ReadHealth(enemy));
            Assert.True(ReadDead(enemy));
            Assert.AreEqual(3, killReward);

            object[] repeatedHit = { enemy, 5 };
            int repeatedReward = (int)calculatorType.GetMethod("ApplyDamage").Invoke(null, repeatedHit);
            Assert.AreEqual(0, repeatedReward);
        }

        private static int ReadHealth(object enemy)
        {
            return (int)enemy.GetType().GetProperty("CurrentHealth").GetValue(enemy);
        }

        private static bool ReadDead(object enemy)
        {
            return (bool)enemy.GetType().GetProperty("IsDead").GetValue(enemy);
        }
    }
}
