using OfficeWarfare.Input;
using UnityEngine;

namespace OfficeWarfare.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InputReader _input;

        public bool IsPaused { get; private set; }

        private void OnEnable()
        {
            _input.PausePressed += TogglePause;
            SetPaused(false);
        }

        private void OnDisable()
        {
            _input.PausePressed -= TogglePause;
            _input.DisableAll();
            Time.timeScale = 1f;
            SetCursorLocked(false);
        }

        public void TogglePause() => SetPaused(!IsPaused);

        public void SetPaused(bool paused)
        {
            IsPaused = paused;
            Time.timeScale = paused ? 0f : 1f;
            SetCursorLocked(!paused);

            if (paused)
                _input.DisableGameplay();
            else
                _input.EnableGameplay();
        }

        private static void SetCursorLocked(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
        }
    }
}
