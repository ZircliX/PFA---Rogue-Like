using System;
using System.Collections;
using System.Collections.Generic;
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

        public int LastLevelIndex { get; private set; }
        
        public int GetNextSceneIndex(int currentScene)
        {
            if (GameDatabase.Global.GetSceneDataFromBuildIndex(currentScene, out SceneData sceneData))
            {
                if (sceneData.Scene.BuildIndex is > 1 and < 8)
                {
                    LastLevelIndex = currentScene;
                    
                    if (sceneData.Scene.BuildIndex is 2 or 3 or 4)
                    {
                        return 1;
                    }
                    
                    return LastLevelIndex + 1;
                }
                if (sceneData.Scene.BuildIndex is 0 or 1)
                {
                    return LastLevelIndex + 1;
                }
            }
            
            return 0;
        }
        
        public void ResetNextSceneIndex()
        {
            LastLevelIndex = 2;
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
            Debug.Log($"Wants to change scene to {sceneIndex}");
            
            FadeUI.Instance.FadeIn(1f).OnComplete(() =>
            {
                SceneManager.LoadScene(sceneIndex);
            });
        }
        
        public void GoToNextLevel(int sceneIndex)
        {
            OnWantsToChangeScene?.Invoke();

            Debug.Log($"From scene {sceneIndex}");
            int index = GetNextSceneIndex(sceneIndex);
            Debug.Log($"Wants to change scene to {index}");
            
            FadeUI.Instance.FadeIn(1f).OnComplete(() =>
            {
                SceneManager.LoadScene(index);
            });
        }
    }
}