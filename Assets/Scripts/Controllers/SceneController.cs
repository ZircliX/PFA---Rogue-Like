using System;
using DevLocker.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueLike.Controllers
{
    public class SceneController
    {
        public event Action OnWantsToChangeScene;
        
        public static SceneController Global => GameController.SceneController;
        public Scene previousScene { get; private set; }
        
        public void ChangeScene(int sceneIndex)
        {
            OnWantsToChangeScene?.Invoke();
            previousScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(sceneIndex);
        }
        
        public void ChangeScene(string sceneName)
        {
            OnWantsToChangeScene?.Invoke();
            previousScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(sceneName);
        }
        
        public void ChangeScene(SceneReference scene)
        {
            OnWantsToChangeScene?.Invoke();
            previousScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.BuildIndex);
            //SceneLoader.LoadScenes(scene);
        }
    }
}