﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Catavaneer.MenuSystem
{
    public class MainMenu : Menu<MainMenu>
    {
        [SerializeField] private GameObject firstSelected;

        #region UNITY ENGINE FUNCTIONS
        private void OnEnable()
        {
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
        }

        public void OnQuitPressed()
        {
            MenuManager.QuitGame();
        }

        public void OnNormalPressed()
        {
            FindObjectOfType<BaseInputModule>().DeactivateModule();
            SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
            GameManager.SetDifficultyLevel(DifficultyLevel.Normal);
            MenuManager.LoadCharacterSelectScene();
        }

        public void OnIronCatPressed()
        {
            FindObjectOfType<BaseInputModule>().DeactivateModule();
            SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
            GameManager.SetDifficultyLevel(DifficultyLevel.IronCat);
            MenuManager.LoadCharacterSelectScene();
        }

        public void OnCatpocalypsePressed()
        {
            FindObjectOfType<BaseInputModule>().DeactivateModule();
            SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
            GameManager.SetDifficultyLevel(DifficultyLevel.Catapocalypse);
            MenuManager.LoadCharacterSelectScene();
        }

        public void OnCatFightPressed()
        {
            FindObjectOfType<BaseInputModule>().DeactivateModule();
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