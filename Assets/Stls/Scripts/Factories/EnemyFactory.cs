using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Stls
{
    public class EnemyFactory : IEnemyFactory
    {
        private const string EnemyPrefabKey = "Enemy";
        private AsyncOperationHandle<GameObject> _handle;
        private readonly DiContainer _diContainer;

        public EnemyFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public async Task LoadAsync()
        {
            if (!_handle.IsValid())
                _handle = Addressables.LoadAssetAsync<GameObject>(EnemyPrefabKey);

            if (!_handle.IsDone)
                await _handle.Task;
        }

        public Enemy Create(Transform parent, Cell location, GridDirection direction, Cell[] patrolWaypoints)
        {
            Enemy enemy = _diContainer.InstantiatePrefabForComponent<Enemy>(_handle.Result);

            enemy.transform.parent = parent;
            enemy.Init(location, direction, patrolWaypoints);

            return enemy;
        }

        public void Reclaim(Enemy enemy)
        {
            Object.Destroy(enemy.gameObject);
        }
    }
}