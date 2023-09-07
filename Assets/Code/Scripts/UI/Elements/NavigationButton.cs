using Game.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

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

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (UIManager.Instance != null)
            {
                UIManager.Instance.SetActiveView(m_targetView);
            }
        }
    }
}
