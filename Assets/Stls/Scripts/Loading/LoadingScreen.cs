using UnityEngine;

namespace Stls
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        private void Awake()
        {
            _canvas.enabled = false;
        }

        public async void Load(params ILoadingOperation[] operations)
        {
            _canvas.enabled = true;

            foreach(ILoadingOperation operation in operations)
            {
                await operation.Load();
            }

            _canvas.enabled = false;
        }
    }
}