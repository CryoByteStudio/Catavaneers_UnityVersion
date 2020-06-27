using UnityEngine;
using UnityEngine.EventSystems;

namespace Catavaneer.MenuSystem
{
    public class PauseMenu : Menu<PauseMenu>
    {
        [SerializeField] private GameObject firstSelected;

        private int buttonPressCount;

        #region UNITY ENGINE FUNCTIONS
        protected override void Awake()
        {
            base.Awake();
            SetSelectedGameObject(firstSelected);
        }

        private void OnEnable()
        {
            buttonPressCount = 0;
        }
        #endregion

        #region PUBLIC METHODS
        public void OnResumePressed()
        {
            MenuManager.ResumeGame();
        }

        public void OnRestartPressed()
        {
            if (!ButtonSmashPreventor.ShouldProceed(ref buttonPressCount)) return;
            MenuManager.RestartLevel();
        }

        public void OnSettingsPressed()
        {
            SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
            MenuManager.OpenSettingsMenu();
        }

        public void OnMainMenuPressed()
        {
            if (!ButtonSmashPreventor.ShouldProceed(ref buttonPressCount)) return;

            if (!GameManager.Instance.IsGameOver)
                GameManager.ResetCampaignParams();

            MenuManager.LoadMainMenuLevel(false);
        }
        #endregion
    }
}