using DG.Tweening;
using Game.Scriptables;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI.Elements
{
    [RequireComponent(typeof(RoundedRect))]
    public class RoundedRectAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float m_transitionDuration = 0.2f;
        [SerializeField] private Ease m_transitionEase = Ease.InOutCubic;
        [SerializeField] private RoundedRectStyles m_styles;
        
        private IInteractableStateChanged m_interactable;
        private RoundedRect m_rect;
        private Tween m_tween;
        private int m_pointerId = -1;
        private bool m_isHovered = false;
        private bool m_isPressed = false;
        private bool m_isDisabled = false;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (m_pointerId == eventData.pointerId)
            {
                m_isPressed = true;
                UpdateRectStyle();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (m_pointerId == eventData.pointerId)
            {
                if (!m_isHovered)
                {
                    m_pointerId = -1;
                }
                m_isPressed = false;
                UpdateRectStyle();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (m_pointerId == -1)
            {
                m_pointerId = eventData.pointerId;
                m_isHovered = true;
                UpdateRectStyle();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (m_pointerId == eventData.pointerId)
            {
                if (!m_isPressed)
                {
                    m_pointerId = -1;
                }
                m_isHovered = false;
                UpdateRectStyle();
            }
        }

        private void Awake()
        {
            m_interactable = GetComponent<IInteractableStateChanged>();
            m_rect = GetComponent<RoundedRect>();

            m_interactable.OnInteractableStateChanged += OnInteractableStateChanged;
        }

        private void OnEnable()
        {
            UpdateRectStyle();
        }

        private void OnDisable()
        {
            m_tween?.Complete();
            m_tween?.Kill();
        }

        private void OnInteractableStateChanged(bool isInteractable)
        {
            m_isDisabled = !isInteractable;
            UpdateRectStyle();
        }

        private void UpdateRectStyle()
        {
            if (m_styles == null)
            {
                return;
            }

            var targetStyle = m_styles.normalStyle;
            if (m_isDisabled)
            {
                targetStyle = m_styles.disabledStyle;
            }
            else if (m_isPressed)
            {
                targetStyle = m_styles.pressedStyle;
            }
            else if (m_isHovered)
            {
                targetStyle = m_styles.hoverStyle;
            }

            m_tween?.Kill();
            m_tween = CreateTween(targetStyle, m_transitionDuration);
        }

        private Tween CreateTween(RoundedRect.StyleData endStyle, float duration)
        {
            var startStyle = m_rect.Style;

            float t = 0f;
            return DOTween.To(
                () => t,
                (v) =>
                {
                    t = v;
                    m_rect.Style = RoundedRect.StyleData.Lerp(startStyle, endStyle, v);
                },
                1f, duration
            ).SetUpdate(true).SetEase(m_transitionEase);
        }
    }
}
