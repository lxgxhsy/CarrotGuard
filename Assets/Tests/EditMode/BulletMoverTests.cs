// 职责：验证子弹追踪目标和目标消失后的自毁规则。
using System;
using NUnit.Framework;

namespace TowerDefense.Tests
{
    public sealed class BulletMoverTests
    {
        [Test]
        public void TracksTargetAndDestroysItselfWhenTargetDisappears()
        {
            Type targetType = Type.GetType("TowerDefense.Tower.TargetPoint, TowerDefense");
            Type moverType = Type.GetType("TowerDefense.Tower.BulletMover, TowerDefense");
            Assert.NotNull(targetType, "TargetPoint type should exist.");
            Assert.NotNull(moverType, "BulletMover type should exist.");

            object target = Activator.CreateInstance(targetType, 4f, 0f);
            object mover = Activator.CreateInstance(moverType, 0f, 0f, 2f, target);

            Tick(mover, 1f);
            AssertPosition(mover, 2f, 0f);

            targetType.GetMethod("MoveTo").Invoke(target, new object[] { 2f, 2f });
            Tick(mover, 0.5f);
            AssertPosition(mover, 2f, 1f);

            targetType.GetMethod("MarkDead").Invoke(target, null);
            Tick(mover, 0.1f);
            Assert.True((bool)moverType.GetProperty("IsDestroyed").GetValue(mover));
        }

        private static void Tick(object mover, float deltaTime)
        {
            mover.GetType().GetMethod("Tick").Invoke(mover, new object[] { deltaTime });
        }

        private static void AssertPosition(object mover, float x, float y)
        {
            Type type = mover.GetType();
            Assert.AreEqual(x, (float)type.GetProperty("X").GetValue(mover), 0.001f);
            Assert.AreEqual(y, (float)type.GetProperty("Y").GetValue(mover), 0.001f);
        }
    }
}
