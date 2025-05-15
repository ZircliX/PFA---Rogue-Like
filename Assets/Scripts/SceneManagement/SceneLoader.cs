using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevLocker.Utils;
using RogueLike;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DeadLink.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        public static event Action<string[], string[]> OnSceneLoaded;
        
        public static void LoadScenes(params SceneReference[] scenes)
        {
            //Create Scene
            Scene scene = SceneManager.CreateScene("LoadingScene");

            //Add Object
            SceneLoader sceneLoader = Instantiate(GameMetrics.Global.SceneLoader);
            SceneManager.MoveGameObjectToScene(sceneLoader.gameObject, scene);

            IEnumerator loadScenes = sceneLoader.ILoadScenes(scenes);
            sceneLoader.StartCoroutine(loadScenes);
        }

        private IEnumerator ILoadScenes(params SceneReference[] scenes)
        {
            //Get opened Scenes
            List<Scene> unloadScenes = new List<Scene>();
            Scene loadingScene = gameObject.scene;
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var current = SceneManager.GetSceneAt(i);
                if (current != loadingScene)
                {
                    unloadScenes.Add(current);
                }
            }

            string[] unloadedPaths = unloadScenes.Select(ctx => ctx.path).ToArray();
            string[] loadedPaths = scenes.Select(ctx => ctx.ScenePath).ToArray();
            SceneManager.SetActiveScene(loadingScene);

            //Unload scenes
            foreach (var scene in unloadScenes)
            {
                AsyncOperation op = SceneManager.UnloadSceneAsync(scene);
                
                //Debug.Log($"Beginning unloading : {scene.name}");
                yield return op;
                
                //Debug.Log($"Unloaded {scene.name}");
            }
            
            //Load scenes
            Scene mainScene = default;
            for (int i = 0; i < scenes.Length; i++)
            {
                SceneReference sceneReference = scenes[i];
                //Debug.Log($"Beginning loading : {sceneReference.ScenePath}");
                AsyncOperation op = SceneManager.LoadSceneAsync(sceneReference.BuildIndex, LoadSceneMode.Additive);
                yield return op;
                
                
                //Debug.Log($"Loaded : {sceneReference.ScenePath}");
                if (!mainScene.IsValid())
                {
                    mainScene = SceneManager.GetSceneByPath(sceneReference.ScenePath);
                }
            }

            SceneManager.SetActiveScene(mainScene);

            GameObject[] objects = gameObject.scene.GetRootGameObjects();
            for (int i = 0; i < objects.Length; i++)
            {
                Debug.Log(objects[i].name);
                SceneManager.MoveGameObjectToScene(objects[i], mainScene);
            }
            yield return SceneManager.UnloadSceneAsync(loadingScene);
            Destroy(gameObject);
            
            OnSceneLoaded?.Invoke(unloadedPaths, loadedPaths);
        }
    }
}