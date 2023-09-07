using Game.Utils;
using UnityEngine;

namespace Game.Components
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovement : MonoBehaviour
    {
        public Vector2 MoveVector
        {
            get => m_moveVector;
            set => m_moveVector = Vector2.ClampMagnitude(value, 1f);
        }
        public Vector2 LookDirection
        {
            get => m_lookDir;
            set
            {
                m_lookDir = value;
                m_targetRot = Mathf.Atan2(value.x, value.y) * Mathf.Rad2Deg;
            }
        }

        [Header("Components")]
        [SerializeField] private AudioSource m_audioSource;
        [Header("Settings")]
        [SerializeField] private float m_groundedAccel = 1f;
        [SerializeField] private float m_airAccel = 1f;
        [SerializeField] private float m_groundedDrag = 1f;
        [SerializeField] private float m_airDrag = 1f;
        [SerializeField] private float m_jumpVelocity = 1f;
        [SerializeField] private float m_gravityScale = 1f;
        [SerializeField] private float m_rotationSmoothTime = 0.2f;
        [Space]
        [SerializeField] private AudioClip[] m_footstepClips;
        [SerializeField] private float m_footstepBaseDelay = 0.5f;
        [SerializeField] private float m_minFlyTime = 0.5f;

        private CharacterController m_char;
        private float m_baseVelocity;
        private float m_lowerGroundY;
        private Vector3 m_lookDir = Vector3.forward;
        private Vector2 m_moveVector = Vector2.zero;
        private Vector3 m_groundNormal = Vector3.zero;
        private Vector3 m_velocity = Vector3.zero;
        private float m_targetRot = 0f;
        private float m_currRot = 0f;
        private float m_rotVelocity = 0f;
        private float m_footstepTimer = 0f;
        private float m_flyTimer = 0f;
        private bool m_groundedState;

        public void ResetState()
        {
            m_velocity = Vector3.zero;
            m_currRot = m_targetRot;
            m_rotVelocity = 0f;
            m_footstepTimer = 0f;
            m_groundedState = m_char.isGrounded;
        }

        public void Jump()
        {
            if (m_char.isGrounded)
            {
                m_velocity.y += m_jumpVelocity;
            }
        }

        private void Awake()
        {
            m_char = GetComponent<CharacterController>();
        }

        private void Start()
        {
            m_baseVelocity = m_groundedAccel / m_groundedDrag;
            m_lowerGroundY = m_char.center.y - m_char.height * 0.5f + m_char.radius;
        }

        private void Update()
        {
            UpdateMovement();
            UpdateRotation();
            UpdateDrag();
            UpdateCharacter();
            UpdateSounds();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            var localPoint = transform.InverseTransformPoint(hit.point);
            if (localPoint.y < m_lowerGroundY)
            {
                m_groundNormal += hit.normal;
            }
            else if (Vector3.Dot(m_velocity, hit.normal) < 0f)
            {
                m_velocity -= Vector3.Project(m_velocity, hit.normal);
            }
        }

        private void UpdateMovement()
        {
            var force = new Vector3(m_moveVector.x, 0f, m_moveVector.y) * (
                m_char.isGrounded ? m_groundedAccel : m_airAccel
            );
            force += Physics.gravity * m_gravityScale;
            if (Vector3.Angle(m_groundNormal, Vector3.up) > m_char.slopeLimit)
            {
                force -= Vector3.Project(Physics.gravity, m_groundNormal) * m_gravityScale;
            }

            m_groundNormal = Vector3.zero;

            m_velocity += force * Time.deltaTime;
        }

        private void UpdateRotation()
        {
            if (m_char.isGrounded)
            {
                m_currRot = Mathf.SmoothDampAngle(
                    m_currRot, m_targetRot, ref m_rotVelocity, m_rotationSmoothTime
                );
            }

            transform.rotation = Quaternion.AngleAxis(m_currRot, Vector3.up);
        }

        private void UpdateDrag()
        {
            var drag = m_char.isGrounded ? m_groundedDrag : m_airDrag;

            m_velocity -= Mathf.Min(drag * Time.deltaTime, 1f) * m_velocity;
        }

        private void UpdateCharacter()
        {
            m_char.Move(m_velocity * Time.deltaTime);
        }

        private void UpdateSounds()
        {
            if (m_char.isGrounded)
            {
                var vel = new Vector2(m_velocity.x, m_velocity.z).magnitude;
                m_footstepTimer += Time.deltaTime;

                bool isTimedOut = !Mathf.Approximately(vel, 0f) && (m_footstepTimer > m_footstepBaseDelay * m_baseVelocity / vel);
                bool didLanded = m_char.isGrounded && !m_groundedState && m_flyTimer > m_minFlyTime;

                if (isTimedOut || didLanded)
                {
                    m_footstepTimer = 0f;
                    m_audioSource.PlayRandomClip(m_footstepClips);
                }

                m_flyTimer = 0f;
            }
            else
            {
                m_flyTimer += Time.deltaTime;
            }

            m_groundedState = m_char.isGrounded;
        }
    }
}
