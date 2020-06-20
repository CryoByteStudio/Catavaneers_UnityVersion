using UnityEngine;

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
            MenuManager.LoadCampaignLevel();
        }

        public void OnRestartPressed()
        {
            MenuManager.RestartLevel();
        }

        public void OnMainMenuPressed()
        {
            MenuManager.LoadMainMenuLevel();
        }
        #endregion
    }
}