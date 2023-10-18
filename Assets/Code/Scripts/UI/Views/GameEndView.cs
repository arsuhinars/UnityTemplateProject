using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Views
{
    public class GameEndView : UiView
    {
        [SerializeField] private Button m_nextLevelButton;
        [SerializeField] private Button m_retryButton;
        [SerializeField] private Button m_quitButton;
        [Space]
        [SerializeField] private TextMeshProUGUI m_timeText;
        [SerializeField] private TextMeshProUGUI m_recordText;

        protected override void OnShowed()
        {
            UpdateElements();
        }

        protected override void OnHidden() { }

        private void Start()
        {
            if (m_nextLevelButton != null)
            {
                m_nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
            }

            if (m_retryButton != null)
            {
                m_retryButton.onClick.AddListener(OnRetryButtonClick);
            }

            m_quitButton.onClick.AddListener(OnQuitButtonClick);
        }

        private void OnNextLevelButtonClick()
        {
            int lvlIdx = LevelManager.Instance.ActiveLevelIndex;
            int lvlCount = LevelManager.Instance.LevelsCount;

            if (lvlIdx + 1 < lvlCount)
            {
                LevelManager.Instance.LoadLevel(lvlIdx + 1);
            }
        }

        private void OnRetryButtonClick()
        {
            if (GameEventsManager.Instance != null)
            {
                GameEventsManager.Instance.StartGame();
            }
        }

        private void OnQuitButtonClick()
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.UnloadActiveLevel();
            }
        }

        private void UpdateElements()
        {
            if (m_timeText != null)
            {
                m_timeText.SetText("{0:0.0} s.", GameLogicManager.Instance.GameTimer);
            }

            if (m_recordText != null)
            {
                float recordTime = SaveManager.Instance.Data.GetLevelRecord(
                    LevelManager.Instance.ActiveLevelIndex
                );

                m_recordText.SetText("{0:0.0} s.", recordTime);
            }

            if (m_nextLevelButton != null)
            {
                int lvlIdx = LevelManager.Instance.ActiveLevelIndex;
                int lvlCount = LevelManager.Instance.LevelsCount;
                m_nextLevelButton.interactable = lvlIdx + 1 < lvlCount;
            }
        }
    }
}
