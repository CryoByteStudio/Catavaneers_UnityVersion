using UnityEngine;

namespace Catavaneer.MenuSystem
{
    public class GameMenu : Menu<GameMenu>
    {
        [SerializeField] private string pauseButton;

        #region UNITY ENGINE FUNCTIONS
        private void Update()
        {
            if (Input.GetButtonDown(pauseButton) || Input.GetKeyDown(KeyCode.Escape))
            {
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