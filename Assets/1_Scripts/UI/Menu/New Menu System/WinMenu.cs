using UnityEngine;
using UnityEngine.EventSystems;

namespace Catavaneer.MenuSystem
{
    public class WinMenu : Menu<WinMenu>
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
        public void OnCampaignPressed()
        {
            if (!ButtonSmashPreventor.ShouldProceed(ref buttonPressCount)) return;
            FindObjectOfType<BaseInputModule>().DeactivateModule();
            MenuManager.LoadCampaignLevel();
        }

        public void OnRestartPressed()
        {
            if (!ButtonSmashPreventor.ShouldProceed(ref buttonPressCount)) return;
            FindObjectOfType<BaseInputModule>().DeactivateModule();
            MenuManager.RestartLevel();
        }

        public void OnMainMenuPressed()
        {
            if (!ButtonSmashPreventor.ShouldProceed(ref buttonPressCount)) return;
            FindObjectOfType<BaseInputModule>().DeactivateModule();
            MenuManager.LoadMainMenuLevel();
        }
        #endregion
    }
}