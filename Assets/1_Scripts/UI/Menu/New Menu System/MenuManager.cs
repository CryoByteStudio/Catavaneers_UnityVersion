using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catavaneer.Singleton;
using ViTiet.Utils;
using Catavaneer.LevelManagement;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace Catavaneer.MenuSystem
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

        #region STATIC FIELDS
        // public
        public static Menu MainMenu { get { return MenuSystem.MainMenu.Instance; } }
        public static Menu CreditsMenu { get { return MenuSystem.CreditsMenu.Instance; } }
        public static Menu PauseMenu { get { return MenuSystem.PauseMenu.Instance; } }
        public static Menu GameMenu { get { return MenuSystem.GameMenu.Instance; } }
        public static Menu WinMenu { get { return MenuSystem.WinMenu.Instance; } }
        public static Menu LoseMenu { get { return MenuSystem.LoseMenu.Instance; } }
        public static Menu SettingsMenu { get { return MenuSystem.SettingsMenu.Instance; } }

        // private
        private static Dictionary<TransitionFaderType, TransitionFader> transitionFaderDictionary = new Dictionary<TransitionFaderType, TransitionFader>();
        private static Stack<Menu> menuStack = new Stack<Menu>();
        private static bool isPaused;
        private static bool isInBetweenScene;
        private static Transform menuParent;
        private static TransitionFader fader;

        [SerializeField] private LoseTransitionFader loseTransitionFader;

        #endregion

        #region UNITY ENGINE FUNCTIONS
        protected override void Awake()
        {
            base.Awake();
            Reset();

            if (!loseTransitionFader)
                loseTransitionFader = FindObjectOfType<LoseTransitionFader>();
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    if (LevelLoader.IsGameLevel() && !isInBetweenScene)
            //    {
            //        if (!isPaused)
            //        {
            //            PauseGame();
            //        }
            //        else
            //        {
            //            ResumeGame();
            //        }
            //    }
            //    else if (LevelLoader.IsMainMenuLevel())
            //    {
            //        if (menuStack.Count > 1)
            //        {
            //            CloseMenu();
            //        }
            //    }
            //}
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
                case TransitionFaderType.LoseScreenTransition:
                    transitionFader.SetTransitionText("YOU LOSE");
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

            if (GameManager.Instance)
            {
                if ((LevelLoader.IsMainMenuLevel() || LevelLoader.IsDevelopmentLevel()) && !menuParent)
                    InitializeMenus();
            }

            isPaused = false;
            isInBetweenScene = false;
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

        private static void PlayTransition(out TransitionFader fader, TransitionFaderType type)
        {
            UnPaused();
            fader = transitionFaderDictionary[type];
            //if (type == TransitionFaderType.LoseScreenTransition)
            //    TransitionFader.PlayTransition(fader);
            //else
            fader.PlayTransition(LevelLoader.GetCurrentSceneIndex(), fader);
        }

        private static void OpenMenuPostTransition(Menu menu)
        {
            CloseAllMenus();
            OpenMenu(menu);
        }

        private static IEnumerator PlayPreTransitionRoutine(TransitionFaderType transitionFaderType)
        {
            PlayTransition(out fader, transitionFaderType);
            yield return new WaitForSeconds(fader.FadeOnDuration);
        }

        private static IEnumerator PlayePostTransitionRoutine()
        {
            yield return new WaitForSeconds(fader.DisplayDuration);
            fader = null;
        }

        private static IEnumerator PlayGameTransitionRoutine()
        {
            yield return PlayPreTransitionRoutine(TransitionFaderType.StartLevelTransition);
            LevelLoader.LoadLevelAsync(instance, GameManager.Instance.FirstGameSceneIndex);
            yield return PlayePostTransitionRoutine();
            OpenMenuPostTransition(GameMenu);
        }

        private static IEnumerator RestartLevelTransitionRoutine()
        {
            yield return PlayPreTransitionRoutine(TransitionFaderType.StartLevelTransition);
            LevelLoader.ReloadLevelAsync(instance);
            yield return PlayePostTransitionRoutine();
            OpenMenuPostTransition(GameMenu);
        }

        private static IEnumerator LoadCampaignLevelRoutine()
        {
            if (!LevelLoader.IsGameLevel())
                yield return PlayPreTransitionRoutine(TransitionFaderType.MainMenuTransition);
            else
                yield return PlayPreTransitionRoutine(TransitionFaderType.WinScreenTransition);
            LevelLoader.LoadLevelAsync(instance, "Campaign");
            yield return PlayePostTransitionRoutine();
            CloseAllMenus();
            //OpenMenuPostTransition(GameMenu);
        }

        private static IEnumerator LoadGameLevelRoutine(int levelIndex)
        {
            yield return PlayPreTransitionRoutine(TransitionFaderType.StartLevelTransition);
            LevelLoader.LoadLevelAsync(instance, levelIndex);
            yield return PlayePostTransitionRoutine();
            OpenMenuPostTransition(GameMenu);
        }

        private static IEnumerator LoadGameLevelRoutine(string levelName)
        {
            yield return PlayPreTransitionRoutine(TransitionFaderType.StartLevelTransition);
            LevelLoader.LoadLevelAsync(instance, levelName);
            yield return PlayePostTransitionRoutine();
            OpenMenuPostTransition(GameMenu);
        }

        private static IEnumerator LoadMainMenuLevelTransitionRoutine()
        {
            yield return PlayPreTransitionRoutine(TransitionFaderType.MainMenuTransition);
            LevelLoader.LoadMainMenuLevelAsync(instance);
            yield return PlayePostTransitionRoutine();
            OpenMenuPostTransition(MainMenu);
        }

        private static IEnumerator LoadCharacterSelectLevelTransitionRoutine()
        {
            yield return PlayPreTransitionRoutine(TransitionFaderType.MainMenuTransition);
            LevelLoader.LoadCharacterSelectLevelAsync(instance);
            yield return PlayePostTransitionRoutine();
            CloseAllMenus();
        }

        private static IEnumerator LoadMainMenuLevelTransitionRoutine(bool isTransitionFromLastLevel)
        {
            if (!isTransitionFromLastLevel)
            {
                yield return PlayPreTransitionRoutine(TransitionFaderType.MainMenuTransition);
            }
            else
            {
                yield return PlayPreTransitionRoutine(TransitionFaderType.EndGameTransition);
            }

            LevelLoader.LoadMainMenuLevelAsync(instance);
            yield return PlayePostTransitionRoutine();
            OpenMenuPostTransition(MainMenu);
            OpenMenu(CreditsMenu);
        }

        private static IEnumerator OpenWinMenuRoutine()
        {
            yield return PlayPreTransitionRoutine(TransitionFaderType.WinScreenTransition);
            OpenMenuPostTransition(WinMenu);
            yield return PlayePostTransitionRoutine();
            Pause();
        }

        private static IEnumerator OpenLoseMenuRoutine()
        {
            yield return PlayPreTransitionRoutine(TransitionFaderType.LoseScreenTransition);
            OpenMenuPostTransition(LoseMenu);
            yield return PlayePostTransitionRoutine();
            Pause();
        }
        #endregion

        #region PUBLIC STATIC METHODS
        public static void OpenMenu(Menu menu)
        {
            if (!menu)
            {
                EditorHelper.ArgumentNullException("menu");
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

            if (menu.selectedGameObject)
            {
                EventSystem.current.SetSelectedGameObject(menu.selectedGameObject);
                menu.selectedGameObject.GetComponent<Selectable>().OnSelect(new BaseEventData(EventSystem.current));
            }
        }

        public static void CloseMenu()
        {
            if (menuStack.Count == 0)
            {
                if (instance)
                {
                    EditorHelper.LogWarning(instance, "No menu in stack.");
                    return;
                }
            }

            Menu menu = menuStack.Pop();
            menu.gameObject.SetActive(false);

            if (menuStack.Count > 0)
            {
                menu = menuStack.Peek();
                menu.gameObject.SetActive(true);
            }

            if (menu.selectedGameObject)
            {
                EventSystem.current.SetSelectedGameObject(menu.selectedGameObject);
                menu.selectedGameObject.GetComponent<Selectable>().OnSelect(new BaseEventData(EventSystem.current));
            }
        }

        public static void CloseAllMenus()
        {
            while (menuStack.Count > 0)
            {
                CloseMenu();
            }
        }

        public static void LoadCharacterSelectScene()
        {
            if (!instance)
            {
                EditorHelper.ArgumentNullException("instance");
                return;
            }

            instance.StartCoroutine(LoadCharacterSelectLevelTransitionRoutine());
        }

        public static void PlayGame()
        {
            if (!instance)
            {
                EditorHelper.ArgumentNullException("instance");
                return;
            }

            instance.StartCoroutine(PlayGameTransitionRoutine());
        }

        public static void PauseGame()
        {
            //DisablePlayerControl();
            Pause();
            CloseAllMenus();
            OpenMenu(PauseMenu);
        }

        public static void ResumeGame()
        {
            //EnablePlayerControl();
            UnPaused();
            CloseAllMenus();
            OpenMenu(GameMenu);
        }

        public static void OpenGameMenu()
        {
            OpenMenu(GameMenu);
        }

        public static void OpenSettingsMenu()
        {
            OpenMenu(SettingsMenu);
        }

        public static void OpenCreditsMenu()
        {
            if (!instance)
            {
                EditorHelper.ArgumentNullException("instance");
                return;
            }

            isInBetweenScene = true;
            instance.StartCoroutine(OpenWinMenuRoutine());
        }

        public static void OpenWinMenu()
        {
            if (!instance)
            {
                EditorHelper.ArgumentNullException("instance");
                return;
            }

            isInBetweenScene = true;
            instance.StartCoroutine(OpenWinMenuRoutine());
        }

        [Obsolete]
        public static void OpenLoseMenuDepricated()
        {
            if (!instance)
            {
                EditorHelper.ArgumentNullException("instance");
                return;
            }

            //isInBetweenScene = true;
            instance.StartCoroutine(OpenLoseMenuRoutine());
        }

        public void OpenLoseMenu()
        {
            loseTransitionFader.gameObject.SetActive(true);
            loseTransitionFader.Play();
        }

        public static void QuitGame()
        {
            if (!GameManager.Instance)
            {
                EditorHelper.ArgumentNullException("GameManager.Instance");
                return;
            }

            GameManager.QuitGame();
        }

        public static void RestartLevel()
        {
            if (!instance)
            {
                EditorHelper.ArgumentNullException("instance");
                return;
            }

            instance.StartCoroutine(RestartLevelTransitionRoutine());
        }

        public static void LoadMainMenuLevel()
        {
            if (!instance)
            {
                EditorHelper.ArgumentNullException("instance");
                return;
            }

            instance.StartCoroutine(LoadMainMenuLevelTransitionRoutine());
        }

        public static void LoadMainMenuLevel(bool isTransitionFromLastLevel)
        {
            if (!instance)
            {
                EditorHelper.ArgumentNullException("instance");
                return;
            }

            instance.StartCoroutine(LoadMainMenuLevelTransitionRoutine(isTransitionFromLastLevel));
        }

        public static void LoadCampaignLevel()
        {
            if (!instance)
            {
                EditorHelper.ArgumentNullException("instance");
                return;
            }

            instance.StartCoroutine(LoadCampaignLevelRoutine());
        }

        public static void LoadGameLevel(int levelIndex)
        {
            if (!instance)
            {
                EditorHelper.ArgumentNullException("instance");
                return;
            }

            instance.StartCoroutine(LoadGameLevelRoutine(levelIndex));
        }

        public static void LoadGameLevel(string levelName)
        {
            if (!instance)
            {
                EditorHelper.ArgumentNullException("instance");
                return;
            }

            instance.StartCoroutine(LoadGameLevelRoutine(levelName));
        }
        #endregion
    }
}