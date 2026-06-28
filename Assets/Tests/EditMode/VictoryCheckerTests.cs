// 职责：验证胜利条件只在全波完成且无存活敌人时触发一次。
using System;
using NUnit.Framework;

namespace TowerDefense.Tests
{
    public sealed class VictoryCheckerTests
    {
        [Test]
        public void RaisesVictoryWhenAllWavesCompleteAndNoEnemiesRemain()
        {
            Type type = Type.GetType("TowerDefense.Core.VictoryChecker, TowerDefense");
            Assert.NotNull(type, "VictoryChecker type should exist.");

            object checker = Activator.CreateInstance(type);
            int victoryCount = 0;
            type.GetEvent("OnVictory").AddEventHandler(checker, (Action)(() => victoryCount++));

            Evaluate(checker, false, 0);
            Evaluate(checker, true, 2);
            Assert.AreEqual(0, victoryCount);

            Evaluate(checker, true, 0);
            Assert.AreEqual(1, victoryCount);
            Assert.True((bool)type.GetProperty("HasVictory").GetValue(checker));

            Evaluate(checker, true, 0);
            Assert.AreEqual(1, victoryCount);
        }

        private static void Evaluate(object checker, bool allWavesComplete, int aliveEnemies)
        {
            checker.GetType()
                .GetMethod("Evaluate")
                .Invoke(checker, new object[] { allWavesComplete, aliveEnemies });
        }
    }
}
