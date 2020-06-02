namespace Learning.MenuSystem
{
    public class MainMenu : Menu<MainMenu>
    {
        #region PUBLIC METHODS

        public void OnPlayPressed()
        {
            MenuManager.PlayGame();
        }

        public void OnSelectMissionPressed()
        {
            LevelSelectMenu.Open();
        }

        public void OnLoadPressed()
        {
            LoadMenu.Open();
        }

        public void OnSettingsPressed()
        {
            SettingsMenu.Open();
        }

        public void OnCreditsPressed()
        {
            CreditsMenu.Open();
        }

        override public void OnBackPressed()
        {
            MenuManager.QuitGame();
        }

        #endregion
    }

}