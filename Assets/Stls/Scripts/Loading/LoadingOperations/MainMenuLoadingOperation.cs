using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Stls
{
    public class MainMenuLoadingOperation : ILoadingOperation
    {
        public async Task Load()
        {
            await LoadScene();
        }

        private static async Task LoadScene()
        {
            var operation = SceneManager.LoadSceneAsync(Constants.Scenes.MainMenu, LoadSceneMode.Single);

            while (operation.isDone == false)
                await Task.Delay(1);
        }
    }
}