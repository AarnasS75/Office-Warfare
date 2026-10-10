using OfficeWarfare.Input;
using UnityEngine;

namespace OfficeWarfare.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerArmsAnimator : MonoBehaviour
    {
        private static readonly int SpeedParam = Animator.StringToHash("Speed");
        private static readonly int AttackParam = Animator.StringToHash("Attack");
        private static readonly int AttackSpeedParam = Animator.StringToHash("AttackSpeed");
        private static readonly int AttackState = Animator.StringToHash("Attack");

        [SerializeField] private InputReader _input;
        [SerializeField] private PlayerMotor _motor;
        [SerializeField] private ArmsAnimationSettings _settings;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable() => _input.AttackPressed += OnAttackPressed;
        private void OnDisable() => _input.AttackPressed -= OnAttackPressed;

        private void Update()
        {
            var speed = _motor.IsGrounded ? _motor.HorizontalSpeed01 : 0f;
            _animator.SetFloat(SpeedParam, speed, _settings.SpeedDampTime, Time.deltaTime);
            _animator.SetFloat(AttackSpeedParam, _settings.AttackSpeed);
        }

        private void OnAttackPressed()
        {
            if (IsAttacking())
                return;

            _animator.SetTrigger(AttackParam);
        }

        private bool IsAttacking()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == AttackState)
                return true;

            return _animator.IsInTransition(0) && _animator.GetNextAnimatorStateInfo(0).shortNameHash == AttackState;
        }
    }
}
