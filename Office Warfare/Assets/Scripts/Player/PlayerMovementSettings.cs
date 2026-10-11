using UnityEngine;

namespace OfficeWarfare.Player
{
    [CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "Office Warfare/Player/Movement Settings")]
    public class PlayerMovementSettings : ScriptableObject
    {
        [Header("Ground")]
        [Tooltip("Top horizontal speed in metres per second.")]
        [SerializeField, Min(0f)] private float _moveSpeed = 6f;
        [Tooltip("How fast the player reaches top speed while grounded. Higher feels snappier.")]
        [SerializeField, Min(0f)] private float _groundAcceleration = 70f;
        [Tooltip("How fast the player stops when there is no input while grounded.")]
        [SerializeField, Min(0f)] private float _groundDeceleration = 90f;

        [Header("Sprint")]
        [Tooltip("Top horizontal speed while sprinting, in metres per second.")]
        [SerializeField, Min(0f)] private float _sprintSpeed = 9f;
        [Tooltip("Only sprint while moving forward, not when strafing or backpedalling.")]
        [SerializeField] private bool _sprintRequiresForward = true;

        [Header("Air")]
        [Tooltip("How quickly the player can change direction mid-air. Lower means less air control.")]
        [SerializeField, Min(0f)] private float _airAcceleration = 30f;

        [Header("Jump")]
        [Tooltip("Peak jump height in metres when the button is held.")]
        [SerializeField, Min(0f)] private float _jumpHeight = 1.2f;
        [Tooltip("Seconds to reach the top of a full jump. Lower is snappier and heavier.")]
        [SerializeField, Min(0.05f)] private float _timeToApex = 0.32f;
        [Tooltip("Gravity multiplier while falling. Above 1 makes the fall faster than the rise.")]
        [SerializeField, Min(1f)] private float _fallGravityMultiplier = 1.6f;
        [Tooltip("Gravity multiplier while rising after the jump button is released, for short hops.")]
        [SerializeField, Min(1f)] private float _jumpCutGravityMultiplier = 2.5f;
        [Tooltip("Maximum downward speed in metres per second.")]
        [SerializeField, Min(0f)] private float _maxFallSpeed = 20f;
        [Tooltip("Seconds after leaving a ledge during which a jump is still allowed.")]
        [SerializeField, Min(0f)] private float _coyoteTime = 0.1f;
        [Tooltip("Seconds a jump press is remembered before landing.")]
        [SerializeField, Min(0f)] private float _jumpBufferTime = 0.12f;
        [Tooltip("Small downward velocity applied while grounded so the controller stays snapped to slopes and steps.")]
        [SerializeField, Range(-10f, 0f)] private float _groundedVerticalVelocity = -2f;

        public float MoveSpeed => _moveSpeed;
        public float GroundAcceleration => _groundAcceleration;
        public float GroundDeceleration => _groundDeceleration;
        public float SprintSpeed => _sprintSpeed;
        public bool SprintRequiresForward => _sprintRequiresForward;
        public float AirAcceleration => _airAcceleration;
        public float FallGravityMultiplier => _fallGravityMultiplier;
        public float JumpCutGravityMultiplier => _jumpCutGravityMultiplier;
        public float MaxFallSpeed => _maxFallSpeed;
        public float CoyoteTime => _coyoteTime;
        public float JumpBufferTime => _jumpBufferTime;
        public float GroundedVerticalVelocity => _groundedVerticalVelocity;

        public float Gravity => -2f * _jumpHeight / (_timeToApex * _timeToApex);
        public float JumpVelocity => 2f * _jumpHeight / _timeToApex;
    }
}
