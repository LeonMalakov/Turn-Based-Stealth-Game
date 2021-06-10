using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Stls.Tests
{
    public class Setup
    {
        public class CellFactoryMock : ICellFactory
        {
            public Cell Create(Transform parent, GridCoordinates coords)
            {
                Cell cell = new GameObject("Cell").AddComponent<Cell>();
                cell.Init(coords);
                return cell;
            }
            public Task LoadAsync() => Task.Delay(0);
            public void Reclaim(Cell cell) { }
        }

        public static Stls.Grid Grid(int width, int height)
        {
            Stls.Grid grid = new GameObject("Grid").AddComponent<Stls.Grid>();
            grid.Construct(new CellFactoryMock());
            grid.CreateAsync(width, height).Wait();
            return grid;
        }

        public static EnemyGroup EnemyGroup(List<Enemy> enemies)
        {
            EnemyGroup enemyGroup = new GameObject("EnemyGroup").AddComponent<EnemyGroup>();
            enemyGroup.Setup(enemies);
            return enemyGroup;
        }

        public static Enemy Enemy(Cell location, GridDirection direction, EnemyConfig enemyConfig, Cell[] patrolWaypoints = null)
        {
            Enemy enemy = new GameObject("Enemy").AddComponent<Enemy>();
            enemy.Init(location, direction, patrolWaypoints);
            enemy.Config = enemyConfig;
            return enemy;
        }

        public static Player Player(Cell location, GridDirection direction)
        {
            Player player = new GameObject("Player").AddComponent<Player>();
            player.Init(location, direction);
            return player;
        }
    }
}