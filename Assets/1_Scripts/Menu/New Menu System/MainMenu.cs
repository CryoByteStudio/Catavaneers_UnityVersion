using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Catavaneer.MenuSystem
{
    public class MainMenu : Menu<MainMenu>
    {
        #region UNITY ENGINE FUNCTIONS
        private void OnEnable()
        {
            if (selectedGameObject)
            {
                EventSystem.current.SetSelectedGameObject(selectedGameObject);
                selectedGameObject.GetComponent<Selectable>().OnSelect(new BaseEventData(EventSystem.current));
            }
        }
        #endregion

        #region PUBLIC METHODS
        public void OnCreditsPressed()
        {
            SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
            CreditsMenu.Open();
        }

        public void OnQuitPressed()
        {
            MenuManager.QuitGame();
        }

        public void OnNormalPressed()
        {
            GameManager.SetDifficultyLevel(DifficultyLevel.Normal);
            MenuManager.LoadCharacterSelectScene();
        }

        public void OnIronCatPressed()
        {
            GameManager.SetDifficultyLevel(DifficultyLevel.IronCat);
            MenuManager.LoadCharacterSelectScene();
        }

        public void OnCatpocalypsePressed()
        {
            GameManager.SetDifficultyLevel(DifficultyLevel.Catapocalypse);
            MenuManager.LoadCharacterSelectScene();
        }

        public void OnCatFightPressed()
        {
            GameManager.SetDifficultyLevel(DifficultyLevel.Catfight);
        }

        public void OnApplyPressed()
        {
            // TODO save settings
        }
        #endregion
    }
}