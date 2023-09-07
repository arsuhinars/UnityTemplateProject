using DG.Tweening;
using UnityEngine;

namespace Game.UI.Elements
{
    [RequireComponent(typeof(RectTransform))]
    public class Spinner : MonoBehaviour
    {
        [SerializeField] private float m_rotationSpeed = 360f;

        private Tween m_tween;

        private void OnEnable()
        {
            m_tween?.Kill();

            m_tween = (transform as RectTransform).DORotate(
                new Vector3(0f, 0f, -360f), 360f / m_rotationSpeed, RotateMode.FastBeyond360
            );
            m_tween.SetEase(Ease.Linear);
            m_tween.SetLoops(-1);
            m_tween.SetUpdate(true);
        }

        private void OnDisable()
        {
            m_tween?.Kill();
        }
    }
}
