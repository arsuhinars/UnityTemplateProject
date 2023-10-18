using DG.Tweening;
using Game.Utils;
using System;
using UnityEngine;

namespace Game.UI.Transitions
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeTransition : MonoBehaviour, ITransition
    {
        public bool IsShowed => m_isShowed;

        [SerializeField] private float m_duration = 0.2f;
        [SerializeField] private Ease m_ease = Ease.InOutCubic;

        private bool m_isShowed = true;
        private CanvasGroup m_canvasGroup;
        private Tween m_tween;

        private void Awake()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnDisable()
        {
            Complete();   
        }

        public void Show(Action completeHandler = null)
        {
            if (m_isShowed)
            {
                return;
            }

            m_isShowed = true;
            m_tween = CreateTween(1f, completeHandler);
        }

        public void Hide(Action completeHandler = null)
        {
            if (!m_isShowed)
            {
                return;
            }

            m_isShowed = false;
            m_tween = CreateTween(0f, completeHandler);
        }

        public void Complete()
        {
            m_tween?.Complete();
            m_tween?.Kill();
        }

        private Tween CreateTween(float endAlphaValue, Action completeHandler)
        {
            return m_canvasGroup.DOAlpha(endAlphaValue, m_duration)
                .SetEase(m_ease)
                .SetUpdate(true)
                .OnComplete(() => completeHandler?.Invoke());
        }
    }
}
