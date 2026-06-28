// 职责：将世界坐标对齐到建造格，并记录格子占用状态。
using System;
using System.Collections.Generic;

namespace TowerDefense.Core
{
    public sealed class GridSystem
    {
        private readonly HashSet<GridCell> occupiedCells = new HashSet<GridCell>();

        public GridCell Align(float x, float y)
        {
            return new GridCell((int)Math.Round(x), (int)Math.Round(y));
        }

        public bool IsOccupied(GridCell cell)
        {
            return occupiedCells.Contains(cell);
        }

        public void SetOccupied(GridCell cell, bool occupied)
        {
            if (occupied)
            {
                occupiedCells.Add(cell);
                return;
            }

            occupiedCells.Remove(cell);
        }
    }
}
