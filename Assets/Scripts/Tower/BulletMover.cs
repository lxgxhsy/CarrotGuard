// 职责：推进子弹位置，使其追踪目标，并在目标失效时标记自毁。
using System;

namespace TowerDefense.Tower
{
    public sealed class BulletMover
    {
        private readonly float speed;
        private readonly TargetPoint target;

        public BulletMover(float x, float y, float speed, TargetPoint target)
        {
            X = x;
            Y = y;
            this.speed = speed;
            this.target = target;
        }

        public float X { get; private set; }

        public float Y { get; private set; }

        public bool IsDestroyed { get; private set; }

        public void Tick(float deltaTime)
        {
            if (target == null || !target.IsAlive)
            {
                IsDestroyed = true;
                return;
            }

            float distance = DistanceToTarget();
            float travelDistance = speed * deltaTime;
            if (distance <= travelDistance)
            {
                X = target.X;
                Y = target.Y;
                return;
            }

            X += (target.X - X) / distance * travelDistance;
            Y += (target.Y - Y) / distance * travelDistance;
        }

        private float DistanceToTarget()
        {
            double dx = target.X - X;
            double dy = target.Y - Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
