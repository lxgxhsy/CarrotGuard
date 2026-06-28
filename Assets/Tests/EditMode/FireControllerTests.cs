// 职责：验证炮塔开火冷却计时规则。
using System;
using NUnit.Framework;

namespace TowerDefense.Tests
{
    public sealed class FireControllerTests
    {
        [Test]
        public void FiresOnlyAfterCooldownWhenTargetExists()
        {
            Type type = Type.GetType("TowerDefense.Tower.FireController, TowerDefense");
            Assert.NotNull(type, "FireController type should exist.");

            object controller = Activator.CreateInstance(type, 1f);
            int fireCount = 0;
            type.GetEvent("OnFire").AddEventHandler(controller, (Action)(() => fireCount++));

            Tick(controller, 0.5f, true);
            Assert.AreEqual(0, fireCount);

            Tick(controller, 0.5f, true);
            Assert.AreEqual(1, fireCount);

            Tick(controller, 0.4f, true);
            Assert.AreEqual(1, fireCount);

            Tick(controller, 0.6f, false);
            Assert.AreEqual(1, fireCount);
        }

        private static void Tick(object controller, float deltaTime, bool hasTarget)
        {
            controller.GetType()
                .GetMethod("Tick")
                .Invoke(controller, new object[] { deltaTime, hasTarget });
        }
    }
}
