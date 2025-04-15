using DevLocker.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueLike.Controllers
{
    public class SceneController
    {
        public static SceneController Global => GameController.SceneController;
        public string previousScene { get; private set; } = "";
        
        public void ChangeScene(int sceneIndex)
        {
            previousScene = SceneManager.GetActiveScene().path;
            SceneManager.LoadScene(sceneIndex);
        }
        
        public void ChangeScene(string sceneName)
        {
            previousScene = SceneManager.GetActiveScene().path;
            SceneManager.LoadScene(sceneName);
        }
        
        public void ChangeScene(SceneReference scene)
        {
            previousScene = SceneManager.GetActiveScene().path;
            
            SceneLoader.LoadScenes(scene);
        }
    }
}