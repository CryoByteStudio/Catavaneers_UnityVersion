using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Catavaneer.MenuSystem
{
    public class MainMenu : Menu<MainMenu>
    {
        [SerializeField] private GameObject firstSelected;

        private int buttonPressCount = 0;

        #region UNITY ENGINE FUNCTIONS
        private void OnEnable()
        {
            buttonPressCount = 0;

            if (selectedGameObject && !GameManager.Instance.HasFinishedAllLevel)
            {
                ForceHighlightFirstSelected();
            }
            else
            {
                SetSelectedGameObject(firstSelected);
                ForceHighlightFirstSelected();
            }
        }
        #endregion

        #region PRIVATE METHODS
        private void ForceHighlightFirstSelected()
        {
            EventSystem.current.SetSelectedGameObject(selectedGameObject);
            selectedGameObject.GetComponent<Selectable>().OnSelect(new BaseEventData(EventSystem.current));
            SetSelectedGameObject(null);
        }
        #endregion

        #region PUBLIC METHODS
        public void OnCreditsPressed()
        {
            SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
            CreditsMenu.Open();
            CreditsMenu.Instance.Play();
        }

        public void OnQuitPressed()
        {
            MenuManager.QuitGame();
        }

        public void OnNormalPressed()
        {
            if (!ButtonSmashPreventor.ShouldProceed(ref buttonPressCount)) return;
            //FindObjectOfType<BaseInputModule>().DeactivateModule();
            SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
            GameManager.SetDifficultyLevel(DifficultyLevel.Normal);
            MenuManager.LoadCharacterSelectScene();
        }

        public void OnIronCatPressed()
        {
            if (!ButtonSmashPreventor.ShouldProceed(ref buttonPressCount)) return;
            //FindObjectOfType<BaseInputModule>().DeactivateModule();
            SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
            GameManager.SetDifficultyLevel(DifficultyLevel.IronCat);
            MenuManager.LoadCharacterSelectScene();
        }

        public void OnCatpocalypsePressed()
        {
            if (!ButtonSmashPreventor.ShouldProceed(ref buttonPressCount)) return;
            //FindObjectOfType<BaseInputModule>().DeactivateModule();
            SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
            GameManager.SetDifficultyLevel(DifficultyLevel.Catapocalypse);
            MenuManager.LoadCharacterSelectScene();
        }

        public void OnCatFightPressed()
        {
            if (!ButtonSmashPreventor.ShouldProceed(ref buttonPressCount)) return;
            //FindObjectOfType<BaseInputModule>().DeactivateModule();
            SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
            GameManager.SetDifficultyLevel(DifficultyLevel.Catfight);
        }

        public void OnApplyPressed()
        {
            SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
            // TODO save settings
        }
        #endregion
    }
}