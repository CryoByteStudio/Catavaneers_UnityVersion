﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using CustomMathLibrary;
using Catavaneer.MenuSystem;
using ViTiet.Utils;

namespace Catavaneer.LevelManagement
{
    public static class LevelLoader
    {
        private static int mainMenuSceneIndex;
        private static int characterSelectSceneIndex;
        private static int firstGameSceneIndex;
        private static float loadingProgress;
        private static bool isLoading = false;

        public static float LoadingProgress { get { return loadingProgress; } }
        public static bool IsLoading { get { return isLoading; } }

        #region PROPERTIES

        private static object objectLock = new object();
        public static event UnityAction<Scene, LoadSceneMode> OnSceneLoaded
        {
            add
            {
                lock (objectLock)
                {
                    SceneManager.sceneLoaded += value;
                }
            }
            remove
            {
                lock (objectLock)
                {
                    SceneManager.sceneLoaded -= value;
                }
            }
        }

        #endregion

        #region PRIVATE STATIC METHODS

        private static IEnumerator LoadLevelAsyncRoutine(string sceneName)
        {
            loadingProgress = 0;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncOperation.isDone)
            {
                loadingProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                yield return null;
            }
            
            isLoading = false;
        }

        private static IEnumerator LoadLevelAsyncRoutine(int sceneIndex)
        {
            loadingProgress = 0;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

            while (!asyncOperation.isDone)
            {
                loadingProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                yield return null;
            }
            
            isLoading = false;
        }

        private static int GetNextLevelIndex()
        {
            return CustomMathf.GetClampedLoopIndex(GetCurrentSceneIndex() + 1, mainMenuSceneIndex, GetLevelCountInBuildSettings());
        }

        #endregion

        #region PUBLIC STATIC METHODS

        public static void SetMainMenuSceneIndex(int index)
        {
            mainMenuSceneIndex = index;
        }

        public static void SetCharacterSelectSceneIndex(int index)
        {
            characterSelectSceneIndex = index;
        }

        public static void SetFirstGameSceneIndex(int index)
        {
            firstGameSceneIndex = index;
        }

        public static int GetCurrentSceneIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }

        public static string GetCurrentSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        public static int GetLevelCountInBuildSettings()
        {
            return SceneManager.sceneCountInBuildSettings;
        }

        public static void LoadLevel(string sceneName)
        {
            if (Application.CanStreamedLevelBeLoaded(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                EditorHelper.LogError(null, "[LevelLoader]: Invalid scene name!");
            }
        }

        public static void LoadLevel(int sceneIndex)
        {
            if (sceneIndex >= 0 && sceneIndex < GetLevelCountInBuildSettings())
            {
                SceneManager.LoadScene(sceneIndex);
            }
            else
            {
                EditorHelper.LogError(null, "[LevelLoader]: Invalid scene index!");
            }
        }

        public static void ReloadLevel()
        {
            LoadLevel(SceneManager.GetActiveScene().name);
        }

        public static void LoadNextLevel()
        {
            LoadLevel(GetNextLevelIndex());
        }

        public static void LoadMainMenuLevel()
        {
            if (mainMenuSceneIndex >= 0 && mainMenuSceneIndex < GetLevelCountInBuildSettings())
            {
                LoadLevel(mainMenuSceneIndex);
            }
            else
            {
                EditorHelper.LogError(null, "[LevelLoader]: Invalid scene index!");
            }
        }

        public static void LoadCharacterSelectLevel()
        {
            if (characterSelectSceneIndex >= 0 && characterSelectSceneIndex < GetLevelCountInBuildSettings())
            {
                LoadLevel(characterSelectSceneIndex);
            }
            else
            {
                EditorHelper.LogError(null, "[LevelLoader]: Invalid scene index!");
            }
        }

        public static void LoadLevelAsync(MonoBehaviour instance, string sceneName)
        {
            if (isLoading) return;

            if (instance)
            {
                isLoading = true;
                instance.StartCoroutine(LoadLevelAsyncRoutine(sceneName));
            }
            else
            {
                EditorHelper.LogError(null, "[LevelLoader]: MonoBehaviour instance is null!");
            }
        }

        public static void LoadLevelAsync(MonoBehaviour instance, int sceneIndex)
        {
            if (isLoading) return;

            if (instance)
            {
                isLoading = true;
                instance.StartCoroutine(LoadLevelAsyncRoutine(sceneIndex));
            }
            else
            {
                EditorHelper.LogError(null, "[LevelLoader]: MonoBehaviour instance is null!");
            }
        }

        public static void ReloadLevelAsync(MonoBehaviour instance)
        {
            LoadLevelAsync(instance, SceneManager.GetActiveScene().name);
        }

        public static void LoadNextLevelAsync(MonoBehaviour instance)
        {
            LoadLevelAsync(instance, GetNextLevelIndex());
        }

        public static void LoadMainMenuLevelAsync(MonoBehaviour instance)
        {
            if (mainMenuSceneIndex >= 0 && mainMenuSceneIndex < GetLevelCountInBuildSettings())
            {
                LoadLevelAsync(instance, mainMenuSceneIndex);
            }
            else
            {
                EditorHelper.LogError(null, "[LevelLoader]: Invalid scene index!");
            }
        }

        public static void LoadCharacterSelectLevelAsync(MonoBehaviour instance)
        {
            if (characterSelectSceneIndex >= 0 && characterSelectSceneIndex < GetLevelCountInBuildSettings())
            {
                LoadLevelAsync(instance, characterSelectSceneIndex);
            }
            else
            {
                EditorHelper.LogError(null, "[LevelLoader]: Invalid scene index!");
            }
        }

        public static bool IsDevelopmentLevel()
        {
            if (GameManager.Instance)
            {
                return SceneManager.GetActiveScene().name.ToLower() == "development";
            }

            EditorHelper.LogError(null, "[LevelLoader]: GameManager instance is null!");
            return false;
        }

        public static bool IsGameLevel()
        {
            if (GameManager.Instance)
            {
                return SceneManager.GetActiveScene().buildIndex >= firstGameSceneIndex && !IsDevelopmentLevel();
            }

            EditorHelper.LogError(null, "[LevelLoader]: GameManager instance is null!");
            return false;
        }

        public static bool IsMainMenuLevel()
        {
            if (MenuManager.Instance)
            {
                return SceneManager.GetActiveScene().buildIndex == mainMenuSceneIndex;
            }

            EditorHelper.LogError(null, "[LevelLoader]: MenuManager instance is null!");
            return false;
        }

        public static bool IsNextLevelGameLevel()
        {
            return GetNextLevelIndex() >= firstGameSceneIndex && GetNextLevelIndex() < GetLevelCountInBuildSettings();
        }

        public static bool IsNextLevelMainMenuLevel()
        {
            return GetNextLevelIndex() == mainMenuSceneIndex;
        }

        #endregion
    }
}