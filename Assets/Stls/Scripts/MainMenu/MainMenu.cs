using UnityEngine;
using Zenject;

namespace Stls
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private LevelButton _playButton;

        private LoadingScreen _loadingScreen;

        [Inject]
        public void Construct(LoadingScreen loadingScreen)
        {
            _loadingScreen = loadingScreen;
        }

        private void OnEnable()
        {
            _playButton.Clicked += StartGame;
        }

        private void OnDisable()
        {
            _playButton.Clicked -= StartGame;
        }

        public void StartGame(LevelData levelData)
        {
            _loadingScreen.Load(new GameLoadingOperation(levelData));
        }
    }
}