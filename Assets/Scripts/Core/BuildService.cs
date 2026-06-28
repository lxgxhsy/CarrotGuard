// 职责：处理建造请求，确保金币足够且目标格未被占用。
namespace TowerDefense.Core
{
    public sealed class BuildService
    {
        private readonly GoldSystem goldSystem;
        private readonly GridSystem gridSystem;

        public BuildService(GoldSystem goldSystem, GridSystem gridSystem)
        {
            this.goldSystem = goldSystem;
            this.gridSystem = gridSystem;
        }

        public bool TryBuild(float x, float y, int cost)
        {
            GridCell cell = gridSystem.Align(x, y);
            if (gridSystem.IsOccupied(cell))
            {
                return false;
            }

            if (!goldSystem.Spend(cost))
            {
                return false;
            }

            gridSystem.SetOccupied(cell, true);
            return true;
        }
    }
}
