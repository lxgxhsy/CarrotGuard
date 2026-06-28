// 职责：描述基础炮塔的固定属性配置。
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TowerDefense.Tower
{
    public enum TowerType
    {
        Rapid,
        Heavy
    }

    public sealed class TowerData
    {
        public TowerData(
            TowerType type,
            int cost,
            int damage,
            float range,
            float fireInterval,
            int upgradeCost)
        {
            Type = type;
            Cost = cost;
            Damage = damage;
            Range = range;
            FireInterval = fireInterval;
            UpgradeCost = upgradeCost;
        }

        public TowerType Type { get; private set; }

        public int Cost { get; private set; }

        public int Damage { get; private set; }

        public float Range { get; private set; }

        public float FireInterval { get; private set; }

        public int UpgradeCost { get; private set; }

        public static ReadOnlyCollection<TowerData> CreateDefaults()
        {
            return new ReadOnlyCollection<TowerData>(new List<TowerData>
            {
                new TowerData(TowerType.Rapid, 50, 5, 3f, 0.35f, 40),
                new TowerData(TowerType.Heavy, 80, 15, 4f, 1.2f, 60)
            });
        }
    }
}
