using System.Collections.Generic;
using System.Linq;

namespace Stls
{
    public static class EnemiesSightChecker
    {
        public static void CheckTargetsVisibility(Player player, EnemyGroup enemyGroup, Grid grid)
        {
            var deadBodies = enemyGroup.Enemies.Where(e => !e.IsAlive && e.Location != null);

            foreach (Enemy enemy in enemyGroup.Enemies)
            {
                if (!enemy.IsAlive)
                    continue;

                CheckPlayerVisibility(enemy, player, grid);

                CheckDeadBodiesVisibility(enemy, deadBodies, grid);
            }
        }
        private static void CheckPlayerVisibility(Enemy enemy, Player player, Grid grid)
        {
            if (GridVisibilityChecker.IsCellVisible(enemy.Location.Position, enemy.Direction, enemy.Config.ViewRange, player.Location.Position, grid))
            {
                bool isInAttackRange = GridVisibilityChecker.IsCellInRange(enemy.Location.Position, enemy.Config.AttackRange, player.Location.Position);

                enemy.SeeHero(player, isInAttackRange);
            }
        }

        private static void CheckDeadBodiesVisibility(Enemy enemy, IEnumerable<Enemy> deadBodies, Grid grid)
        {
            foreach (Enemy deadBody in deadBodies)
            {
                if (GridVisibilityChecker.IsCellVisible(enemy.Location.Position, enemy.Direction, enemy.Config.ViewRange, deadBody.Location.Position, grid))
                {
                    enemy.SeeDeadBody(deadBody);
                }
            }
        }


        public static void CheckCellsVisibility(EnemyGroup enemyGroup, Grid grid)
        {
            foreach (Cell cell in grid.Cells)
            {
                CheckCellVisibility(cell, enemyGroup, grid);
            }
        }

        private static void CheckCellVisibility(Cell cell, EnemyGroup enemyGroup, Grid grid)
        {
            bool isVisible = false;

            foreach (Enemy enemy in enemyGroup.Enemies)
            {
                if (!enemy.IsAlive)
                    continue;

                if (GridVisibilityChecker.IsCellVisible(enemy.Location.Position, enemy.Direction, enemy.Config.ViewRange, cell.Position, grid))
                {
                    isVisible = true;
                    bool isInAttackRange = GridVisibilityChecker.IsCellInRange(enemy.Location.Position, enemy.Config.AttackRange, cell.Position);

                    if (isInAttackRange)
                    {
                        cell.SetVisibilityState(isVisible, isInAttackRange);
                        return;
                    }
                }
            }

            cell.SetVisibilityState(isVisible, false);
        }
    }
}