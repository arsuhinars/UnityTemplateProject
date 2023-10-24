using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Managers
{
    public class LevelManager : MonoBehaviour
    {
        [Serializable]
        public class LevelData
        {
            public string SceneName => m_sceneName;
            public Sprite PreviewImage => m_previewImage;

            [SerializeField] private string m_sceneName;
            [SerializeField] private Sprite m_previewImage;
        }

        public event Action<int> OnLoadingStarted;
        public event Action<int> OnLoadingFinished;

        public static LevelManager Instance { get; private set; }

        public int ActiveLevelIndex => m_activeLevelIndex;
        public bool IsLoading => m_isLoading;
        public bool IsLevelLoaded => m_activeLevelIndex != -1;
        public int LevelsCount => m_levels.Length;

        [Header("Settings")]
        [SerializeField] private LevelData[] m_levels;
        [SerializeField] private float m_minLoadingTime = 0f;

        private Coroutine m_activeCoroutine = null;
        private bool m_isLoading = false;
        private int m_activeLevelIndex = -1;

        public LevelData GetLevelData(int levelIndex)
        {
            return m_levels[levelIndex];
        }

        public void LoadLevel(int levelIndex)
        {
            if (m_activeCoroutine != null)
            {
                Debug.LogError("Unable to perform multiple loading operations simultaneously");
                return;
            }

            m_activeCoroutine = StartCoroutine(LoadingCoroutine(levelIndex));
        }

        public void UnloadActiveLevel()
        {
            if (m_activeCoroutine != null)
            {
                Debug.LogError("Unable to perform multiple loading operations simultaneously");
                return;
            }

            m_activeCoroutine = StartCoroutine(LoadingCoroutine(-1));
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private IEnumerator LoadingCoroutine(int levelIndex)
        {
            m_isLoading = true;
            OnLoadingStarted?.Invoke(levelIndex);

            AsyncOperation op;
            float startTime = Time.unscaledTime;

            if (m_activeLevelIndex != -1)
            {
                op = SceneManager.UnloadSceneAsync(m_levels[m_activeLevelIndex].SceneName);
                while (!op.isDone)
                {
                    yield return null;
                }

                m_activeLevelIndex = -1;
            }

            if (levelIndex != -1)
            {
                var sceneName = m_levels[levelIndex].SceneName;
                op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                while (!op.isDone)
                {
                    yield return null;
                }
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

                m_activeLevelIndex = levelIndex;
            }
            
            float t = Mathf.Max(0f, m_minLoadingTime - Time.unscaledTime + startTime);
            yield return new WaitForSecondsRealtime(t);
            yield return null;

            m_activeCoroutine = null;
            m_isLoading = false;
            OnLoadingFinished?.Invoke(levelIndex);

            Resources.UnloadUnusedAssets();
        }
    }
}
