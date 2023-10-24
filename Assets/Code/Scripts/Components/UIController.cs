using Game.Managers;
using Game.Utils;
using UnityEngine;

namespace Game.Components
{
    public class UIController : MonoBehaviour
    {
        private const string MAIN_MENU_VIEW_NAME = "MainMenuView";
        private const string GAME_VIEW_NAME = "GameView";
        private const string PAUSE_VIEW_NAME = "PauseView";
        private const string TIME_OVER_VIEW = "TimeOverView";
        private const string LEVEL_FINISH_VIEW = "LevelFinishView";
        private const string LOADING_VIEW_NAME = "LoadingView";

        [Header("Settings")]
        [SerializeField] private AudioClip m_levelFinishClip;
        [SerializeField] private AudioClip m_timeOverClip;

        private void Start()
        {
            var gameManager = GameEventsManager.Instance;
            gameManager.OnMainMenuOpened += OnMainMenuOpened;
            gameManager.OnStarted += OnGameStarted;
            gameManager.OnEnded += OnGameEnded;
            gameManager.OnPaused += OnGamePaused;
            gameManager.OnResumed += OnGameResumed;

            LevelManager.Instance.OnLoadingStarted += OnLoadingStarted;
        }

        private void OnDestroy()
        {
            var gameManager = GameEventsManager.Instance;
            if (gameManager != null)
            {
                gameManager.OnMainMenuOpened -= OnMainMenuOpened;
                gameManager.OnStarted -= OnGameStarted;
                gameManager.OnEnded -= OnGameEnded;
                gameManager.OnPaused -= OnGamePaused;
                gameManager.OnResumed -= OnGameResumed;
            }

            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.OnLoadingStarted -= OnLoadingStarted;
            }
        }

        private void OnMainMenuOpened()
        {
            UiManager.Instance.SetActiveView(MAIN_MENU_VIEW_NAME);
        }

        private void OnGameStarted()
        {
            Cursor.lockState = CursorLockMode.Locked;

            UiManager.Instance.SetActiveView(GAME_VIEW_NAME);
        }

        private void OnGameEnded(GameEndReason reason)
        {
            Cursor.lockState = CursorLockMode.None;

            switch (reason)
            {
                case GameEndReason.LevelFinished:
                    UiManager.Instance.SetActiveView(LEVEL_FINISH_VIEW);
                    UiManager.Instance.PlayClip(m_levelFinishClip);
                    break;
                case GameEndReason.TimeOver:
                    UiManager.Instance.SetActiveView(TIME_OVER_VIEW);
                    UiManager.Instance.PlayClip(m_timeOverClip);
                    break;
            }
        }

        private void OnGamePaused()
        {
            Cursor.lockState = CursorLockMode.None;

            UiManager.Instance.SetActiveView(PAUSE_VIEW_NAME);
        }

        private void OnGameResumed()
        {
            Cursor.lockState = CursorLockMode.Locked;

            UiManager.Instance.SetActiveView(GAME_VIEW_NAME);
        }

        private void OnLoadingStarted(int levelIndex)
        {
            Cursor.lockState = CursorLockMode.None;

            UiManager.Instance.SetActiveView(LOADING_VIEW_NAME);
        }
    }
}
