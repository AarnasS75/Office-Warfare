using OfficeWarfare.Input;
using UnityEngine;

namespace OfficeWarfare.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField] private InputReader _input;
        [SerializeField] private PlayerMovementSettings _settings;

        public bool IsGrounded { get; private set; }
        public Vector3 Velocity { get; private set; }

        public float HorizontalSpeed01
        {
            get
            {
                if (_settings.MoveSpeed <= 0f)
                    return 0f;

                var horizontal = new Vector3(Velocity.x, 0f, Velocity.z);
                return Mathf.Clamp01(horizontal.magnitude / _settings.MoveSpeed);
            }
        }

        private CharacterController _controller;
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

            var velocity = GetHorizontalVelocity();
            velocity.y = GetVerticalVelocity();

            _controller.Move(velocity * Time.deltaTime);
            Velocity = _controller.velocity;
        }

        private Vector3 GetHorizontalVelocity()
        {
            var moveInput = _input.Move;
            var direction = transform.right * moveInput.x + transform.forward * moveInput.y;
            return Vector3.ClampMagnitude(direction, 1f) * _settings.MoveSpeed;
        }

        private float GetVerticalVelocity()
        {
            if (IsGrounded && _verticalVelocity < 0f)
                _verticalVelocity = _settings.GroundedVerticalVelocity;

            var jumpBuffered = Time.time - _lastJumpPressedTime <= _settings.JumpBufferTime;
            var withinCoyoteTime = Time.time - _lastGroundedTime <= _settings.CoyoteTime;

            if (jumpBuffered && withinCoyoteTime)
            {
                _verticalVelocity = Mathf.Sqrt(2f * _settings.JumpHeight * -_settings.Gravity);
                _lastJumpPressedTime = float.NegativeInfinity;
                _lastGroundedTime = float.NegativeInfinity;
            }

            _verticalVelocity += _settings.Gravity * Time.deltaTime;
            return _verticalVelocity;
        }
    }
}
