// 职责：从候选目标中选择射程内距离炮塔最近的目标。
using System;

namespace TowerDefense.Tower
{
    public sealed class TargetSelector
    {
        public TargetPoint SelectNearest(
            TargetPoint[] targets,
            float towerX,
            float towerY,
            float range)
        {
            TargetPoint selected = null;
            float nearestDistance = range;

            for (int i = 0; i < targets.Length; i++)
            {
                TargetPoint target = targets[i];
                if (!target.IsAlive)
                {
                    continue;
                }

                float distance = Distance(towerX, towerY, target.X, target.Y);
                if (distance <= nearestDistance)
                {
                    nearestDistance = distance;
                    selected = target;
                }
            }

            return selected;
        }

        private static float Distance(float startX, float startY, float endX, float endY)
        {
            double dx = endX - startX;
            double dy = endY - startY;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
