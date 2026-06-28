// 职责：验证建造网格坐标对齐与占用状态。
using System;
using NUnit.Framework;

namespace TowerDefense.Tests
{
    public sealed class GridSystemTests
    {
        [Test]
        public void AlignsCoordinatesAndTracksOccupancy()
        {
            Type gridType = Type.GetType("TowerDefense.Core.GridSystem, TowerDefense");
            Assert.NotNull(gridType, "GridSystem type should exist.");

            object grid = Activator.CreateInstance(gridType);
            object cell = gridType.GetMethod("Align").Invoke(grid, new object[] { 2.4f, 3.6f });
            Assert.AreEqual(2, cell.GetType().GetProperty("X").GetValue(cell));
            Assert.AreEqual(4, cell.GetType().GetProperty("Y").GetValue(cell));
            Assert.False((bool)gridType.GetMethod("IsOccupied").Invoke(grid, new object[] { cell }));

            gridType.GetMethod("SetOccupied").Invoke(grid, new object[] { cell, true });
            Assert.True((bool)gridType.GetMethod("IsOccupied").Invoke(grid, new object[] { cell }));

            gridType.GetMethod("SetOccupied").Invoke(grid, new object[] { cell, false });
            Assert.False((bool)gridType.GetMethod("IsOccupied").Invoke(grid, new object[] { cell }));
        }
    }
}
