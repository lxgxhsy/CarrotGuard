// 职责：按路径点顺序推进敌人位置，并在抵达终点时通知外部。
using System;

namespace TowerDefense.Enemy
{
    public sealed class PathFollower
    {
        private readonly Waypoint[] waypoints;
        private int targetIndex;

        public event Action OnReachedEnd;

        public PathFollower(Waypoint[] waypoints)
            : this(waypoints, 1f)
        {
        }

        public PathFollower(Waypoint[] waypoints, float speed)
        {
            this.waypoints = waypoints;
            Speed = speed;
            X = waypoints[0].X;
            Y = waypoints[0].Y;
            targetIndex = 1;
        }

        public float X { get; private set; }

        public float Y { get; private set; }

        public bool HasReachedEnd { get; private set; }

        public float Speed { get; private set; }

        public void Tick(float deltaTime)
        {
            if (HasReachedEnd)
            {
                return;
            }

            Waypoint target = waypoints[targetIndex];
            float distance = DistanceTo(target);
            float travelDistance = Speed * deltaTime;
            if (distance <= travelDistance)
            {
                X = target.X;
                Y = target.Y;
                MoveToNextTarget();
                return;
            }

            X += (target.X - X) / distance * travelDistance;
            Y += (target.Y - Y) / distance * travelDistance;
        }

        private float DistanceTo(Waypoint target)
        {
            double dx = target.X - X;
            double dy = target.Y - Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private void MoveToNextTarget()
        {
            targetIndex++;
            if (targetIndex < waypoints.Length)
            {
                return;
            }

            HasReachedEnd = true;
            if (OnReachedEnd != null)
            {
                OnReachedEnd.Invoke();
            }
        }
    }
}
