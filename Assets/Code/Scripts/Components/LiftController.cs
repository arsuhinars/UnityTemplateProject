using DG.Tweening;
using Game.Managers;
using UnityEngine;

namespace Game.Components
{
    public class LiftController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Transform m_model;
        [Header("Settings")]
        [SerializeField] private Vector3 m_moveOffset;
        [SerializeField] private float m_moveDuration = 1f;
        [SerializeField] private float m_moveDelay = 1f;
        [SerializeField] private Ease m_moveEase = Ease.InCubic;
        [SerializeField, Range(0f, 1f)] private float m_phasePercentage = 0f;

        private Tween m_tween;

        private void OnEnable()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(
                m_model.DOLocalMove(m_moveOffset, m_moveDuration).SetEase(m_moveEase).From(Vector3.zero)
            );
            sequence.AppendInterval(m_moveDelay);
            sequence.Append(
                m_model.DOLocalMove(Vector3.zero, m_moveDuration).SetEase(m_moveEase)
            );
            sequence.AppendInterval(m_moveDelay);
            sequence.SetLoops(-1);

            var gameTime = GameLogicManager.Instance.GameTimer;
            float period = 2f * (m_moveDuration + m_moveDelay);
            float phase = period * m_phasePercentage;

            sequence.Goto((gameTime + phase) % period, true);

            m_tween = sequence;
        }

        private void OnDisable()
        {
            m_tween?.Kill();
        }
    }
}
