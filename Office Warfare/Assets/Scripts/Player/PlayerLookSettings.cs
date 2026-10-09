using UnityEngine;

namespace OfficeWarfare.Player
{
    [CreateAssetMenu(fileName = "PlayerLookSettings", menuName = "Office Warfare/Player/Look Settings")]
    public class PlayerLookSettings : ScriptableObject
    {
        [Tooltip("Degrees of rotation per pixel of mouse movement.")]
        [SerializeField, Min(0f)] private float _sensitivity = 0.2f;
        [SerializeField] private bool _invertY;

        [Tooltip("How far the camera can look up (negative) and down (positive), in degrees.")]
        [SerializeField, Range(-90f, 0f)] private float _minPitch = -85f;
        [SerializeField, Range(0f, 90f)] private float _maxPitch = 85f;

        public float Sensitivity => _sensitivity;
        public bool InvertY => _invertY;
        public float MinPitch => _minPitch;
        public float MaxPitch => _maxPitch;
    }
}
