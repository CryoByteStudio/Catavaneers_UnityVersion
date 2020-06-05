using Catavaneer.LevelManagement;
using Catavaneer.Singleton;
using ViTiet.Utils;
using ObjectPooling;
using SpawnSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Catavaneer.MenuSystem;

namespace Catavaneer
{
    public class GameManager : SingletonEntity<GameManager>
    {
        [SerializeField] private int mainMenuSceneIndex = 0;
        [SerializeField] private int characterSelectSceneIndex = 0;
        [SerializeField] private int firstGameSceneIndex = 0;
        [SerializeField] private DifficultyLevel difficultyLevel;

        public static DifficultyLevel DifficultyLevel { get { return instance.difficultyLevel; } }
        public float startDelay = 1;
        public float quitDelay = 0;
        private bool doneOnce = false;

        private bool isGameOver;
        public bool IsGameOver { get { return isGameOver; } }

        public int FirstGameSceneIndex { get { return firstGameSceneIndex; } }

        [SerializeField] HealthComp caravan_HC;

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
            //if (Input.GetKeyDown(KeyCode.C)) { difficulty = DifficultyLevel.IronCat; }

            //if (!caravan_HC)
            //{
            //    HealthComp[] healthComps = FindObjectsOfType<HealthComp>();
            //    foreach (HealthComp hc in healthComps)
            //    {
            //        if (hc.myClass == CharacterClass.Caravan)
            //        {
            //            caravan_HC = hc;
            //            break;
            //        }
            //    }
            //    //Debug.Log("NO Caravan attached to game manager");
            //}

            //if (caravan_HC)
            //{
            //    // always start coroutine once in update
            //    if (caravan_HC.IsDead() && !doneOnce)
            //    {
            //        doneOnce = true;
            //        caravan_HC.SetIsDead(false);

            //        StartCoroutine(RestartLevel());

            //        transform.MyExtensionFunction();
            //    }
            //}

            if (Input.anyKeyDown && SceneManager.GetActiveScene().name == "Menu_Credits")
            {
                SceneManager.LoadScene("Menu_Main");
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton6))
            {
                ScreenCapture.CaptureScreenshot("ScreenShot_" + System.DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss") + ".JPG");
            }
        }

        protected override void SceneLoadedHandler(Scene scene, LoadSceneMode mode)
        {
            base.SceneLoadedHandler(scene, mode);

            if (LevelLoader.IsGameLevel())
            {
                Reset();
            }
        }

        private IEnumerator RestartLevel()
        {
            ObjectPooler.DisableAllActiveObjects();
            yield return new WaitForSeconds(quitDelay);
            //string curScene = SceneManager.GetActiveScene().name;
            Reset();
            SceneManager.LoadScene("Menu_LoseScene");
            //StartCoroutine(StartDelay());
        }

        private void Reset()
        {
            HealthComp.OnCaravanDestroyed += OnCaravanDestroyedHandler;
            LevelLoader.SetMainMenuSceneIndex(mainMenuSceneIndex);
            LevelLoader.SetCharacterSelectSceneIndex(characterSelectSceneIndex);
            LevelLoader.SetFirstGameSceneIndex(firstGameSceneIndex);
            isGameOver = false;
            doneOnce = false;
        }

        private void OnCaravanDestroyedHandler()
        {
            StartCoroutine(LoseDelay());
        }

        private IEnumerator LoseDelay()
        {
            yield return new WaitForSeconds(quitDelay);
            //MenuManager.OpenLoseMenu();
            MenuManager.OpenWinMenu();
        }

        public IEnumerator StartDelay()
        {
            SpawnManager.CanSpawn = false;
            yield return new WaitForSeconds(startDelay);
            SpawnManager.CanSpawn = true;
        }

        private IEnumerator QuitDelay()
        {
            yield return new WaitForSeconds(quitDelay);
            QuitGame();
        }

        public static void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }


        //below section edit by Will
        public void ToMainMenu()
        {
            doneOnce = true;
            if (caravan_HC != null) caravan_HC.SetIsDead(false);
            ObjectPooler.DisableAllActiveObjects();
            SceneManager.LoadScene(0);
            if (FindObjectOfType<CharacterManager>())
            {
                Destroy(FindObjectOfType<CharacterManager>());
            }
        }

        public void ToPlayerSelectionScene()
        {
            doneOnce = true;
            if (caravan_HC != null) caravan_HC.SetIsDead(false);
            ObjectPooler.DisableAllActiveObjects();
            SceneManager.LoadScene("Menu_CharacterSelect");
        }

        public void LoadLevel(string leveltoload)
        {
            Debug.Log("Loading: " + leveltoload);
            SceneManager.LoadScene(leveltoload);
        }

        public void StartSceneButton()
        {
            SceneManager.LoadScene("Menu_CharacterSelect");
            Debug.Log("normal");
            FindObjectOfType<GameDifficultyManager>().dif = DifficultyLevel.Normal;
        }
        public void StartIroncatButton()
        {
            SceneManager.LoadScene("Menu_CharacterSelect");
            Debug.Log("ironcat");
            FindObjectOfType<GameDifficultyManager>().dif = DifficultyLevel.IronCat;
        }
        public void StartCatfightButton()
        {
            SceneManager.LoadScene("Catfight_01");
            Debug.Log("Catfight");
            FindObjectOfType<GameDifficultyManager>().dif = DifficultyLevel.Catfight;

        }
        public void StartCatpocalypseButton()
        {
            SceneManager.LoadScene("Menu_CharacterSelect");
            Debug.Log("catpoc");
            FindObjectOfType<GameDifficultyManager>().dif = DifficultyLevel.Catapocalypse;
        }

        public void CreditsSceneButton()
        {
            LoadLevel("Menu_Credits");
        }

        public void QuitApp()
        {
            Debug.Log("Quitting");
            Application.Quit();
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
    }
}
