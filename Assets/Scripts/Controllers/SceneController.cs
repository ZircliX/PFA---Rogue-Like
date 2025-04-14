using DevLocker.Utils;
using UnityEngine.SceneManagement;

namespace RogueLike.Controllers
{
    public class SceneController
    {
        public static SceneController Global => GameController.SceneController;
        
        public void ChangeScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }
        
        public void ChangeScene(SceneReference scene)
        {
            SceneManager.LoadScene(scene.BuildIndex);
        }
        
        public void ChangeScene(int scene)
        {
            SceneManager.LoadScene(scene);
        }
    }
}