// 职责：表示炮塔可锁定目标的当前位置与存活状态。
namespace TowerDefense.Tower
{
    public sealed class TargetPoint
    {
        public TargetPoint(float x, float y)
        {
            X = x;
            Y = y;
            IsAlive = true;
        }

        public float X { get; private set; }

        public float Y { get; private set; }

        public bool IsAlive { get; private set; }

        public void MoveTo(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void MarkDead()
        {
            IsAlive = false;
        }
    }
}
