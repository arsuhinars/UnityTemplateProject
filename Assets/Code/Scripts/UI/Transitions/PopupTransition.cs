using DG.Tweening;
using Game.Utils;
using System;
using UnityEngine;

namespace Game.UI.Transitions
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PopupTransition : MonoBehaviour, ITransition
    {
        public bool IsShowed => m_isShowed;

        [SerializeField] private RectTransform m_popupTarget;
        [SerializeField] private Vector3 m_popupScale = Vector3.one;
        [SerializeField] private bool m_fade = true;
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
            m_tween = CreateTween(1f, Vector3.one, completeHandler);
        }

        public void Hide(Action completeHandler = null)
        {
            if (!m_isShowed)
            {
                return;
            }

            m_isShowed = false;
            m_tween = CreateTween(0f, m_popupScale, completeHandler);
        }


        public void Complete()
        {
            m_tween?.Complete();
            m_tween?.Kill();
        }

        private Tween CreateTween(float endAlphaValue, Vector3 endScaleValue, Action completeHandler)
        {
            var seq = DOTween.Sequence();
            seq.Append(m_popupTarget.DOScale(endScaleValue, m_duration).SetEase(m_ease));

            if (m_fade)
            {
                seq.Join(m_canvasGroup.DOAlpha(endAlphaValue, m_duration).SetEase(m_ease));
            }

            seq.SetUpdate(true);
            seq.OnComplete(() => completeHandler?.Invoke());

            return seq;
        }
    }
}
