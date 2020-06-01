using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Learning.Singleton;
using Learning.LevelManagement;
using Learning.Utils;

namespace Learning.MenuSystem
{
    [System.Serializable]
    public struct MenuData
    {
        public MenuType type;
        public Menu menu;
    }

    public class MenuManager : SingletonEntity<MenuManager>
    {
        [SerializeField] private List<TransitionFader> transitionFaderList = new List<TransitionFader>();
        [SerializeField] private List<MenuData> menuDataList = new List<MenuData>();

        public static Menu MainMenu { get { return MenuSystem.MainMenu.Instance; } }
        public static Menu LevelSelectMenu { get { return MenuSystem.LevelSelectMenu.Instance; } }
        public static Menu SaveMenu { get { return MenuSystem.SaveMenu.Instance; } }
        public static Menu LoadMenu { get { return MenuSystem.LoadMenu.Instance; } }
        public static Menu SettingsMenu { get { return MenuSystem.SettingsMenu.Instance; } }
        public static Menu CreditsMenu { get { return MenuSystem.CreditsMenu.Instance; } }
        public static Menu PauseMenu { get { return MenuSystem.PauseMenu.Instance; } }
        public static Menu GameMenu { get { return MenuSystem.GameMenu.Instance; } }
        public static Menu WinMenu { get { return MenuSystem.WinMenu.Instance; } }
        
        private static Dictionary<TransitionFaderType, TransitionFader> transitionFaderDictionary = new Dictionary<TransitionFaderType, TransitionFader>();
        private static Stack<Menu> menuStack = new Stack<Menu>();
        private static bool isPaused;
        private static bool isInBetweenScene;
        private static Transform menuParent;

        #region UNITY ENGINE FUNCTIONS

        override protected void Awake()
        {
            base.Awake();

            Reset();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (LevelLoader.IsGameLevel() && !isInBetweenScene)
                {
                    if (!isPaused)
                    {
                        PauseGame();
                    }
                    else
                    {
                        ResumeGame();
                    }
                }
                else if (LevelLoader.IsMainMenuLevel())
                {
                    if (menuStack.Count > 1)
                    {
                        CloseMenu();
                    }
                }
            }
        }

        #endregion

        #region PRIVATE METHODS

        protected override void SceneLoadedHandler(Scene scene, LoadSceneMode mode)
        {
            Reset();
        }

        private void InitializeMenus()
        {
            menuParent = new GameObject("Menus").transform;
            DontDestroyOnLoad(menuParent.gameObject);

            Menu menuInstance = null;

            foreach (MenuData menuData in menuDataList)
            {
                if (menuData.menu)
                {
                    menuInstance = Instantiate(menuData.menu, menuParent.transform);

                    if (menuData.type != (LevelLoader.IsGameLevel() ? MenuType.GameMenu : MenuType.MainMenu))
                    {
                        menuInstance.gameObject.SetActive(false);
                    }
                    else
                    {
                        OpenMenu(menuInstance);
                    }
                }
            }
        }

        private void InitializeTransitions()
        {
            foreach (TransitionFader transitionFader in transitionFaderList)
            {
                if (transitionFader && !transitionFaderDictionary.ContainsKey(transitionFader.Type))
                {
                    SetTransitionText(transitionFader);
                    transitionFaderDictionary.Add(transitionFader.Type, transitionFader);
                }
            }
        }

        private void SetTransitionText(TransitionFader transitionFader)
        {
            switch (transitionFader.Type)
            {
                case TransitionFaderType.MainMenuTransition:
                    transitionFader.SetTransitionText("");
                    break;
                case TransitionFaderType.StartLevelTransition:
                    transitionFader.SetTransitionText("READY");
                    break;
                case TransitionFaderType.WinScreenTransition:
                    transitionFader.SetTransitionText("YOU WIN");
                    break;
                case TransitionFaderType.EndGameTransition:
                    transitionFader.SetTransitionText("GAME OVER");
                    break;
            }
        }

        private void Reset()
        {
            if (transitionFaderDictionary.Count == 0)
            {
                InitializeTransitions();
            }

            if (((GameManager.Instance && LevelLoader.IsMainMenuLevel()) || LevelLoader.IsDevelopmentLevel()) && !menuParent)
            {
                InitializeMenus();
            }
            
            isPaused = false;
            isInBetweenScene = false;
        }

