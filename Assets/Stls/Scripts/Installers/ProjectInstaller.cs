using UnityEngine;
using Zenject;

namespace Stls
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private LoadingScreen _loadingScreenPrefab;

        public override void InstallBindings()
        {
            BindLoadingScreen();
        }

        private void BindLoadingScreen()
        {
            Container
                .Bind<LoadingScreen>()
                .FromComponentInNewPrefab(_loadingScreenPrefab)
                .AsSingle();
        }
    }
}