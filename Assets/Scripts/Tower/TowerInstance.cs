// 职责：保存单个已建造炮塔的运行时等级、属性与投入成本。
namespace TowerDefense.Tower
{
    public sealed class TowerInstance
    {
        private readonly TowerData data;

        public TowerInstance(TowerData data)
        {
            this.data = data;
            Level = 1;
            Damage = data.Damage;
            Range = data.Range;
            FireInterval = data.FireInterval;
            TotalSpent = data.Cost;
        }

        public int Level { get; private set; }

        public int Damage { get; private set; }

        public float Range { get; private set; }

        public float FireInterval { get; private set; }

        public int TotalSpent { get; private set; }

        public bool IsSold { get; private set; }

        public int UpgradeCost
        {
            get { return data.UpgradeCost; }
        }

        public void ApplyUpgrade()
        {
            Level++;
            Damage += data.Damage;
            Range += 0.5f;
            TotalSpent += data.UpgradeCost;
        }

        public void MarkSold()
        {
            IsSold = true;
        }
    }
}
