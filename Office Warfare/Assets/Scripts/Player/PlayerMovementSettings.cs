using UnityEngine;

namespace OfficeWarfare.Player
{
    [CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "Office Warfare/Player/Movement Settings")]
    public class PlayerMovementSettings : ScriptableObject
    {
        [Header("Ground")]
        [Tooltip("Horizontal speed in metres per second.")]
        [SerializeField, Min(0f)] private float _moveSpeed = 8f;

        [Header("Jump")]
        [Tooltip("Peak jump height in metres.")]
        [SerializeField, Min(0f)] private float _jumpHeight = 1.6f;
        [Tooltip("Multiplier applied to Physics.gravity while airborne.")]
        [SerializeField, Min(0f)] private float _gravityMultiplier = 2f;
        [Tooltip("Seconds after leaving a ledge during which a jump is still allowed.")]
        [SerializeField, Min(0f)] private float _coyoteTime = 0.1f;
        [Tooltip("Seconds a jump press is remembered before landing.")]
        [SerializeField, Min(0f)] private float _jumpBufferTime = 0.1f;
        [Tooltip("Small downward velocity applied while grounded so the controller stays snapped to slopes and steps.")]
        [SerializeField, Range(-10f, 0f)] private float _groundedVerticalVelocity = -2f;

        public float MoveSpeed => _moveSpeed;
        public float JumpHeight => _jumpHeight;
        public float GravityMultiplier => _gravityMultiplier;
        public float CoyoteTime => _coyoteTime;
        public float JumpBufferTime => _jumpBufferTime;
        public float GroundedVerticalVelocity => _groundedVerticalVelocity;

        public float Gravity => Physics.gravity.y * _gravityMultiplier;
    }
}
