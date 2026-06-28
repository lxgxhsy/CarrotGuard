// 职责：提供关卡刷怪波次的纯数据配置。
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TowerDefense.Core
{
    public sealed class WaveConfig
    {
        public WaveConfig(IList<WaveInfo> waves)
        {
            Waves = new ReadOnlyCollection<WaveInfo>(waves);
        }

        public ReadOnlyCollection<WaveInfo> Waves { get; private set; }

        public static WaveConfig CreateDefault()
        {
            return new WaveConfig(new List<WaveInfo>
            {
                new WaveInfo(5, 1f, 2f),
                new WaveInfo(8, 0.8f, 3f),
                new WaveInfo(12, 0.6f, 0f)
            });
        }
    }

    public sealed class WaveInfo
    {
        public WaveInfo(int enemyCount, float spawnInterval, float breakAfterSeconds)
        {
            EnemyCount = enemyCount;
            SpawnInterval = spawnInterval;
            BreakAfterSeconds = breakAfterSeconds;
        }

        public int EnemyCount { get; private set; }

        public float SpawnInterval { get; private set; }

        public float BreakAfterSeconds { get; private set; }
    }
}
