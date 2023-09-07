using DG.Tweening;
using System;
using UnityEngine;

namespace Game.UI.Elements
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeTransition : MonoBehaviour, ITransition
    {
        public bool IsShowed => m_isShowed;

        [SerializeField] private float m_duration = 0.2f;
        [SerializeField] private Ease m_ease;

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
            m_tween = CreateFadeTween(1f, m_duration);
            m_tween.SetEase(m_ease);
            m_tween.SetUpdate(true);
            m_tween.OnComplete(() => completeHandler?.Invoke());
        }

        public void Hide(Action completeHandler = null)
        {
            if (!m_isShowed)
            {
                return;
            }

            m_isShowed = false;
            m_tween = CreateFadeTween(0f, m_duration);
            m_tween.SetEase(m_ease);
            m_tween.SetUpdate(true);
            m_tween.OnComplete(() => completeHandler?.Invoke());
        }

        public void Complete()
        {
            m_tween?.Complete();
            m_tween?.Kill();
        }

        private Tween CreateFadeTween(float endAlphaValue, float duration)
        {
            return DOTween.To(
                () => m_canvasGroup.alpha,
                (a) => m_canvasGroup.alpha = a,
                endAlphaValue, duration
            );
        }
    }
}
