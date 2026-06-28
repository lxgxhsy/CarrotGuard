// 职责：验证炮塔目标选择规则。
using System;
using NUnit.Framework;

namespace TowerDefense.Tests
{
    public sealed class TargetSelectorTests
    {
        [Test]
        public void SelectsNearestTargetAndDropsTargetsOutsideRange()
        {
            Type targetType = Type.GetType("TowerDefense.Tower.TargetPoint, TowerDefense");
            Type selectorType = Type.GetType("TowerDefense.Tower.TargetSelector, TowerDefense");
            Assert.NotNull(targetType, "TargetPoint type should exist.");
            Assert.NotNull(selectorType, "TargetSelector type should exist.");

            Array targets = Array.CreateInstance(targetType, 3);
            object near = Activator.CreateInstance(targetType, 2f, 0f);
            object far = Activator.CreateInstance(targetType, 4f, 0f);
            object outside = Activator.CreateInstance(targetType, 10f, 0f);
            targets.SetValue(far, 0);
            targets.SetValue(outside, 1);
            targets.SetValue(near, 2);

            object selector = Activator.CreateInstance(selectorType);
            object selected = Select(selector, targets, 0f, 0f, 5f);
            Assert.AreSame(near, selected);

            Assert.Null(Select(selector, targets, 0f, 0f, 1f));

            targetType.GetMethod("MoveTo").Invoke(near, new object[] { 6f, 0f });
            Array singleTarget = Array.CreateInstance(targetType, 1);
            singleTarget.SetValue(near, 0);
            Assert.Null(Select(selector, singleTarget, 0f, 0f, 5f));
        }

        private static object Select(object selector, Array targets, float x, float y, float range)
        {
            return selector.GetType()
                .GetMethod("SelectNearest")
                .Invoke(selector, new object[] { targets, x, y, range });
        }
    }
}
