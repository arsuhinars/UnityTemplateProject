using Game.UI.Elements;
using UnityEngine;

namespace Game.UI.Views
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public class BaseUIView : MonoBehaviour
    {
        private enum ViewState
        {
            None, Showed, Hidden
        }

        private Canvas m_canvas;
        private CanvasGroup m_canvasGroup;
        private ITransition m_transition;
        private ViewState m_state = ViewState.None;

        public void Show()
        {
            if (m_state == ViewState.Showed)
            {
                return;
            }

            m_state = ViewState.Showed;
            m_canvas.enabled = true;
            m_canvasGroup.blocksRaycasts = true;
            enabled = true;

            if (m_transition != null)
            {
                m_transition.Show();
            }

            OnShowed();
        }

        public void Hide()
        {
            if (m_state == ViewState.Hidden)
            {
                return;
            }

            m_state = ViewState.Hidden;
            m_canvasGroup.blocksRaycasts = false;

            if (m_transition != null)
            {
                m_transition?.Hide(() =>
                {
                    m_canvas.enabled = false;
                    enabled = false;
                });
            }
            else
            {
                enabled = false;
                m_canvas.enabled = false;
            }

            OnHidden();
        }

        public void Complete()
        {
            if (m_transition != null)
            {
                m_transition.Complete();
            }
        }

        protected virtual void OnShowed() { }

        protected virtual void OnHidden() { }

        protected virtual void Awake()
        {
            m_canvas = GetComponent<Canvas>();
            m_canvasGroup = GetComponent<CanvasGroup>();
            m_transition = GetComponent<ITransition>();
        }
    }
}
