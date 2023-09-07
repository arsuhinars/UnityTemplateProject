using DG.Tweening;
using System;
using UnityEngine;

namespace Game.UI.Elements
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PopupTransition : MonoBehaviour, ITransition
    {
        public bool IsShowed => m_isShowed;

        [SerializeField] private RectTransform m_popupTarget;
        [SerializeField] private Vector3 m_popupScale;
        [SerializeField] private bool m_doFade = true;
        [SerializeField] private float m_duration = 0.2f;
        [SerializeField] private Ease m_ease;

        private bool m_isShowed = true;
        private CanvasGroup m_canvasGroup;
        private Sequence m_tween;

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

            m_tween = DOTween.Sequence();
            m_tween.Append(m_popupTarget.DOScale(Vector3.one, m_duration).SetEase(m_ease));

            if (m_doFade)
            {
                m_tween.Join(CreateFadeTween(1f, m_duration).SetEase(m_ease));
            }

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

            m_tween = DOTween.Sequence();
            m_tween.Append(m_popupTarget.DOScale(m_popupScale, m_duration).SetEase(m_ease));

            if (m_doFade)
            {
                m_tween.Join(CreateFadeTween(0f, m_duration).SetEase(m_ease));
            }

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
                endAlphaValue,
                duration
            );
        }
    }
}
