using UnityEngine;
using Catavaneer.MenuSystem;

namespace Catavaneer.MenuSystem
{
    public class GameMenu : Menu<GameMenu>
    {
        [SerializeField] private string pauseButton;
        public static bool gameIsPaused = true;
        #region UNITY ENGINE FUNCTIONS
        private void Update()
        {
            if (Input.GetButtonDown(pauseButton) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (!GameManager.Instance.IsGameOver)
                    OnPausePressed();
            }
        }
        #endregion

        #region PUBLIC METHODS
        public void OnPausePressed()
        {
                MenuManager.PauseGame();
        }
        #endregion
    }
}