using OfficeWarfare.Input;
using UnityEngine;

namespace OfficeWarfare.Player
{
    public class PlayerLook : MonoBehaviour
    {
        [SerializeField] private InputReader _input;
        [SerializeField] private PlayerLookSettings _settings;
        [Tooltip("Child transform that pitches up and down; the camera follows it.")]
        [SerializeField] private Transform _cameraPivot;

        private float _pitch;

        private void Start()
        {
            _pitch = Mathf.DeltaAngle(0f, _cameraPivot.localEulerAngles.x);
        }

        private void Update()
        {
            var look = _input.Look * _settings.Sensitivity;

            transform.Rotate(0f, look.x, 0f, Space.Self);

            _pitch += _settings.InvertY ? look.y : -look.y;
            _pitch = Mathf.Clamp(_pitch, _settings.MinPitch, _settings.MaxPitch);
            _cameraPivot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }
    }
}
