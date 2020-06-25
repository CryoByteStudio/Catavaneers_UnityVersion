using UnityEngine;
using Catavaneer.MenuSystem;

namespace Catavaneer.MenuSystem
{
    public class GameMenu : Menu<GameMenu>
    {
        [SerializeField] private string pauseButton;
        public static bool gameIsPaused = false;
        #region UNITY ENGINE FUNCTIONS
        private void Update()
        {
            if (Input.GetButtonDown(pauseButton) || Input.GetKeyDown(KeyCode.Escape))
            {
                gameIsPaused = !gameIsPaused;
                OnPausePressed();
            }
        }
        #endregion

        #region PUBLIC METHODS
        public void OnPausePressed()
        {
            if(gameIsPaused)
            {
                MenuManager.PauseGame();
            }
            else
            {
                MenuManager.ResumeGame();
            }
        }
        #endregion
    }
}