using System.Linq;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Stls
{
    public class GameLoadingOperation : ILoadingOperation
    {
        private readonly LevelData _levelData;

        public GameLoadingOperation(LevelData levelData)
        {
            _levelData = levelData;
        }

        public async Task Load()
        {
            await LoadScene();
            await CreateGame(_levelData);
        }

        private static async Task LoadScene()
        {
            var operation = SceneManager.LoadSceneAsync(Constants.Scenes.Game, LoadSceneMode.Single);

            while (operation.isDone == false)
                await Task.Delay(1);
        }

        private static async Task CreateGame(LevelData levelData)
        {
            var game = GetLevelInstaller().Game;
            await game.InitAsync(levelData);
        }

        private static LevelInstaller GetLevelInstaller()
        {
            return SceneManager
                .GetActiveScene()
                .GetRootGameObjects()
                .First(x => x.GetComponent<LevelInstaller>())
                .GetComponent<LevelInstaller>();
        }
    }
}
