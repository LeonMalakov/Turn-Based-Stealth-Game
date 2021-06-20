using UnityEngine;
using Zenject;

namespace Stls
{
    public class GameMenu : MonoBehaviour
    {
        [SerializeField] private GameoverMenu _gameoverMenu;
        [SerializeField] private TopMenu _topMenu;

        private LoadingScreen _loadingScreen;
        private Game _game;

        [Inject]
        public void Construct(LoadingScreen loadingScreen, Game game)
        {
            _loadingScreen = loadingScreen;
            _game = game;
        }

        private void OnEnable()
        {
            _game.PlayerWon += OnPlayerWon;
            _game.PlayerLost += OnPlayerLoose;

            _topMenu.RestartClicked += Restart;
            _topMenu.ExitClicked += ExitToMenu;
            _gameoverMenu.RestartClicked += Restart;
            _gameoverMenu.ExitClicked += ExitToMenu;
        }

        private void OnDisable()
        {
            _game.PlayerWon -= OnPlayerWon;
            _game.PlayerLost -= OnPlayerLoose;

            _topMenu.RestartClicked -= Restart;
            _topMenu.ExitClicked -= ExitToMenu;
            _gameoverMenu.RestartClicked -= Restart;
            _gameoverMenu.ExitClicked -= ExitToMenu;
        }

        private void OnPlayerWon()
        {
            _topMenu.Hide();
            _gameoverMenu.ShowWin();
        }

        private void OnPlayerLoose()
        {
            _topMenu.Hide();
            _gameoverMenu.ShowLoose();
        }

        private void ExitToMenu()
        {
            _loadingScreen.Load(new MainMenuLoadingOperation());
        }

        private void Restart()
        {
            _loadingScreen.Load(new GameLoadingOperation(_game.Level.LevelData));
        }
    }
}