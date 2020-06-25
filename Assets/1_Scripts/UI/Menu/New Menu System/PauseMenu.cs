using UnityEngine;
using UnityEngine.EventSystems;

namespace Catavaneer.MenuSystem
{
    public class PauseMenu : Menu<PauseMenu>
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
        public void OnResumePressed()
        {
            MenuManager.ResumeGame();
        }

        public void OnRestartPressed()
        {
            FindObjectOfType<BaseInputModule>().DeactivateModule();
            MenuManager.RestartLevel();
        }

        public void OnSettingsPressed()
        {
            SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
            MenuManager.OpenSettingsMenu();
        }

        public void OnMainMenuPressed()
        {
            FindObjectOfType<BaseInputModule>().DeactivateModule();
            MenuManager.LoadMainMenuLevel(false);
        }
        #endregion
    }
}