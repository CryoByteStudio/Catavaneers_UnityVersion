namespace Learning.MenuSystem
{
    public class PauseMenu : Menu<PauseMenu>
    {
        #region PUBLIC METHODS

        public void OnResumePressed()
        {
            MenuManager.ResumeGame();
        }

        public void OnSavePressed()
        {
            SaveMenu.Open();
        }

        public void OnLoadPressed()
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

        public override void OnBackPressed()
        {
            MenuManager.QuitGame();
        }

        #endregion
    }
}