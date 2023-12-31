using Game.UI.Transitions;
using UnityEngine;

namespace Game.UI.Views
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseView : MonoBehaviour
    {
        private enum ViewState
        {
            None, Showed, Hidden
        }

        private Canvas m_canvas;
        private CanvasGroup m_canvasGroup;
        private ITransition m_transition;
        private ViewState m_state = ViewState.None;

        public void Show(bool instantly=false)
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
                if (instantly)
                {
                    m_transition.Complete();
                }
            }

            OnShowed();
        }

        public void Hide(bool instantly = false)
        {
            if (m_state == ViewState.Hidden)
            {
                return;
            }

            m_state = ViewState.Hidden;
            m_canvasGroup.blocksRaycasts = false;

            if (m_transition != null)
            {
                m_transition.Hide(() =>
                {
                    m_canvas.enabled = false;
                    enabled = false;
                });
                if (instantly)
                {
                    m_transition.Complete();
                }
            }
            else
            {
                enabled = false;
                m_canvas.enabled = false;
            }

            OnHidden();
        }

        protected abstract void OnShowed();

        protected abstract void OnHidden();

        protected virtual void Awake()
        {
            m_canvas = GetComponent<Canvas>();
            m_canvasGroup = GetComponent<CanvasGroup>();
            m_transition = GetComponent<ITransition>();
        }
    }
}