        #endregion

        #region PRIVATE STATIC METHODS
        
        private static void PlayTransition(out TransitionFader fader, TransitionFaderType type)
        {
            UnPaused();
            fader = transitionFaderDictionary[type];
            TransitionFader.PlayTransition(fader);
        }

        private static void OpenMenuPostTransition(Menu menu)
        {
            CloseAllMenus();
            OpenMenu(menu);
        }

        private static IEnumerator PlayGameTransitionRoutine()
        {
            TransitionFader fader;
            PlayTransition(out fader, TransitionFaderType.StartLevelTransition);
            yield return new WaitForSeconds(fader.FadeOnDuration + fader.DisplayDuration);
            LevelLoader.LoadLevelAsync(instance, GameManager.Instance.FirstGameSceneIndex);
            OpenMenuPostTransition(GameMenu);
        }

        private static IEnumerator LoadNextLevelTransitionRoutine()
        {
            TransitionFader fader;

            if (LevelLoader.IsNextLevelGameLevel())
            {
                PlayTransition(out fader, TransitionFaderType.StartLevelTransition);
                yield return new WaitForSeconds(fader.FadeOnDuration + fader.DisplayDuration);
                LevelLoader.LoadNextLevelAsync(instance);
                OpenMenuPostTransition(GameMenu);
            }
            else if (LevelLoader.IsNextLevelMainMenuLevel())
            {
                PlayTransition(out fader, TransitionFaderType.EndGameTransition);
                yield return new WaitForSeconds(fader.FadeOnDuration + fader.DisplayDuration);
                LevelLoader.LoadMainMenuLevelAsync(instance);
                OpenMenuPostTransition(MainMenu);
            }

            yield return null;
        }

        private static IEnumerator LoadLevelTransitionRoutine(int sceneIndex)
        {
            TransitionFader fader;
            PlayTransition(out fader, TransitionFaderType.StartLevelTransition);
            yield return new WaitForSeconds(fader.FadeOnDuration + fader.DisplayDuration);
            LevelLoader.LoadLevelAsync(instance, sceneIndex);
            OpenMenuPostTransition(GameMenu);
        }

        private static IEnumerator LoadLevelTransitionRoutine(string sceneName)
        {
            TransitionFader fader;
            PlayTransition(out fader, TransitionFaderType.StartLevelTransition);
            yield return new WaitForSeconds(fader.FadeOnDuration + fader.DisplayDuration);
            LevelLoader.LoadLevelAsync(instance, sceneName);
            OpenMenuPostTransition(GameMenu);
        }

        private static IEnumerator RestartLevelTransitionRoutine()
        {
            TransitionFader fader;
            PlayTransition(out fader, TransitionFaderType.StartLevelTransition);
            yield return new WaitForSeconds(fader.FadeOnDuration + fader.DisplayDuration);
            LevelLoader.ReloadLevelAsync(instance);
            OpenMenuPostTransition(GameMenu);
        }

        private static IEnumerator LoadMainMenuLevelTransitionRoutine(bool isTransitionFromLastLevel)
        {
            TransitionFader fader;

            if (!isTransitionFromLastLevel)
            {
                PlayTransition(out fader, TransitionFaderType.MainMenuTransition);
                yield return new WaitForSeconds(fader.FadeOnDuration + fader.DisplayDuration);
            }
            else
            {
                PlayTransition(out fader, TransitionFaderType.EndGameTransition);
                yield return new WaitForSeconds(fader.FadeOnDuration + fader.DisplayDuration);
            }

            LevelLoader.LoadMainMenuLevelAsync(instance);
            OpenMenuPostTransition(MainMenu);
        }

        private static IEnumerator OpenWinMenuRoutine()
        {
            TransitionFader fader = transitionFaderDictionary[TransitionFaderType.WinScreenTransition];
            TransitionFader.PlayTransition(fader);
            yield return new WaitForSeconds(fader.FadeOnDuration + fader.DisplayDuration);
            OpenMenuPostTransition(WinMenu);
            yield return new WaitForSeconds(fader.FadeOffDuration);
            Pause();
        }

