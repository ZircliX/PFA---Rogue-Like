using System;
using System.Collections;
using DeadLink.Misc;
using DevLocker.Utils;
using DG.Tweening;
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
        public void ChangeScene(string sceneName)
        {
            OnWantsToChangeScene?.Invoke();
            PreviousScene = SceneManager.GetActiveScene();
            
            SceneManager.LoadScene(sceneName);
        }

        public void ChangeScene(SceneData scene)
        {
            ChangeScene(scene.Scene);
        }
        
        public void ChangeScene(SceneReference scene)
        {
            ChangeScene(scene.BuildIndex);
        }
        
        public void ChangeScene(int sceneIndex)
        {
            OnWantsToChangeScene?.Invoke();
            PreviousScene = SceneManager.GetActiveScene();
            FadeUI.Instance.FadeIn(1f).OnComplete(() =>
            {
                SceneManager.LoadScene(sceneIndex);
            });
        }
    }
}