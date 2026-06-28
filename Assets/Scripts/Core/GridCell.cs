// 职责：表示建造网格中的一个整数坐标格。
using System;

namespace TowerDefense.Core
{
    public struct GridCell : IEquatable<GridCell>
    {
        public GridCell(int x, int y)
            : this()
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public bool Equals(GridCell other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is GridCell && Equals((GridCell)obj);
        }

        public override int GetHashCode()
        {
            return (X * 397) ^ Y;
        }
    }
}
