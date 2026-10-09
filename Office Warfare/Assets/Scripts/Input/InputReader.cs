using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OfficeWarfare.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Office Warfare/Input/Input Reader")]
    public class InputReader : ScriptableObject
    {
        private const string GameplayMapName = "Gameplay";
        private const string SystemMapName = "System";

        [SerializeField] private InputActionAsset _actions;

        public event Action JumpPressed;
        public event Action AttackPressed;
        public event Action PausePressed;

        public Vector2 Move => _moveAction?.ReadValue<Vector2>() ?? Vector2.zero;
        public Vector2 Look => _lookAction?.ReadValue<Vector2>() ?? Vector2.zero;

        private InputActionMap _gameplayMap;
        private InputActionMap _systemMap;
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _jumpAction;
        private InputAction _attackAction;
        private InputAction _pauseAction;

        private void OnEnable()
        {
            if (_actions == null)
                return;

            _gameplayMap = _actions.FindActionMap(GameplayMapName, throwIfNotFound: true);
            _systemMap = _actions.FindActionMap(SystemMapName, throwIfNotFound: true);

            _moveAction = _gameplayMap.FindAction("Move", throwIfNotFound: true);
            _lookAction = _gameplayMap.FindAction("Look", throwIfNotFound: true);
            _jumpAction = _gameplayMap.FindAction("Jump", throwIfNotFound: true);
            _attackAction = _gameplayMap.FindAction("Attack", throwIfNotFound: true);
            _pauseAction = _systemMap.FindAction("Pause", throwIfNotFound: true);

            _jumpAction.performed += OnJump;
            _attackAction.performed += OnAttack;
            _pauseAction.performed += OnPause;
        }

        private void OnDisable()
        {
            if (_actions == null)
                return;

            _jumpAction.performed -= OnJump;
            _attackAction.performed -= OnAttack;
            _pauseAction.performed -= OnPause;

            _actions.Disable();
        }

        public void EnableGameplay()
        {
            _gameplayMap?.Enable();
            _systemMap?.Enable();
        }

        public void DisableGameplay()
        {
            _gameplayMap?.Disable();
            _systemMap?.Enable();
        }

        public void DisableAll() => _actions?.Disable();

        private void OnJump(InputAction.CallbackContext _) => JumpPressed?.Invoke();
        private void OnAttack(InputAction.CallbackContext _) => AttackPressed?.Invoke();
        private void OnPause(InputAction.CallbackContext _) => PausePressed?.Invoke();
    }
}
