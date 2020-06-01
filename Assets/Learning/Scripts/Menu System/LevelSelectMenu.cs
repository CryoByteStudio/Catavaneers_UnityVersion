using CustomMathLibrary;
using Learning.Data;
using Learning.LevelManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Learning.MenuSystem
{
    public class LevelSelectMenu : Menu<LevelSelectMenu>
    {
        [SerializeField] private MissionList missionList;
        [SerializeField] private TMP_Text levelName;
        [SerializeField] private TMP_Text levelDescription;
        [SerializeField] private Image displayScreen;

        private MissionSpecs currentMissionSpecs;
        private int currentMissionSpecsIndex = 0;
        private int levelUnlocked;

        #region UNITY FUNCTIONS

        override protected void Awake()
        {
            base.Awake();

            if (!missionList)
                LoadFromResources(out missionList, "Missions/MissionList_SO");

            UpdateLevelInfo();
        }

        private void Start()
        {
            LoadData();
        }

        private void OnEnable()
        {
            levelUnlocked = DataManager.Instance.LevelUnlocked;
            Reset();
            UpdateLevelInfo();
        }

        private void Reset()
        {
            currentMissionSpecsIndex = 0;
        }

        #endregion

        #region PRIVATE METHODS

        private void LoadFromResources<T>(out T resourceToLoad, string path) where T : Object
        {
            resourceToLoad = Resources.Load<T>(path);
        }

        private void LoadData()
        {
            if (DataManager.Instance)
                DataManager.Instance.Load();

            levelUnlocked = DataManager.Instance.LevelUnlocked;
        }

        private void UpdateLevelInfo()
        {
            currentMissionSpecs = missionList?.GetMission(currentMissionSpecsIndex);
            levelName.text = currentMissionSpecs?.Name;
            levelDescription.text = currentMissionSpecs?.Description;
            displayScreen.sprite = currentMissionSpecs?.Image;
        }

        private void ClampIndexToLoop()
        {
            if (DataManager.Instance)
                currentMissionSpecsIndex = CustomMathf.GetLoopIndex(currentMissionSpecsIndex, levelUnlocked);
        }

        #endregion

        #region PUBLIC METHODS

        public void OnPlayPressed()
        {
            MenuManager.LoadLevel(currentMissionSpecs.SceneName);
        }

        public void OnLeftPressed()
        {
            currentMissionSpecsIndex--;
            ClampIndexToLoop();
            UpdateLevelInfo();
        }

        public void OnRightPressed()
        {
            currentMissionSpecsIndex++;
            ClampIndexToLoop();
            UpdateLevelInfo();
        }

        #endregion
    }
}