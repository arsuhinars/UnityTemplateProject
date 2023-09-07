using DG.Tweening;
using UnityEngine;

namespace Game.Components.Tweeners
{
    public abstract class BaseTweener : MonoBehaviour
    {
        private Tween m_tween = null;

        public void Play()
        {
            Complete();
            m_tween = CreateTween();
        }

        public void Complete()
        {
            m_tween?.Complete();
            m_tween?.Kill();
            m_tween = null;
        }

        protected abstract Tween CreateTween();

        private void OnDisable()
        {
            Complete();
        }
    }
}
