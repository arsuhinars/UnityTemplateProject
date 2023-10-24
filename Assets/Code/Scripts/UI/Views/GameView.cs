using Game.Managers;
using System.Text;
using TMPro;
using UnityEngine;

namespace Game.UI.Views
{
    public class GameView : UIView
    {
        [SerializeField] private TextMeshProUGUI m_timerText;
        [SerializeField] private TextMeshProUGUI m_orbsCounterText;

        private readonly StringBuilder m_timerTextBuilder = new();
        private readonly StringBuilder m_orbsCounterTextBuilder = new();

        protected override void OnShowed() { }

        protected override void OnHidden() { }

        private void Start()
        {
            GameLogicManager.Instance.OnOrbCollected += UpdateOrbCounter;
        }

        private void OnDestroy()
        {
            if (GameLogicManager.Instance != null)
            {
                GameLogicManager.Instance.OnOrbCollected -= UpdateOrbCounter;
            }
        }

        private void Update()
        {
            if (GameEventsManager.Instance.IsStarted)
            {
                var time = GameLogicManager.Instance.RemainedTime;

                m_timerTextBuilder.Clear();
                m_timerTextBuilder.AppendFormat("{0:00}", Mathf.FloorToInt(time / 60f));
                m_timerTextBuilder.Append(':');
                m_timerTextBuilder.AppendFormat("{0:00}", Mathf.FloorToInt(time % 60f));

                m_timerText.SetText(m_timerTextBuilder);
            }
        }

        private void UpdateOrbCounter()
        {
            m_orbsCounterTextBuilder.Clear();
            m_orbsCounterTextBuilder.Append(GameLogicManager.Instance.RemainedOrbs);
            m_orbsCounterTextBuilder.Append('/');
            m_orbsCounterTextBuilder.Append(GameLogicManager.Instance.MaxOrbs);

            m_orbsCounterText.SetText(m_orbsCounterTextBuilder);
        }
    }
}
