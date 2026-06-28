// 职责：表示敌人路径中的一个二维路径点。
namespace TowerDefense.Enemy
{
    public struct Waypoint
    {
        public Waypoint(float x, float y)
            : this()
        {
            X = x;
            Y = y;
        }

        public float X { get; private set; }

        public float Y { get; private set; }
    }
}
