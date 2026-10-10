using UnityEngine;

namespace OfficeWarfare.Player
{
    [CreateAssetMenu(fileName = "ArmsAnimationSettings", menuName = "Office Warfare/Player/Arms Animation Settings")]
    public class ArmsAnimationSettings : ScriptableObject
    {
        [Tooltip("Seconds to blend between idle and run when speed changes.")]
        [SerializeField, Min(0f)] private float _speedDampTime = 0.08f;
        [Tooltip("Playback speed of the attack animation. Higher feels snappier.")]
        [SerializeField, Range(0.1f, 3f)] private float _attackSpeed = 1.25f;

        public float SpeedDampTime => _speedDampTime;
        public float AttackSpeed => _attackSpeed;
    }
}
