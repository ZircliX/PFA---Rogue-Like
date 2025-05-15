using System;
using System.Collections;
using DevLocker.Utils;
using RogueLike.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DeadLink.SceneManagement
{
    public class SceneController
    {
        public static SceneController Global => GameController.SceneController;
        public event Action OnWantsToChangeScene;
        
        public Scene PreviousScene { get; private set; }
        
        public IEnumerator LoadSceneAsync(string sceneName)
        {
            OnWantsToChangeScene?.Invoke();
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            while (op != null && !op.isDone)
                yield return null;
        }
        
        public IEnumerator LoadSceneAsync(int sceneIndex)
        {
            OnWantsToChangeScene?.Invoke();
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);
            while (op != null && !op.isDone)
                yield return null;
        }
        
        public void ChangeScene(int sceneIndex)
        {
            OnWantsToChangeScene?.Invoke();
            PreviousScene = SceneManager.GetActiveScene();
            
            SceneManager.LoadScene(sceneIndex);
        }
        
        public void ChangeScene(string sceneName)
        {
            OnWantsToChangeScene?.Invoke();
            PreviousScene = SceneManager.GetActiveScene();
            
            SceneManager.LoadScene(sceneName);
        }
        
        public void ChangeScene(SceneReference scene)
        {
            OnWantsToChangeScene?.Invoke();
            PreviousScene = SceneManager.GetActiveScene();
            
            SceneManager.LoadScene(scene.BuildIndex);
        }
    }
}