// 职责：验证路径跟随器沿路径点移动并在终点触发事件。
using System;
using NUnit.Framework;

namespace TowerDefense.Tests
{
    public sealed class PathFollowerTests
    {
        [Test]
        public void MovesAlongWaypointsAndRaisesEndEvent()
        {
            Type waypointType = Type.GetType("TowerDefense.Enemy.Waypoint, TowerDefense");
            Type followerType = Type.GetType("TowerDefense.Enemy.PathFollower, TowerDefense");
            Assert.NotNull(waypointType, "Waypoint type should exist.");
            Assert.NotNull(followerType, "PathFollower type should exist.");

            Array points = Array.CreateInstance(waypointType, 3);
            points.SetValue(Activator.CreateInstance(waypointType, 0f, 0f), 0);
            points.SetValue(Activator.CreateInstance(waypointType, 2f, 0f), 1);
            points.SetValue(Activator.CreateInstance(waypointType, 2f, 1f), 2);

            object follower = Activator.CreateInstance(followerType, points);
            int reachedCount = 0;
            followerType.GetEvent("OnReachedEnd").AddEventHandler(follower, (Action)(() => reachedCount++));

            Tick(follower, 1f);
            AssertPosition(follower, 1f, 0f);
            Assert.AreEqual(0, reachedCount);

            Tick(follower, 1f);
            AssertPosition(follower, 2f, 0f);
            Assert.AreEqual(0, reachedCount);

            Tick(follower, 1f);
            AssertPosition(follower, 2f, 1f);
            Assert.True((bool)followerType.GetProperty("HasReachedEnd").GetValue(follower));
            Assert.AreEqual(1, reachedCount);

            Tick(follower, 1f);
            Assert.AreEqual(1, reachedCount);
        }

        [Test]
        public void MovementScalesWithSpeedAndDeltaTime()
        {
            Type waypointType = Type.GetType("TowerDefense.Enemy.Waypoint, TowerDefense");
            Type followerType = Type.GetType("TowerDefense.Enemy.PathFollower, TowerDefense");
            Assert.NotNull(waypointType, "Waypoint type should exist.");
            Assert.NotNull(followerType, "PathFollower type should exist.");

            Array points = Array.CreateInstance(waypointType, 2);
            points.SetValue(Activator.CreateInstance(waypointType, 0f, 0f), 0);
            points.SetValue(Activator.CreateInstance(waypointType, 10f, 0f), 1);

            object follower = Activator.CreateInstance(followerType, points, 2f);
            Tick(follower, 0.5f);
            AssertPosition(follower, 1f, 0f);

            Tick(follower, 1.5f);
            AssertPosition(follower, 4f, 0f);
        }

        private static void Tick(object follower, float deltaTime)
        {
            follower.GetType().GetMethod("Tick").Invoke(follower, new object[] { deltaTime });
        }

        private static void AssertPosition(object follower, float x, float y)
        {
            Type type = follower.GetType();
            Assert.AreEqual(x, (float)type.GetProperty("X").GetValue(follower), 0.001f);
            Assert.AreEqual(y, (float)type.GetProperty("Y").GetValue(follower), 0.001f);
        }
    }
}
