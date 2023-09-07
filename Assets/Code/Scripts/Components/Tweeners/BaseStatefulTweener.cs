using DG.Tweening;
using UnityEngine;

namespace Game.Components.Tweeners
{
    public abstract class BaseStatefulTweener : MonoBehaviour
    {
        public bool State
        {
            get => m_state;
            set
            {
                if (value == m_state)
                {
                    return;
                }

                m_tween?.Kill();
                m_state = value;
                if (value)
                {
                    m_tween = CreateTweenIn();
                }
                else
                {
                    m_tween = CreateTweenOut();
                }
            }
        }

        private bool m_state = false;
        private Tween m_tween = null;

        public void Complete()
        {
            m_tween?.Complete();
            m_tween?.Kill();
            m_tween = null;
        }

        protected abstract Tween CreateTweenIn();

        protected abstract Tween CreateTweenOut();

        private void Start()
        {
            m_state = false;
            m_tween = CreateTweenOut();
            Complete();
        }

        private void OnDestroy()
        {
            Complete();
        }
    }
}
