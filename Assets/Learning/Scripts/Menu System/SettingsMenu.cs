using UnityEngine;
using UnityEngine.UI;
using Learning.Data;
using Learning.Utils;

namespace Learning.MenuSystem
{
    public class SettingsMenu : Menu<SettingsMenu>
    {
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;

        #region UNITY FUNCTIONS

        private void Start()
        {
            LoadData();
        }

        private void OnEnable()
        {
            LoadData();
        }

        #endregion

        #region PRIVATE METHODS

        private void LoadData()
        {
            if (EditorHelper.IsValueNull(DataManager.Instance, "DataManager.Instance")) return;
            if (EditorHelper.IsValueNull(masterVolumeSlider, "masterVolumeSlider")) return;
            if (EditorHelper.IsValueNull(sfxVolumeSlider, "sfxVolumeSlider")) return;
            if (EditorHelper.IsValueNull(musicVolumeSlider, "musicVolumeSlider")) return;

            DataManager.Instance.Load();
            masterVolumeSlider.value = DataManager.Instance.MasterVolume;
            sfxVolumeSlider.value = DataManager.Instance.SFXVolume;
            musicVolumeSlider.value = DataManager.Instance.MusicVolume;
        }

        #endregion

        #region PUBLIC METHODS

        public void OnMasterVolumeChanged(float volume)
        {
            // Change master volume
            if (EditorHelper.IsValueNull(DataManager.Instance, "DataManager.Instance")) return;

            DataManager.Instance.MasterVolume = volume;
        }

        public void OnSFXVolumeChanged(float volume)
        {
            // Change SFX volume
            if (EditorHelper.IsValueNull(DataManager.Instance, "DataManager.Instance")) return;

            DataManager.Instance.SFXVolume = volume;
        }

        public void OnMusicVolumeChanged(float volume)
        {
            // Change music volume
            if (EditorHelper.IsValueNull(DataManager.Instance, "DataManager.Instance")) return;

            DataManager.Instance.MusicVolume = volume;
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();

            if (EditorHelper.IsValueNull(DataManager.Instance, "DataManager.Instance")) return;

            DataManager.Instance.Save();
        }

        #endregion
    }
}