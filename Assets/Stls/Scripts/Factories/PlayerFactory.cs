using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Stls
{
    public class PlayerFactory : IPlayerFactory
    {
        private const string PlayerPrefabKey = "Player";
        private AsyncOperationHandle<GameObject> _handle;
        private readonly DiContainer _diContainer;

        public PlayerFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public async Task LoadAsync()
        {
            if (!_handle.IsValid())
                _handle = Addressables.LoadAssetAsync<GameObject>(PlayerPrefabKey);

            if (!_handle.IsDone)
                await _handle.Task;
        }

        public Player Create(Transform parent, Cell location, GridDirection direction)
        {
            Player player = _diContainer.InstantiatePrefabForComponent<Player>(_handle.Result);

            player.transform.parent = parent;
            player.Init(location, direction);

            return player;
        }

        public void Reclaim(Player player)
        {
            Object.Destroy(player.gameObject);
        }
    }
}