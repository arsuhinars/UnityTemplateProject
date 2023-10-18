using System;
using UnityEngine.UI;

namespace Game.UI.Elements
{
    public class BaseButton : Button, IInteractableStateChanged
    {
        public event Action<bool> OnInteractableStateChanged;

        private bool m_isInteractable;

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
