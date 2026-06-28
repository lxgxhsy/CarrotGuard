// 职责：处理炮塔升级扣款和出售返还计算。
using TowerDefense.Core;

namespace TowerDefense.Tower
{
    public sealed class TowerUpgrade
    {
        public bool Upgrade(TowerInstance tower, GoldSystem goldSystem)
        {
            if (!goldSystem.Spend(tower.UpgradeCost))
            {
                return false;
            }

            tower.ApplyUpgrade();
            return true;
        }

        public int SellRefund(TowerInstance tower)
        {
            tower.MarkSold();
            return tower.TotalSpent / 2;
        }
    }
}
