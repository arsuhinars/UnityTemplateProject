using Game.Managers;
using UnityEngine;

namespace Game.UI.Elements
{
    public class NavigationButton : BaseButton
    {
        public string TargetView
        {
            get => m_targetView;
            set => m_targetView = value;
        }

        [SerializeField] private string m_targetView;

        protected override void Awake()
        {
            base.Awake();

            onClick.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.SetActiveView(m_targetView);
            }
        }
    }
}
