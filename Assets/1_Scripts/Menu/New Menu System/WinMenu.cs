namespace Catavaneer.MenuSystem
{
    public class WinMenu : Menu<WinMenu>
    {
        #region PUBLIC METHODS
        public void OnRestartPressed()
        {
            MenuManager.RestartLevel();
        }

        public void OnMainMenuPressed()
        {
            MenuManager.LoadMainMenuLevel();
        }
        #endregion
    }
}