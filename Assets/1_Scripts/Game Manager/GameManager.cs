using Catavaneer.LevelManagement;
using Catavaneer.Singleton;
using ViTiet.Utils;
using ObjectPooling;
using SpawnSystem;
using System.Collections;
using UnityEngine;
//using UnityEngine.SceneManagement;
using Catavaneer.MenuSystem;

namespace Catavaneer
{
    public class GameManager : SingletonEntity<GameManager>
    {
        [SerializeField] private int mainMenuSceneIndex = 0;
        [SerializeField] private int characterSelectSceneIndex = 0;
        [SerializeField] private int firstGameSceneIndex = 0;
        [SerializeField] private int catFightSceneIndex = 0;

        public float startDelay = 2;
        public float quitDelay = 4;

        private bool doneOnce = false;

        private DifficultyLevel difficultyLevel;
        public DifficultyLevel DifficultyLevel { get { return instance.difficultyLevel; } }
        private bool isGameOver;
        public bool IsGameOver { get { return isGameOver; } }
        private bool hasFinishedAllLevel = false;
        public bool HasFinishedAllLevel { get { return hasFinishedAllLevel; } }
        public int FirstGameSceneIndex { get { return firstGameSceneIndex; } }
        
        public static int LastEncounterIndex = 0;
        public static int CurrentDay = 0;

        protected override void Awake()
        {
            base.Awake();
            Reset();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            HealthComp.OnCaravanDestroyed -= OnCaravanDestroyedHandler;
        }

        private void Update()
        {
            isGameOver = CheckObjectiveCondition();

            if (!doneOnce && isGameOver)
            {
                if (LevelLoader.IsGameLevel() && LevelLoader.GetCurrentSceneIndex() < catFightSceneIndex)
                {
                    if (FindObjectOfType<CaravanDamage>()){

                        FindObjectOfType<CaravanDamage>().PlayVictory();

                    }
                    StartCoroutine(WinDelay());
                }
                else
                {
                    ResetCampaignParams();
                    hasFinishedAllLevel = true;
                    MenuManager.LoadMainMenuLevel(true);
                }

                doneOnce = true;
            }
        }

        protected override void SceneLoadedHandler(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            base.SceneLoadedHandler(scene, mode);

            if (LevelLoader.IsGameLevel())
            {
                Reset();

                if (LevelLoader.GetCurrentSceneIndex() < catFightSceneIndex)
                    StartCoroutine(StartDelay());
            }
            else if (hasFinishedAllLevel)
            {
                MenuManager.OpenMenu(MenuManager.CreditsMenu);
                hasFinishedAllLevel = false;
            }
        }

        private static void ResetCampaignParams()
        {
            CurrentDay = 0;
            LastEncounterIndex = 0;
        }

        private void Reset()
        {
            if (gameObject.activeSelf && FindCaravanHealthComp())
                HealthComp.OnCaravanDestroyed += OnCaravanDestroyedHandler;

            LevelLoader.SetMainMenuSceneIndex(mainMenuSceneIndex);
            LevelLoader.SetCharacterSelectSceneIndex(characterSelectSceneIndex);
            LevelLoader.SetFirstGameSceneIndex(firstGameSceneIndex);
            isGameOver = false;
            doneOnce = false;
        }

        /// <summary>
        /// Find Caravan health component in scene. Not optimized to use every frame.
        /// </summary>
        /// <returns></returns>
        private static HealthComp FindCaravanHealthComp()
        {
            HealthComp[] healthComps = FindObjectsOfType<HealthComp>();

            if (healthComps != null && healthComps.Length > 0)
            {
                foreach (HealthComp healthComp in healthComps)
                {
                    if (healthComp.myClass == CharacterClass.Caravan)
                    {
                        return healthComp;
                    }
                }
            }

            return null;
        }

        private void OnCaravanDestroyedHandler(HealthComp healthComp)
        {
            StartCoroutine(LoseDelay());
        }

        private bool CheckObjectiveCondition()
        {
            int currentSceneIndex = LevelLoader.GetCurrentSceneIndex();

            // if PVE mode
            if (LevelLoader.IsGameLevel() && currentSceneIndex < catFightSceneIndex)
                return SpawnManager.HasFinishedSpawning && SpawnManager.EnemiesAlive <= 0;
            // if PVP mode
            else if (currentSceneIndex >= catFightSceneIndex)
                return Goldbag.HasWinner;

            return false;
        }

        private IEnumerator LoseDelay()
        {
            ResetCampaignParams();
            yield return new WaitForSeconds(quitDelay);
            MenuManager.OpenLoseMenu();
        }

        private IEnumerator WinDelay()
        {
            yield return new WaitForSeconds(quitDelay);
            MenuManager.LoadCampaignLevel();
        }

        public IEnumerator StartDelay()
        {
            SpawnManager.CanSpawnEnemy = false;
            yield return new WaitForSeconds(startDelay);
            SpawnManager.CanSpawnEnemy = true;
        }

        public static void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public static void SetDifficultyLevel(DifficultyLevel difficultyLevel)
        {
            if (!instance)
            {
                EditorHelper.ArgumentNullException("instance");
                return;
            }

            instance.difficultyLevel = difficultyLevel;
        }

        //below section edit by Will
        //public void ToMainMenu()
        //{
        //    //doneOnce = true;
        //    if (caravan_HC != null) caravan_HC.SetIsDead(false);
        //    SceneManager.LoadScene(0);
        //    ObjectPooler.DisableAllActiveObjects();
        //}

        //public void ToPlayerSelectionScene()
        //{
        //    //doneOnce = true;
        //    if (caravan_HC != null) caravan_HC.SetIsDead(false);
        //    ObjectPooler.DisableAllActiveObjects();

        //    SceneManager.LoadScene("Menu_CharacterSelect");
        //}

        //public void LoadLevel(string leveltoload)
        //{
        //    Debug.Log("Loading: " + leveltoload);
        //    SceneManager.LoadScene(leveltoload);
        //}

        //public void StartSceneButton()
        //{
        //    SceneManager.LoadScene("Menu_CharacterSelect");
        //    Debug.Log("normal");
        //    FindObjectOfType<GameDifficultyManager>().dif = DifficultyLevel.Normal;
        //}
        //public void StartIroncatButton()
        //{
        //    SceneManager.LoadScene("Menu_CharacterSelect");
        //    Debug.Log("ironcat");
        //    FindObjectOfType<GameDifficultyManager>().dif = DifficultyLevel.IronCat;
        //}
        //public void StartCatfightButton()
        //{
        //    SceneManager.LoadScene("Catfight_01");
        //    Debug.Log("Catfight");
        //    FindObjectOfType<GameDifficultyManager>().dif = DifficultyLevel.Catfight;

        //}
        //public void StartCatpocalypseButton()
        //{
        //    SceneManager.LoadScene("Menu_CharacterSelect");
        //    Debug.Log("catpoc");
        //    FindObjectOfType<GameDifficultyManager>().dif = DifficultyLevel.Catapocalypse;
        //}

        //public void CreditsSceneButton()
        //{
        //    LoadLevel("Menu_Credits");
        //}

        //public void QuitApp()
        //{
        //    Debug.Log("Quitting");
        //    Application.Quit();
        //}
    }
}
