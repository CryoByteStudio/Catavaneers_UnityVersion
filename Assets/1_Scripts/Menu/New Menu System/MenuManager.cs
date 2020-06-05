﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catavaneer.Singleton;
using ViTiet.Utils;
using Catavaneer.LevelManagement;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        // private
        private static Dictionary<TransitionFaderType, TransitionFader> transitionFaderDictionary = new Dictionary<TransitionFaderType, TransitionFader>();
        private static Stack<Menu> menuStack = new Stack<Menu>();
        private static bool isPaused;
        private static bool isInBetweenScene;
        private static Transform menuParent;
        #endregion

        #region UNITY ENGINE FUNCTIONS
        protected override void Awake()
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

            if (((GameManager.Instance && LevelLoader.IsMainMenuLevel()) || LevelLoader.IsDevelopmentLevel()) && !menuParent)
            {
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

        private static IEnumerator RestartLevelTransitionRoutine()
        {
            TransitionFader fader;
            PlayTransition(out fader, TransitionFaderType.StartLevelTransition);
            yield return new WaitForSeconds(fader.FadeOnDuration + fader.DisplayDuration);
            LevelLoader.ReloadLevelAsync(instance);
            OpenMenuPostTransition(GameMenu);
        }

        private static IEnumerator LoadMainMenuLevelTransitionRoutine()
        {
            TransitionFader fader;
            PlayTransition(out fader, TransitionFaderType.MainMenuTransition);
            yield return new WaitForSeconds(fader.FadeOnDuration + fader.DisplayDuration);
            LevelLoader.LoadMainMenuLevelAsync(instance);
            OpenMenuPostTransition(MainMenu);
        }

        private static IEnumerator LoadCharacterSelectLevelTransitionRoutine()
        {
            TransitionFader fader;
            PlayTransition(out fader, TransitionFaderType.MainMenuTransition);
            yield return new WaitForSeconds(fader.FadeOnDuration + fader.DisplayDuration);
            LevelLoader.LoadCharacterSelectLevelAsync(instance);
            CloseAllMenus();
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

        private static IEnumerator OpenLoseMenuRoutine()
        {
            TransitionFader fader = transitionFaderDictionary[TransitionFaderType.LoseScreenTransition];
            TransitionFader.PlayTransition(fader);
            yield return new WaitForSeconds(fader.FadeOnDuration + fader.DisplayDuration);
            OpenMenuPostTransition(LoseMenu);
            yield return new WaitForSeconds(fader.FadeOffDuration);
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

        public static void OpenLoseMenu()
        {
            if (!instance)
            {
                EditorHelper.ArgumentNullException("instance");
                return;
            }

            isInBetweenScene = true;
            instance.StartCoroutine(OpenLoseMenuRoutine());
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
        #endregion
    }
}