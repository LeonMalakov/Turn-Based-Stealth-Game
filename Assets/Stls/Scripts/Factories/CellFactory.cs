using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Stls
{
    public class CellFactory : ICellFactory
    {
        private const string CellPrefabKey = "Cell";
        private AsyncOperationHandle<GameObject> _handle;
        private readonly DiContainer _diContainer;

        public CellFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public async Task LoadAsync()
        {
            if (!_handle.IsValid())
                _handle = Addressables.LoadAssetAsync<GameObject>(CellPrefabKey);

            if (!_handle.IsDone)
                await _handle.Task;
        }

        public Cell Create(Transform parent, GridCoordinates coords)
        {
            Cell cell = _diContainer.InstantiatePrefabForComponent<Cell>(_handle.Result);

            cell.transform.parent = parent;
            cell.Init(coords);

            return cell;
        }

        public void Reclaim(Cell cell)
        {
            Object.Destroy(cell.gameObject);
        }
    }
}
