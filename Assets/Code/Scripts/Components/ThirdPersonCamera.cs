using UnityEngine;

namespace Game.Components
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public Transform LookTarget
        {
            get => m_lookTarget;
            set => m_lookTarget = value;
        }
        public Vector2 TargetRotation
        {
            get => m_targetRotation;
            set
            {
                m_targetRotation = value;
                m_targetRotation.y = Mathf.Clamp(
                    m_targetRotation.y, m_minMaxVerticalAngle.x, m_minMaxVerticalAngle.y
                );
            }
        }

        [Header("Settings")]
        [SerializeField] private Transform m_lookTarget;
        [SerializeField] private float m_lookDistance = 1f;
        [SerializeField] private float m_rotationSmoothTime = 0.2f;
        [SerializeField] private float m_positionSmoothTime = 0.2f;
        [SerializeField] private Vector2 m_minMaxVerticalAngle = new(-89f, 89f);
        [Space]
        [SerializeField] private LayerMask m_collisionLayers;
        [SerializeField] private float m_cameraRadius = 1f;

        private Vector2 m_currRotation;
        private Vector2 m_targetRotation;
        private Vector2 m_rotationVelocity;
        private Vector3 m_lookDirection;
        private Vector3 m_currTargetPos;
        private Vector3 m_targetPosVelocity;

        public void LookAt(Vector3 direction)
        {
            var q = Quaternion.LookRotation(direction);
            var rotation = new Vector2(
                -q.eulerAngles.y, -q.eulerAngles.x
            );

            m_currRotation = rotation;
            m_targetRotation = rotation;
            m_rotationVelocity = Vector2.zero;
            m_currTargetPos = m_lookTarget.position;
            m_targetPosVelocity = Vector3.zero;
        }

        private void Update()
        {
            if (m_lookTarget == null)
            {
                return;
            }

            m_currRotation.x = Mathf.SmoothDampAngle(
                m_currRotation.x, m_targetRotation.x, ref m_rotationVelocity.x, m_rotationSmoothTime
            );
            m_currRotation.y = Mathf.SmoothDamp(
                m_currRotation.y, m_targetRotation.y, ref m_rotationVelocity.y, m_rotationSmoothTime
            );

            m_currTargetPos = Vector3.SmoothDamp(
                m_currTargetPos, m_lookTarget.position, ref m_targetPosVelocity, m_positionSmoothTime
            );

            var radRotation = m_currRotation * Mathf.Deg2Rad;
            m_lookDirection = new Vector3(
                Mathf.Sin(-radRotation.x), Mathf.Tan(radRotation.y), Mathf.Cos(radRotation.x)
            ).normalized;

            transform.SetPositionAndRotation(
                m_currTargetPos - m_lookDirection * FindLookDistance(),
                Quaternion.LookRotation(m_lookDirection)
            );
        }

        private float FindLookDistance()
        {
            bool hasHit = Physics.SphereCast(
                m_currTargetPos,
                m_cameraRadius,
                -m_lookDirection,
                out var hit,
                m_lookDistance,
                m_collisionLayers,
                QueryTriggerInteraction.Ignore
            );

            if (hasHit)
            {
                return hit.distance;
            }

            return m_lookDistance;
        }
    }
}
