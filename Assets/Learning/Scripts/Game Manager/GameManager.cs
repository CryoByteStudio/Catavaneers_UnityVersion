using UnityEngine;
using UnityEngine.SceneManagement;
using Learning.Data;
using Learning.Singleton;
using Learning.MenuSystem;
using Learning.LevelManagement;
using Learning.Utils;
using Learning.Gameplay;
//using UnityStandardAssets.Characters.ThirdPerson;

namespace Learning
{
    public class GameManager : SingletonEntity<GameManager>
    {
        [SerializeField] private int mainMenuSceneIndex = 0;
        [SerializeField] private int firstGameSceneIndex = 0;

        //private ThirdPersonCharacter player;
        private GoalEffect goalEffect;
        private Objective objective;
        //private ThirdPersonUserControl thirdPersonUserControl;
        private Rigidbody rb;

        private bool isGameOver;
        public bool IsGameOver { get { return isGameOver; } }

        public int FirstGameSceneIndex { get { return firstGameSceneIndex; } }

        #region UNITY ENGINE FUNCTIONS

        protected override void Awake()
        {
            base.Awake();

            Reset();
        }

        private void Update()
        {
            if (objective && objective.IsComplete)
            {
                EndLevel();
            }
        }

        #endregion

        #region PRIVATE METHODS

        override protected void SceneLoadedHandler(Scene scene, LoadSceneMode mode)
        {
            base.SceneLoadedHandler(scene, mode);

            if (LevelLoader.IsGameLevel())
            {
                Reset();
            }
        }

        private void Reset()
        {
            LevelLoader.SetMainMenuSceneIndex(mainMenuSceneIndex);
            LevelLoader.SetFirstGameSceneIndex(firstGameSceneIndex);
            goalEffect = FindObjectOfType<GoalEffect>();
            objective = FindObjectOfType<Objective>();
            isGameOver = false;

            //player = FindObjectOfType<ThirdPersonCharacter>();
            //if (player)
            //{
            //    thirdPersonUserControl = player.GetComponent<ThirdPersonUserControl>();
            //    rb = player.GetComponent<Rigidbody>();
            //}
        }

        private void EndLevel()
        {
            //if (!player) return;

            // disable player control
            EnablePlayerController(false);

            if (!goalEffect) return;

            // set game over, play particle FX, load next level
            // unlock next level
            if (!isGameOver)
            {
                isGameOver = true;
                goalEffect.PlayEffect();
                UnlockNextLevel();

                MenuManager.OpenWinMenu();
            }
        }

        private static void UnlockNextLevel()
        {
            if (!DataManager.Instance) return;

            DataManager.Instance.LevelUnlocked++;
            DataManager.Instance.LevelUnlocked = Mathf.Min(DataManager.Instance.LevelUnlocked, MissionList.TotalMission);
            DataManager.Instance.Save();
        }

        /// <summary>
        /// Enable/Disable player control
        /// </summary>
        /// <param name="value"> The value to set. True to Enable and False to Disable. </param>
        public void EnablePlayerController(bool value)
        {
            //if (!player)
            //{
            //    EditorHelper.LogError(this, "ThirdPersonCharacter component is null!");
            //    return;
            //}
            
            //player.SetActive(value);

            //// get reference to ThirdPersonUserControl if not already done
            //if (!thirdPersonUserControl)
            //{
            //    thirdPersonUserControl = player.GetComponent<ThirdPersonUserControl>();
            //}
            
            ////// set ThirdPersonUserControl enabled on/off
            //if (thirdPersonUserControl)
            //{
            //    thirdPersonUserControl.enabled = value;
            //}
        }

        #endregion

        #region PUBLIC STATIC METHODS

        public static void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        
        #endregion
    }
}