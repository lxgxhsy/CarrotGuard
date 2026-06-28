// 职责：验证默认波次配置能提供三波递增刷怪数据。
using System;
using System.Collections;
using NUnit.Framework;

namespace TowerDefense.Tests
{
    public sealed class WaveConfigTests
    {
        [Test]
        public void DefaultConfigContainsThreeIncreasingWaves()
        {
            Type type = Type.GetType("TowerDefense.Core.WaveConfig, TowerDefense");
            Assert.NotNull(type, "WaveConfig type should exist.");

            object config = type.GetMethod("CreateDefault").Invoke(null, null);
            IList waves = (IList)type.GetProperty("Waves").GetValue(config);

            Assert.AreEqual(3, waves.Count);
            AssertWave(waves[0], 5, 1f, 2f);
            AssertWave(waves[1], 8, 0.8f, 3f);
            AssertWave(waves[2], 12, 0.6f, 0f);
        }

        private static void AssertWave(object wave, int count, float interval, float breakAfter)
        {
            Type waveType = wave.GetType();
            Assert.AreEqual(count, waveType.GetProperty("EnemyCount").GetValue(wave));
            Assert.AreEqual(interval, (float)waveType.GetProperty("SpawnInterval").GetValue(wave), 0.001f);
            Assert.AreEqual(breakAfter, (float)waveType.GetProperty("BreakAfterSeconds").GetValue(wave), 0.001f);
        }
    }
}
