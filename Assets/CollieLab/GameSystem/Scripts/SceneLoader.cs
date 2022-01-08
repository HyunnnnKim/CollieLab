using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CollieLab.GameSystems
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        #region Serialized Field
        [Header("Scene Change Events")]
        [SerializeField] private UnityEvent OnLoadBegin = new UnityEvent();
        [SerializeField] private UnityEvent OnLoadEnd = new UnityEvent();

        [Header("Load Settings")]
        [SerializeField] private ScreenFader screenFader = null;
        #endregion

        #region Private Field
        private bool isLoading = false;
        #endregion

        private void Awake()
        {
            SceneManager.sceneLoaded += SetActiveScene;

            LoadFirstScene();
        }

        #region Initialize
        /// <summary>
        /// Load build index 1 scene as additive.
        /// </summary>
        private void LoadFirstScene()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Additive);
        }
        #endregion

        #region Subscribers
        /// <summary>
        /// Activate loaded scene.
        /// </summary>
        private void SetActiveScene(Scene scene, LoadSceneMode mode)
        {
            SceneManager.SetActiveScene(scene);
        }
        #endregion

        #region Scene Controls
        /// <summary>
        /// Unload the current scene and load the new scene.
        /// </summary>
        public void LoadNewScene(string sceneName)
        {
            if (!isLoading)
            {
                StartCoroutine(LoadScene(sceneName));
            }
        }

        private IEnumerator LoadScene(string sceneName)
        {
            isLoading = true;

            OnLoadBegin?.Invoke();
            yield return screenFader.FadeIn();

            yield return StartCoroutine(UnloadCurrent());

            yield return new WaitForSeconds(1f); // This is for testing.

            yield return StartCoroutine(LoadNew(sceneName));

            yield return screenFader.FadeOut();
            OnLoadEnd?.Invoke();

            isLoading = false;
        }

        private IEnumerator UnloadCurrent()
        {
            AsyncOperation unloadAsyncOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            while (!unloadAsyncOperation.isDone)
            {
                float progress = Mathf.Clamp01(unloadAsyncOperation.progress / 0.9f);
                yield return null;
            }
        }

        private IEnumerator LoadNew(string sceneName)
        {
            AsyncOperation loadAsyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!loadAsyncOperation.isDone)
            {
                float progress = Mathf.Clamp01(loadAsyncOperation.progress / 0.9f);
                yield return null;
            }
        }
        #endregion

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= SetActiveScene;
        }
    }
}
