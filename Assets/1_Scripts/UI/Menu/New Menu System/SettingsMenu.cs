using UnityEngine;
using UnityEngine.UI;

namespace Catavaneer.MenuSystem
{
    public class SettingsMenu : Menu<SettingsMenu>
    {
        [SerializeField] private GameObject firstSelected;
        [SerializeField] private Slider VolumeSlider;
        [SerializeField] private Slider SFXVolumeSlider;
        [SerializeField] private Toggle MuteToggle;
        private bool isMuted;
        private int muteInt;

        #region UNITY ENGINE FUNCTIONS
        protected override void Awake()
        {
            base.Awake();
            SetSelectedGameObject(firstSelected);
        }

        private void OnEnable()
        {
            LoadSetting();
        }
        #endregion
        
        #region PUBLIC METHODS
        public void LoadSetting()
        {
            VolumeSlider.value = PlayerPrefs.GetFloat("MVolume", 1f);
            SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);
            muteInt = PlayerPrefs.GetInt("Mutetogglestate");
        }

        public void OnSetVolume(float volume)
        {
            PlayerPrefs.SetFloat("MVolume", volume);
            MusicManager.Instance.menuState.setVolume(PlayerPrefs.GetFloat("MVolume"));
            MusicManager.Instance.caravanState.setVolume(PlayerPrefs.GetFloat("MVolume"));
        }

        public void OnSetSFXVolume(float volume)
        {
            if (!isMuted)
                MusicManager.Instance.sfxVolume = volume;

            PlayerPrefs.SetFloat("SFXVolume", volume);
        }

        public void OnMutePressed(bool value)
        {
            isMuted = value;
            MusicManager.Instance.menuState.setPaused(value);
            MusicManager.Instance.caravanState.setPaused(value);
            
            Debug.Log(AudioListener.pause);

            if (!value)
            {
                MusicManager.Instance.sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
                PlayerPrefs.SetInt("Mutetogglestate", 0);
            }
            else
            {
                MusicManager.Instance.sfxVolume = 0;
                PlayerPrefs.SetInt("Mutetogglestate", 1);
            }
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }
        #endregion
    }
}