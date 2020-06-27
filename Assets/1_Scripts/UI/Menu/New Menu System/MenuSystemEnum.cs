namespace Catavaneer.MenuSystem
{
    public enum MenuType
    {
        MainMenu,
        CreditsMenu,
        PauseMenu,
        GameMenu,
        WinMenu,
        LoseMenu,
        SettingsMenu
    }

    public enum TransitionFaderType
    {
        MainMenuTransition,
        StartLevelTransition,
        WinScreenTransition,
        LoseScreenTransition,
        EndGameTransition
    }

    public enum IntendedLevelImage
    {
        Default = default,
        Encounter_01 = 4,
        Encounter_02 = 5,
        Encounter_03 = 6,
    }
}