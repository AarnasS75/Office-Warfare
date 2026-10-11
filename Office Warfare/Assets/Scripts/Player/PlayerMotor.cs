using OfficeWarfare.Input;
using UnityEngine;

namespace OfficeWarfare.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMotor : MonoBehaviour
    {
        private const float InputDeadZone = 0.01f;

        [SerializeField] private InputReader _input;
        [SerializeField] private PlayerMovementSettings _settings;

        public bool IsGrounded { get; private set; }
        public bool IsSprinting { get; private set; }
        public Vector3 Velocity { get; private set; }

        public float HorizontalSpeedRatio
        {
            get
            {
                if (_settings.MoveSpeed <= 0f)
                    return 0f;

                var horizontal = new Vector3(Velocity.x, 0f, Velocity.z);
                return horizontal.magnitude / _settings.MoveSpeed;
            }
        }

        public float HorizontalSpeed01 => Mathf.Clamp01(HorizontalSpeedRatio);

        private CharacterController _controller;
        private Vector3 _horizontalVelocity;
        private float _verticalVelocity;
        private float _lastGroundedTime = float.NegativeInfinity;
        private float _lastJumpPressedTime = float.NegativeInfinity;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void OnEnable() => _input.JumpPressed += OnJumpPressed;
        private void OnDisable() => _input.JumpPressed -= OnJumpPressed;

        private void OnJumpPressed() => _lastJumpPressedTime = Time.time;

        private void Update()
        {
            IsGrounded = _controller.isGrounded;
            if (IsGrounded)
                _lastGroundedTime = Time.time;

            UpdateHorizontalVelocity();
            UpdateVerticalVelocity();

            var motion = _horizontalVelocity;
            motion.y = _verticalVelocity;

            var collisions = _controller.Move(motion * Time.deltaTime);
            if ((collisions & CollisionFlags.Above) != 0 && _verticalVelocity > 0f)
                _verticalVelocity = 0f;

            Velocity = _controller.velocity;
        }

        private void UpdateHorizontalVelocity()
        {
            var moveInput = _input.Move;
            IsSprinting = _input.IsSprintHeld && CanSprint(moveInput);

            var direction = Vector3.ClampMagnitude(transform.right * moveInput.x + transform.forward * moveInput.y, 1f);
            var targetVelocity = direction * (IsSprinting ? _settings.SprintSpeed : _settings.MoveSpeed);

            _horizontalVelocity = Vector3.MoveTowards(_horizontalVelocity, targetVelocity, GetAcceleration(direction) * Time.deltaTime);
        }

        private bool CanSprint(Vector2 moveInput)
        {
            if (_settings.SprintRequiresForward)
                return moveInput.y > InputDeadZone;

            return moveInput.sqrMagnitude > InputDeadZone;
        }

        private float GetAcceleration(Vector3 direction)
        {
            if (!IsGrounded)
                return _settings.AirAcceleration;

            return direction.sqrMagnitude > InputDeadZone ? _settings.GroundAcceleration : _settings.GroundDeceleration;
        }

        private void UpdateVerticalVelocity()
        {
            if (IsGrounded && _verticalVelocity < 0f)
                _verticalVelocity = _settings.GroundedVerticalVelocity;

            var jumpBuffered = Time.time - _lastJumpPressedTime <= _settings.JumpBufferTime;
            var withinCoyoteTime = Time.time - _lastGroundedTime <= _settings.CoyoteTime;

            if (jumpBuffered && withinCoyoteTime)
            {
                _verticalVelocity = _settings.JumpVelocity;
                _lastJumpPressedTime = float.NegativeInfinity;
                _lastGroundedTime = float.NegativeInfinity;
            }

            _verticalVelocity += GetGravity() * Time.deltaTime;
            _verticalVelocity = Mathf.Max(_verticalVelocity, -_settings.MaxFallSpeed);
        }

        private float GetGravity()
        {
            if (_verticalVelocity < 0f)
                return _settings.Gravity * _settings.FallGravityMultiplier;

            if (!_input.IsJumpHeld)
                return _settings.Gravity * _settings.JumpCutGravityMultiplier;

            return _settings.Gravity;
        }
    }
}
