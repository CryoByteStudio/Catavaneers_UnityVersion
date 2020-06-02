namespace Learning.MenuSystem
{
    public class GameMenu : Menu<GameMenu>
    {
        #region PUBLIC METHODS

        public void OnPausePressed()
        {
            MenuManager.PauseGame();
        }

        #endregion
    }
}