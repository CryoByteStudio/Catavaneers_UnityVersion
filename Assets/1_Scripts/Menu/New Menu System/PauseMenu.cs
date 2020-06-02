namespace Catavaneer.MenuSystem
{
    public class PauseMenu : Menu<PauseMenu>
    {
        #region PUBLIC METHODS
        public void OnResumePressed()
        {
            MenuManager.ResumeGame();
        }

        public void OnRestartPressed()
        {
            MenuManager.RestartLevel();
        }

        public void OnMainMenuPressed()
        {
            MenuManager.LoadMainMenuLevel(false);
        }
        #endregion
    }
}