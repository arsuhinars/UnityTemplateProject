using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Elements
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class RoundedRect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
    {
        private static readonly int m_sizeId = Shader.PropertyToID("_Size");

        [SerializeField] private float m_transitionDuration = 0.2f;
        [SerializeField] private Ease m_transitionEase = Ease.InOutCubic;
        [Space]
        [SerializeField] private Material m_normalMaterial;
        [SerializeField] private Material m_hoverMaterial;
        [SerializeField] private Material m_selectedMaterial;
        [SerializeField] private Material m_pressedMaterial;
        [SerializeField] private Material m_disabledMaterial;

        private Tween m_tween;
        private bool m_isInteractable = true;
        private bool m_isHovered = false;
        private bool m_isPressed = false;
        private bool m_isSelected = false;
        private int m_hoverPointerId = -1;
        private int m_pressedPointerId = -1;
        private Material m_activeMaterial;
        private Material m_material;
        private Image m_image;

        private void Awake()
        {
            m_image = GetComponent<Image>();

            if (TryGetComponent<BaseButton>(out var button))
            {
                button.OnBecameInteractable += OnBecameInteractabke;
                button.OnBecameUninteractable += OnBecameUninteractable;
            }
        }

        private void Start()
        {
            if (m_normalMaterial == null)
            {
                m_normalMaterial = m_image.material;
            }

            m_activeMaterial = m_normalMaterial;
            m_material = new Material(m_normalMaterial);

            m_image.material = m_material;

            UpdateRectSize();
        }

        private void OnDisable()
        {
            m_tween?.Complete();
        }

        private void OnRectTransformDimensionsChange()
        {
            UpdateRectSize();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (m_hoverPointerId != -1)
            {
                return;
            }

            m_isHovered = true;
            m_hoverPointerId = eventData.pointerId;

            UpdateMaterial();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerId != m_hoverPointerId)
            {
                return;
            }

            m_isHovered = false;
            m_hoverPointerId = -1;

            UpdateMaterial();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (m_pressedPointerId != -1)
            {
                return;
            }

            m_isPressed = true;
            m_pressedPointerId = eventData.pointerId;

            UpdateMaterial();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerId != m_pressedPointerId)
            {
                return;
            }

            m_isPressed = false;
            m_pressedPointerId = -1;

            UpdateMaterial();
        }

        public void OnSelect(BaseEventData eventData)
        {
            m_isSelected = true;

            UpdateMaterial();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            m_isSelected = false;

            UpdateMaterial();
        }

        private void OnBecameInteractabke()
        {
            m_isInteractable = true;

            UpdateMaterial();
        }

        private void OnBecameUninteractable()
        {
            m_isInteractable = false;

            UpdateMaterial();
        }

        private void UpdateMaterial()
        {
            var targetMaterial = m_normalMaterial;

            if (!m_isInteractable && m_disabledMaterial != null)
            {
                targetMaterial = m_disabledMaterial;
            }
            else if (m_isPressed && m_pressedMaterial != null)
            {
                targetMaterial = m_pressedMaterial;
            }
            else if (m_isHovered && m_hoverMaterial != null)
            {
                targetMaterial = m_hoverMaterial;
            }
            else if (m_isSelected && m_selectedMaterial != null)
            {
                targetMaterial = m_selectedMaterial;
            }

            if (targetMaterial != m_activeMaterial)
            {
                m_tween?.Kill();
                m_tween = CreateMaterialTween(m_activeMaterial, targetMaterial, m_transitionDuration);
                m_tween.SetUpdate(true);
                m_tween.SetEase(m_transitionEase);
                m_activeMaterial = targetMaterial;
            }
        }

        private Tween CreateMaterialTween(Material startValue, Material endValue, float duration)
        {
            float t = 0f;
            return DOTween.To(
                () => t,
                (val) =>
                {
                    t = val;
                    m_material.Lerp(startValue, endValue, t);
                    UpdateRectSize();
                },
                1f, duration
            );
        }

        private void UpdateRectSize()
        {
            if (m_material != null)
            {
                var size = (transform as RectTransform).sizeDelta;
                m_material.SetVector(m_sizeId, size);
            }
        }
    }
}
