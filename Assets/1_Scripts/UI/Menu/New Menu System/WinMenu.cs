using UnityEngine;
using UnityEngine.EventSystems;

namespace Catavaneer.MenuSystem
{
    public class WinMenu : Menu<WinMenu>
    {
        [SerializeField] private GameObject firstSelected;

        #region UNITY ENGINE FUNCTIONS
        protected override void Awake()
        {
            base.Awake();
            SetSelectedGameObject(firstSelected);
        }
        #endregion

        #region PUBLIC METHODS
        public void OnCampaignPressed()
        {
            FindObjectOfType<BaseInputModule>().DeactivateModule();
            MenuManager.LoadCampaignLevel();
        }

        public void OnRestartPressed()
        {
            FindObjectOfType<BaseInputModule>().DeactivateModule();
            MenuManager.RestartLevel();
        }

        public void OnMainMenuPressed()
        {
            FindObjectOfType<BaseInputModule>().DeactivateModule();
            MenuManager.LoadMainMenuLevel();
        }
        #endregion
    }
}