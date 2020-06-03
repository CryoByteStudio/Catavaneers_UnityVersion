namespace Catavaneer.MenuSystem
{
    #region PUBLIC METHODS
    public class GameMenu : Menu<GameMenu>
    {
        public void OnPausePressed()
        {
            MenuManager.PauseGame();
        }
    }
    #endregion
}