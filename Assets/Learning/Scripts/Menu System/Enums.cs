namespace Learning
{
    public enum MenuType
    {
        MainMenu,
        LevelSelectMenu,
        SaveMenu,
        LoadMenu,
        SettingsMenu,
        CreditsMenu,
        PauseMenu,
        GameMenu,
        WinMenu
    }

    public enum TransitionFaderType
    {
        MainMenuTransition,
        StartLevelTransition,
        WinScreenTransition,
        EndGameTransition
    }

    public enum GameTag
    {
        Player,
        Object
    }
}