        private static void DisablePlayerControl()
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.EnablePlayerController(false);
            }
        }

        private static void EnablePlayerControl()
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.EnablePlayerController(true);
            }
        }

        #endregion

        #region PUBLIC STATIC METHODS

        public static void OpenMenu(Menu menu)
        {
            if (!menu)
            {
                EditorHelper.LogWarning(null, "[MenuManager]: Invalid menu!");
                return;
            }

            if (menuStack.Count > 0)
            {
                foreach (Menu m in menuStack)
                {
                    m.gameObject.SetActive(false);
                }
            }

            menu.gameObject.SetActive(true);
            menuStack.Push(menu);
        }

        public static void CloseMenu()
        {
            if (menuStack.Count == 0)
            {
                EditorHelper.LogWarning(null, "[MenuManager]: No menu in menu stack!");
                return;
            }

            menuStack.Pop().gameObject.SetActive(false);

            if (menuStack.Count > 0)
            {
                menuStack.Peek().gameObject.SetActive(true);
            }
        }

        public static void CloseAllMenus()
        {
            while (menuStack.Count > 0)
            {
                CloseMenu();
            }
        }

        public static void PlayGame()
        {
            if (GameManager.Instance)
            {
                if (instance)
                {
                    instance.StartCoroutine(PlayGameTransitionRoutine());
                    return;
                }

                EditorHelper.LogError(null, "[MenuManager]: Instance is null!");
                return;
            }

            LogNoGameManagerError();
        }

        public static void PauseGame()
        {
            DisablePlayerControl();
            Pause();
            CloseAllMenus();
            OpenMenu(PauseMenu);
        }

        public static void ResumeGame()
        {
            EnablePlayerControl();
            UnPaused();
            CloseAllMenus();
            OpenMenu(GameMenu);
        }

        public static void OpenWinMenu()
        {
            isInBetweenScene = true;

            if (instance)
            {
                instance.StartCoroutine(OpenWinMenuRoutine());
            }
        }

        public static void QuitGame()
        {
            if (GameManager.Instance)
            {
                GameManager.QuitGame();
                return;
            }

            LogNoGameManagerError();
        }

        public static void LoadLevel(int sceneIndex)
        {
            if (GameManager.Instance)
            {
                if (instance)
                {
                    instance.StartCoroutine(LoadLevelTransitionRoutine(sceneIndex));
                    return;
                }

                EditorHelper.LogError(null, "[MenuManager]: Instance is null!");
                return;
            }

            LogNoGameManagerError();
        }

        public static void LoadLevel(string sceneName)
        {
            if (GameManager.Instance)
            {
                if (instance)
                {
                    instance.StartCoroutine(LoadLevelTransitionRoutine(sceneName));
                    return;
                }

                EditorHelper.LogError(null, "[MenuManager]: Instance is null!");
                return;
            }

            LogNoGameManagerError();
        }

        public static void LoadNextLevel()
        {
            if (GameManager.Instance)
            {
                if (instance)
                {
                    instance.StartCoroutine(LoadNextLevelTransitionRoutine());
                    return;
                }

                EditorHelper.LogError(null, "[MenuManager]: Instance is null!");
                return;
            }

            LogNoGameManagerError();
        }

        public static void RestartLevel()
        {
            if (instance)
            {
                instance.StartCoroutine(RestartLevelTransitionRoutine());
                return;
            }

            EditorHelper.LogError(null, "[MenuManager]: Instance is null!");
        }

        public static void LoadMainMenuLevel(bool isTransitionFromLastLevel)
        {
            if (instance)
            {
                instance.StartCoroutine(LoadMainMenuLevelTransitionRoutine(isTransitionFromLastLevel));
                return;
            }

            EditorHelper.LogError(null, "[MenuManager]: Instance is null!");
        }

        #endregion

        #region PRIVATE STATIC METHODS

        private static void Pause()
        {
            Time.timeScale = 0;
            isPaused = true;
        }

        private static void UnPaused()
        {
            Time.timeScale = 1;
            isPaused = false;
        }

        private static void LogNoGameManagerError()
        {
            EditorHelper.LogError(null, "[MenuManager]: GameManager instance is null!");
        }

        #endregion
    }
}