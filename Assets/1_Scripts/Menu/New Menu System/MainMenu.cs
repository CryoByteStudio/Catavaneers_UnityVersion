namespace Catavaneer.MenuSystem
{
    public class MainMenu : Menu<MainMenu>
    {
        public void OnCreditsPressed()
        {
            CreditsMenu.Open();
        }

        public void OnQuitPressed()
        {
            MenuManager.QuitGame();
        }

        public void OnNormalPressed()
        {
            GameManager.SetDifficultyLevel(DifficultyLevel.Normal);
            MenuManager.LoadCharacterSelectScene();
            //MenuManager.PlayGame();
        }

        public void OnIronCatPressed()
        {
            GameManager.SetDifficultyLevel(DifficultyLevel.IronCat);
            MenuManager.LoadCharacterSelectScene();
            //MenuManager.PlayGame();
        }

        public void OnCatpocalypsePressed()
        {
            GameManager.SetDifficultyLevel(DifficultyLevel.Catapocalypse);
            MenuManager.LoadCharacterSelectScene();
            //MenuManager.PlayGame();
        }

        public void OnCatFightPressed()
        {
            GameManager.SetDifficultyLevel(DifficultyLevel.Catfight);
        }

        public void OnApplyPressed()
        {
            // TODO save settings
        }
    }
}