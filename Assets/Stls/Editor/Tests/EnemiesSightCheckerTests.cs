using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Stls.Tests
{
    public class EnemiesSightCheckerTests
    {
        private static void Arrange(int viewRange, int attackRange, out Grid grid, out Player player, out Enemy enemy, out EnemyGroup enemyGroup)
        {
            grid = Setup.Grid(10, 10);
            player = Setup.Player(grid.GetCell(new GridCoordinates(0, 6)), GridDirection.ForwardRight);
            enemy = Setup.Enemy(
                grid.GetCell(new GridCoordinates(0, 0)),
                GridDirection.ForwardRight,
                new EnemyConfig() { ViewRange = viewRange, AttackRange = attackRange });
            enemyGroup = Setup.EnemyGroup(new List<Enemy>() { enemy });
        }

        [Test]
        public void WhenPlayerInViewRangeOfEnemy_AndNoObstacles_ThenEnemyKnownPlayerShouldBeEqualsToPlayer()
        {
            // Arrange.            
            Arrange(
                viewRange: 8,
                attackRange: 3,
                out Grid grid,
                out Player player,
                out Enemy enemy,
                out EnemyGroup enemyGroup);

            // Act.
            EnemiesSightChecker.CheckTargetsVisibility(player, enemyGroup, grid);

            // Assert.
            Assert.AreEqual(
            new { Enemy_KnownPlayer = player },
            new { Enemy_KnownPlayer = enemy.KnownPlayer });
        }

        [Test]
        public void WhenPlayerOutOfViewRangeOfEnemy_AndNoObstacles_ThenEnemyKnownPlayerShouldBeNull()
        {
            // Arrange.            
            Arrange(
                viewRange: 3,
                attackRange: 3,
                out Grid grid,
                out Player player,
                out Enemy enemy,
                out EnemyGroup enemyGroup);

            // Act.
            EnemiesSightChecker.CheckTargetsVisibility(player, enemyGroup, grid);

            // Assert.
            Assert.IsNull(enemy.KnownPlayer);
        }

        [Test]
        public void WhenPlayerInViewRangeOfEnemy_AndObstacleBetweenThem_ThenEnemyKnownPlayerShouldBeNull()
        {
            // Arrange.            
            Arrange(
                viewRange: 8,
                attackRange: 3,
                out Grid grid,
                out Player player,
                out Enemy enemy,
                out EnemyGroup enemyGroup);
            grid.GetCell(new GridCoordinates(0, 3)).SetIsWalkable(false);

            // Act.
            EnemiesSightChecker.CheckTargetsVisibility(player, enemyGroup, grid);

            // Assert.
            Assert.IsNull(enemy.KnownPlayer);
        }

        [Test]
        public void WhenPlayerInAttackRangeOfEnemy_AndNoObstacles_ThenEnemyIsPlayerInAttackRangeShouldBeTrue()
        {
            // Arrange.
            Arrange(
                viewRange: 8,
                attackRange: 6,
                out Grid grid,
                out Player player,
                out Enemy enemy,
                out EnemyGroup enemyGroup);

            // Act.
            EnemiesSightChecker.CheckTargetsVisibility(player, enemyGroup, grid);

            // Assert.
            Assert.AreEqual(
            new { Enemy_IsPlayerInAttackRange = true },
            new { Enemy_IsPlayerInAttackRange = enemy.IsPlayerInAttackRange });
        }

        [Test]
        public void WhenPlayerOutOfAttackRangeOfEnemy_AndNoObstacles_ThenEnemyIsPlayerInAttackRangeShouldBeFalse()
        {
            // Arrange.
            Arrange(
                viewRange: 8,
                attackRange: 3,
                out Grid grid,
                out Player player,
                out Enemy enemy,
                out EnemyGroup enemyGroup);

            // Act.
            EnemiesSightChecker.CheckTargetsVisibility(player, enemyGroup, grid);

            // Assert.
            Assert.AreEqual(
            new { Enemy_IsPlayerInAttackRange = false },
            new { Enemy_IsPlayerInAttackRange = enemy.IsPlayerInAttackRange });
        }
    }
}