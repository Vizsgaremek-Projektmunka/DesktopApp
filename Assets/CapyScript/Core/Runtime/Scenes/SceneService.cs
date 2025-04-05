using CapyScript.Threads;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CapyScript.Scenes
{
    [AddComponentMenu("CapyScript/Services/Scene Service")]
    public class SceneService : SingletonMonoBehaviour<SceneService>
    {
        static Coroutine activeLoading;

        public static void Init()
        {
            if (!Exists)
            {
                GameObject serviceObject = new GameObject("Scene Service");
                serviceObject.AddComponent<SceneService>();
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            activeLoading = null;
        }

        public static bool SoftLoad(string loadingScene, string sceneToLoad)
        {
            Init();
            
            if (activeLoading == null) 
            {
                activeLoading = Instance.StartCoroutine(SoftLoadAsync(Instance.gameObject.scene.name, loadingScene, sceneToLoad));
                return true;
            }
            else
            {
                return false;
            }
        }

        static IEnumerator SoftLoadAsync(string startScene, string loadingScene, string sceneToLoad)
        {
            yield return SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Additive);
            yield return SceneManager.UnloadSceneAsync(startScene);
            yield return SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            yield return SceneManager.UnloadSceneAsync(loadingScene);
            activeLoading = null;
        }

        public static bool HardLoad(string loadingScene, string sceneToLoad)
        {
            Init();

            if (activeLoading == null)
            {
                activeLoading = Instance.StartCoroutine(HardLoadAsync(loadingScene, sceneToLoad));
                return true;
            }
            else
            {
                return false;
            }
        }

        static IEnumerator HardLoadAsync(string loadingScene, string sceneToLoad)
        {
            yield return SceneManager.LoadSceneAsync(loadingScene);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            activeLoading = null;
        }
    }
}
