using Game.Components;
using Game.Input;
using Game.Managers;
using Game.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Entities
{
    [RequireComponent(typeof(CharacterMovement))]
	public class PlayerEntity : BaseEntity
	{
        public bool IsFreezed
        {
            get => m_isFreezed;
            set
            {
                m_isFreezed = value;
                if (value)
                {
                    m_moveVector = Vector3.zero;
                }
            }
        }

        [Header("Components")]
        [SerializeField] private ThirdPersonCamera m_camera;

        private bool m_isFreezed = false;
        private Vector2 m_moveVector;
        private CharacterMovement m_movement;
        private GameInputActions m_inputActions;

        protected override void SpawnHandler()
        {
            IsFreezed = false;
            m_camera.enabled = true;
            m_camera.LookAt(transform.forward);
            m_movement.LookDirection = new Vector2(transform.forward.x, transform.forward.z);
            m_movement.ResetState();
            m_inputActions.Player.Enable();
        }

        protected override void ReleaseHandler()
        {
            m_moveVector = Vector2.zero;
            m_camera.enabled = false;
            m_inputActions.Player.Disable();
        }

        private void Awake()
        {
            m_movement = GetComponent<CharacterMovement>();
            m_inputActions = new();
        }

        private void OnEnable()
        {
            m_inputActions.Player.Look.performed += OnLookActionPerformed;
            m_inputActions.Player.Look.canceled += OnLookActionPerformed;
            m_inputActions.Player.Movement.performed += OnMovementActionPerformed;
            m_inputActions.Player.Movement.canceled += OnMovementActionPerformed;
            m_inputActions.Player.Jump.performed += OnJumpActionPerformed;
            m_inputActions.Player.Pause.canceled += OnPauseActionPerformed;
        }

        private void OnDisable()
        {
            m_inputActions.Player.Look.performed -= OnLookActionPerformed;
            m_inputActions.Player.Look.canceled -= OnLookActionPerformed;
            m_inputActions.Player.Movement.performed -= OnMovementActionPerformed;
            m_inputActions.Player.Movement.canceled -= OnMovementActionPerformed;
            m_inputActions.Player.Jump.performed -= OnJumpActionPerformed;
            m_inputActions.Player.Pause.canceled -= OnPauseActionPerformed;
        }

        private void Update()
        {
            var camForward = m_camera.transform.forward;
            var lookDir = new Vector2(camForward.x, camForward.z).normalized;
            var moveVec = Vector2.Perpendicular(lookDir) * -m_moveVector.x + lookDir * m_moveVector.y;

            m_movement.MoveVector = moveVec;

            if (!moveVec.IsZero())
            {
                m_movement.LookDirection = moveVec;
            }
        }

        private void OnLookActionPerformed(InputAction.CallbackContext context)
        {
            if (!m_isFreezed)
            {
                var v = context.ReadValue<Vector2>();
                m_camera.TargetRotation += new Vector2(-v.x, v.y);
            }
        }

        private void OnMovementActionPerformed(InputAction.CallbackContext context)
        {
            if (!m_isFreezed)
            {
                m_moveVector = context.ReadValue<Vector2>();
            }
        }

        private void OnJumpActionPerformed(InputAction.CallbackContext context)
        {
            if (!m_isFreezed && context.phase == InputActionPhase.Performed)
            {
                m_movement.Jump();
            }
        }

        private void OnPauseActionPerformed(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Canceled)
            {
                return;
            }

            if (GameEventsManager.Instance.IsPaused)
            {
                GameEventsManager.Instance.ResumeGame();
            }
            else
            {
                GameEventsManager.Instance.PauseGame();
            }
        }
    }
}
