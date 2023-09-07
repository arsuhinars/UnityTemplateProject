using Game.Managers;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Elements
{
    public class BaseButton : Button
    {
        public event Action OnBecameInteractable;
        public event Action OnBecameUninteractable;

        private bool m_isInteractable;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (IsInteractable() && UIManager.Instance != null)
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
                OnBecameUninteractable?.Invoke();
            }
            else if (state != SelectionState.Disabled && !m_isInteractable)
            {
                m_isInteractable = true;
                OnBecameInteractable?.Invoke();
            }
        }

        protected override void Awake()
        {
            base.Awake();

            m_isInteractable = IsInteractable();
        }
    }
}
