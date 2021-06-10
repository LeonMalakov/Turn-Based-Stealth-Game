using NUnit.Framework;

namespace Stls.Tests
{
    public class GridVisibilityCheckerTests
    {
        [Test]
        public void WhenTargetInFrontOfObserver_AndNoObstacles_ThenTargetShouldBeVisible()
        {
            // Arrange.
            GridCoordinates observerPosition = new GridCoordinates(0, 0);
            GridDirection viewDirection = GridDirection.ForwardRight;
            int viewRange = 6;
            GridCoordinates targetPosition = new GridCoordinates(0, 6);

            Stls.Grid grid = Setup.Grid(10, 10);

            // Act.
            bool isVisible = GridVisibilityChecker.IsCellVisible(observerPosition, viewDirection, viewRange, targetPosition, grid);

            // Assert.
            Assert.AreEqual(
                new { IsTargetVisible = true },
                new { IsTargetVisible = isVisible });
        }

        [Test]
        public void WhenTargetInFrontOfObserver_AndObstacleIsBetween_ThenTargetShouldBeInvisible()
        {
            // Arrange.
            GridCoordinates observerPosition = new GridCoordinates(0, 0);
            GridDirection viewDirection = GridDirection.ForwardRight;
            int viewRange = 6;
            GridCoordinates targetPosition = new GridCoordinates(0, 6);

            Stls.Grid grid = Setup.Grid(10, 10);
            grid.GetCell(new GridCoordinates(0, 3)).SetIsWalkable(false);

            // Act.
            bool isVisible = GridVisibilityChecker.IsCellVisible(observerPosition, viewDirection, viewRange, targetPosition, grid);

            // Assert.
            Assert.AreEqual(
                new { IsTargetVisible = false },
                new { IsTargetVisible = isVisible });
        }
    }
}