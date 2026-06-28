// 职责：验证建造服务同时检查金币与格子占用。
using System;
using NUnit.Framework;

namespace TowerDefense.Tests
{
    public sealed class BuildServiceTests
    {
        [Test]
        public void BuildsOnlyWhenGoldIsEnoughAndCellIsEmpty()
        {
            Type goldType = Type.GetType("TowerDefense.Core.GoldSystem, TowerDefense");
            Type gridType = Type.GetType("TowerDefense.Core.GridSystem, TowerDefense");
            Type serviceType = Type.GetType("TowerDefense.Core.BuildService, TowerDefense");
            Assert.NotNull(goldType, "GoldSystem type should exist.");
            Assert.NotNull(gridType, "GridSystem type should exist.");
            Assert.NotNull(serviceType, "BuildService type should exist.");

            object gold = Activator.CreateInstance(goldType, 100);
            object grid = Activator.CreateInstance(gridType);
            object service = Activator.CreateInstance(serviceType, gold, grid);

            Assert.True(TryBuild(service, 1.2f, 2.2f, 50));
            Assert.AreEqual(50, goldType.GetProperty("Balance").GetValue(gold));
            Assert.True(IsOccupied(grid, 1.2f, 2.2f));

            Assert.False(TryBuild(service, 1.2f, 2.2f, 30));
            Assert.AreEqual(50, goldType.GetProperty("Balance").GetValue(gold));

            object poorGold = Activator.CreateInstance(goldType, 20);
            object emptyGrid = Activator.CreateInstance(gridType);
            object poorService = Activator.CreateInstance(serviceType, poorGold, emptyGrid);
            Assert.False(TryBuild(poorService, 3f, 3f, 50));
            Assert.AreEqual(20, goldType.GetProperty("Balance").GetValue(poorGold));
            Assert.False(IsOccupied(emptyGrid, 3f, 3f));
        }

        private static bool TryBuild(object service, float x, float y, int cost)
        {
            return (bool)service.GetType()
                .GetMethod("TryBuild")
                .Invoke(service, new object[] { x, y, cost });
        }

        private static bool IsOccupied(object grid, float x, float y)
        {
            object cell = grid.GetType().GetMethod("Align").Invoke(grid, new object[] { x, y });
            return (bool)grid.GetType().GetMethod("IsOccupied").Invoke(grid, new[] { cell });
        }
    }
}
