namespace Learning.MenuSystem
{
    public class WinMenu : Menu<WinMenu>
    {
        #region PUBLIC METHODS

        public void OnNextLevelPressed()
        {
            MenuManager.LoadNextLevel();
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