using Game.Utils;
using System;
using UnityEngine;

namespace Game.Managers
{
    public class GameLogicManager : MonoBehaviour
    {
        public event Action OnOrbCollected;

        public static GameLogicManager Instance { get; private set; }

        public float GameTimer => m_gameTimer;
        public float RemainedTime => m_gameDuration - m_gameTimer;
        public int RemainedOrbs => m_remainedOrbs;
        public int MaxOrbs => m_maxOrbs;

        [SerializeField] private float m_gameDuration = 60f;

        private float m_gameTimer = 0f;
        private int m_maxOrbs;
        private int m_remainedOrbs;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            var gameManager = GameEventsManager.Instance;
            gameManager.OnMainMenuOpened += OnMainMenuOpened;
            gameManager.OnStarted += OnGameStarted;
            gameManager.OnEnded += OnGameEnded;
            gameManager.OnPaused += OnGamePaused;
            gameManager.OnResumed += OnGameResumed;

            var levelManager = LevelManager.Instance;
            levelManager.OnLoadingStarted += OnLoadingStarted;
            levelManager.OnLoadingFinished += OnLoadingFinished;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            var gameManager = GameEventsManager.Instance;
            if (gameManager != null)
            {
                gameManager.OnMainMenuOpened -= OnMainMenuOpened;
                gameManager.OnStarted -= OnGameStarted;
                gameManager.OnEnded -= OnGameEnded;
                gameManager.OnPaused -= OnGamePaused;
                gameManager.OnResumed -= OnGameResumed;
            }

            var levelManager = LevelManager.Instance;
            if (levelManager != null)
            {
                levelManager.OnLoadingStarted -= OnLoadingStarted;
                levelManager.OnLoadingFinished -= OnLoadingFinished;
            }
        }

        private void Update()
        {
            if (GameEventsManager.Instance.IsStarted)
            {
                m_gameTimer += Time.deltaTime;
                if (m_gameTimer > m_gameDuration)
                {
                    GameEventsManager.Instance.EndGame(GameEndReason.TimeOver);
                }
            }
        }

        private void OnMainMenuOpened() { }

        private void OnGameStarted()
        {
            m_gameTimer = 0f;
            m_remainedOrbs = m_maxOrbs;
            OnOrbCollected?.Invoke();

            LevelEntities.Instance.PlayerSpawnPoint.GetPositionAndRotation(
                out var pos, out var rot
            );

            var player = LevelEntities.Instance.Player;
            player.transform.SetPositionAndRotation(pos, rot);
            player.Release();
            player.Spawn();
        }

        private void OnGameEnded(GameEndReason reason)
        {
            LevelEntities.Instance.Player.IsFreezed = true;

            switch (reason)
            {
                case GameEndReason.LevelFinished:
                    int levelIndex = LevelManager.Instance.ActiveLevelIndex;
                    var saveData = SaveManager.Instance.Data;
                    var levelData = SaveManager.Instance.GetLevelData(levelIndex);

                    levelData.isFinished = true;
                    if (float.IsNaN(levelData.recordTime) || m_gameTimer < levelData.recordTime)
                    {
                        levelData.recordTime = m_gameTimer;
                    }

                    saveData.lastAvailableLevelIndex = Mathf.Max(saveData.lastAvailableLevelIndex, levelIndex + 1);

                    SaveManager.Instance.Data = saveData;
                    SaveManager.Instance.SetLevelData(levelIndex, levelData);
                    SaveManager.Instance.Save();

                    break;
            }
        }

        private void OnGamePaused()
        {
            LevelEntities.Instance.Player.IsFreezed = true;
        }

        private void OnGameResumed()
        {
            LevelEntities.Instance.Player.IsFreezed = false;
        }

        private void OnLoadingStarted(int levelIndex)
        {
            if (LevelEntities.Instance != null)
            {
                LevelEntities.Instance.Player.Release();
            }

            if (PoolsManager.Instance != null)
            {
                PoolsManager.Instance.ReleaseAllPools();
            }
        }

        private void OnLoadingFinished(int levelIndex)
        {
            if (levelIndex != -1)
            {
                var orbs = LevelEntities.Instance.Orbs;
                for (int i = 0; i < orbs.Length; i++)
                {
                    orbs[i].OnPickedUp += OnOrbPickedUp;
                }

                m_maxOrbs = orbs.Length;

                GameEventsManager.Instance.StartGame();
            }
            else
            {
                GameEventsManager.Instance.OpenMainMenu();
            }
        }

        private void OnOrbPickedUp(GameObject target)
        {
            m_remainedOrbs--;
            OnOrbCollected?.Invoke();

            if (m_remainedOrbs == 0)
            {
                GameEventsManager.Instance.EndGame(GameEndReason.LevelFinished);
            }
        }
    }
}
