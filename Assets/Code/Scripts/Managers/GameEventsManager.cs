using System;
using System.Collections;
using UnityEngine;

namespace Game.Managers
{
    public class GameEventsManager : MonoBehaviour
    {
        public enum GameState
        {
            None, MainMenu, Started, Ended, Paused
        }

        public enum GameEndReason
        {
            LevelFinished, TimeOver
        }

        public event Action OnMainMenuOpened;
        public event Action OnStarted;
        public event Action<GameEndReason> OnEnded;
        public event Action OnPaused;
        public event Action OnResumed;

        public static GameEventsManager Instance { get; private set; }

        public bool IsStarted => m_state == GameState.Started;
        public bool IsPaused => m_state == GameState.Paused;
        public GameState State => m_state;

        private GameState m_state;

        public void OpenMainMenu()
        {
            m_state = GameState.MainMenu;
            Time.timeScale = 1f;

            OnMainMenuOpened?.Invoke();
        }

        public void StartGame()
        {
            m_state = GameState.Started;
            Time.timeScale = 1f;

            OnStarted?.Invoke();
        }

        public void EndGame(GameEndReason reason)
        {
            if (m_state != GameState.Started && m_state != GameState.Paused)
            {
                return;
            }

            m_state = GameState.Ended;
            Time.timeScale = 1f;

            OnEnded?.Invoke(reason);
        }

        public void PauseGame()
        {
            if (m_state != GameState.Started)
            {
                return;
            }

            m_state = GameState.Paused;
            Time.timeScale = 0f;

            OnPaused?.Invoke();
        }

        public void ResumeGame()
        {
            if (m_state != GameState.Paused)
            {
                return;
            }

            m_state = GameState.Started;
            Time.timeScale = 1f;

            OnResumed?.Invoke();
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private IEnumerator Start()
        {
            yield return null;
            OpenMainMenu();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
