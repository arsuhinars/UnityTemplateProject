using Game.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Views
{
    public class PauseView : BaseUIView
    {
        [SerializeField] private Button m_continueButton;
        [SerializeField] private Button m_restartButton;
        [SerializeField] private Button m_quitButton;

        private void Start()
        {
            m_continueButton.onClick.AddListener(OnContinueButtonClick);
            m_restartButton.onClick.AddListener(OnRestartButtonClick);
            m_quitButton.onClick.AddListener(OnQuitButtonClick);
        }

        private void OnContinueButtonClick()
        {
            if (GameEventsManager.Instance != null)
            {
                GameEventsManager.Instance.ResumeGame();
            }
        }

        private void OnRestartButtonClick()
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
    }
}
