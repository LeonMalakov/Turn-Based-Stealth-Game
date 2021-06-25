using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Stls
{
    public class Level : MonoBehaviour
    {
        private IPlayerFactory _playerFactory;
        private IEnemyFactory _enemyFactory;

        public Player Player { get; private set; }
        public Grid Grid { get; private set; }

        public EnemyGroup EnemyGroup { get; private set; }
        public LevelData LevelData { get; private set; }

        private Transform _transform;

        public bool IsCreated { get; private set; }
        public event Action Created;

        [Inject]
        public void Construct(IPlayerFactory playerFactory, IEnemyFactory enemyFactory, Grid grid)
        {
            _playerFactory = playerFactory;
            _enemyFactory = enemyFactory;
            Grid = grid;
        }

        public async Task CreateAsync(LevelData data)
        {
            if (IsCreated)
                throw new InvalidOperationException("Level already created.");

            _transform = transform;

            LevelData = data;

            await CreateGridAsync(data);

            await Task.WhenAll(
                CreatePlayerAsync(data),
                CreateEnemiesAsync(data)
                );

            IsCreated = true;
            Created?.Invoke();
        }


        private async Task CreateGridAsync(LevelData data)
        {
            await Grid.CreateAsync(data.GridWidth, data.GridHeight);
            
            foreach (GridCoordinates coords in data.NotWalkableCells)
                Grid.GetCell(coords).SetIsWalkable(false);
        }

        private async Task CreatePlayerAsync(LevelData data)
        {
            await _playerFactory.LoadAsync();

            Player = _playerFactory.Create(_transform, Grid.GetCell(data.PlayerPosition), data.PlayerDirection);
        }

        private async Task CreateEnemiesAsync(LevelData data)
        {
            await _enemyFactory.LoadAsync();
            
            GameObject groupsGO = new GameObject("Enemy Groups");
            EnemyGroup = groupsGO.AddComponent<EnemyGroup>();
            EnemyGroup.transform.parent = _transform;

            List<Enemy> enemies = new List<Enemy>();

            foreach (LevelData.EnemyData enemyData in data.Enemies)
            {
                enemies.Add(CreateEnemy(enemyData));
            }
            EnemyGroup.Setup(enemies);
        }


        private Enemy CreateEnemy(LevelData.EnemyData enemyData)
        {
            Cell[] patrolWaypoints = enemyData.PatrolWaypoints.Select(w => Grid.GetCell(w)).ToArray();

            Enemy enemy = _enemyFactory.Create(_transform, Grid.GetCell(enemyData.Position), enemyData.Direction, patrolWaypoints);
            return enemy;
        }
    }
}