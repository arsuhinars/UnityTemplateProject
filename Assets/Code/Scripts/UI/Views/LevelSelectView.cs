using Game.Managers;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Views
{
    public class LevelSelectView : BaseUIView
    {
        [SerializeField] private Button m_previousButton;
        [SerializeField] private Button m_nextButton;
        [SerializeField] private Button m_playButton;
        [Space]
        [SerializeField] private Image m_previewImage;
        [SerializeField] private Image m_checkmark;
        [SerializeField] private TextMeshProUGUI m_levelText;
        [SerializeField] private TextMeshProUGUI m_recordText;

        private int m_selectedLevelIndex = 0;

        protected override void OnShowed()
        {
            StartCoroutine(UpdateElements());
        }

        private void Start()
        {
            m_previousButton.onClick.AddListener(OnPreviousButtonClick);
            m_nextButton.onClick.AddListener(OnNextButtonClick);
            m_playButton.onClick.AddListener(OnPlayButtonClick);
        }

        private IEnumerator UpdateElements()
        {
            yield return null;

            var levelData = LevelManager.Instance.GetLevelData(m_selectedLevelIndex);
            var saveData = SaveManager.Instance.Data;

            m_previewImage.sprite = levelData.PreviewImage;
            m_levelText.text = $"Level {m_selectedLevelIndex + 1}";
            m_recordText.text = $"Best time - {saveData.GetLevelRecord(m_selectedLevelIndex):0.0} s.";

            m_previousButton.interactable = m_selectedLevelIndex > 0;
            m_nextButton.interactable = (m_selectedLevelIndex + 1) < LevelManager.Instance.LevelsCount;
            m_playButton.interactable = m_selectedLevelIndex <= saveData.MaxPassedLevelIndex + 1;

            m_checkmark.gameObject.SetActive(m_selectedLevelIndex <= saveData.MaxPassedLevelIndex);
        }

        private void OnPreviousButtonClick()
        {
            if (m_selectedLevelIndex > 0)
            {
                m_selectedLevelIndex--;
                StartCoroutine(UpdateElements());
            }
        }

        private void OnNextButtonClick()
        {
            if (m_selectedLevelIndex + 1 < LevelManager.Instance.LevelsCount)
            {
                m_selectedLevelIndex++;
                StartCoroutine(UpdateElements());
            }
        }

        private void OnPlayButtonClick()
        {
            LevelManager.Instance.LoadLevel(m_selectedLevelIndex);
        }
    }
}
