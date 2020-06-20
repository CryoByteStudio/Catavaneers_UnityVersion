using UnityEngine;

namespace Catavaneer.MenuSystem
{
    public class LoseMenu : Menu<LoseMenu>
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