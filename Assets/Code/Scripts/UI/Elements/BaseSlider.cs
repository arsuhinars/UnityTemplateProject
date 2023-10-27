using Game.Managers;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Elements
{
    public class BaseSlider : Slider, IInteractableStateChanged, IPointerClickHandler
    {
        public event Action<bool> OnInteractableStateChanged;

        private bool m_isInteractable;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_isInteractable)
            {
                UIManager.Instance.PlayClickSound();
            }
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (state == SelectionState.Disabled && m_isInteractable)
            {
                m_isInteractable = false;
                OnInteractableStateChanged?.Invoke(false);
            }
            else if (state != SelectionState.Disabled && !m_isInteractable)
            {
                m_isInteractable = true;
                OnInteractableStateChanged?.Invoke(true);
            }
        }

        protected override void Awake()
        {
            base.Awake();

            m_isInteractable = IsInteractable();
        }
    }
}
