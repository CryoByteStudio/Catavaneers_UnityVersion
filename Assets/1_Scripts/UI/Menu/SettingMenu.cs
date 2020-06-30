using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using FMODUnity;


public class SettingMenu : MonoBehaviour
{
    Resolution[] Resolutions;

    public AudioMixer VolMixer;

    public Slider VolumeSlider;

    public Slider SFXVolumeSlider;

    //public Dropdown QualityDropDown;
    public TMP_Dropdown QualityDropDown;

    //public Dropdown ResolutionDropDown;
    public TMP_Dropdown ResolutionDropDown;

    public Toggle FullScreenToggle;

    public Toggle MuteToggle;

    private bool isMuted;

    private int ScreenInt;

    private int MuteInt;

    private bool isFullScreen;

    const string prefName = "optionvalue";
    const string resName = "resolutionoption";

    private MusicManager MM;

    private void Awake()
    {
        LoadSetting();
    }

    public void LoadSetting()
    {

        MM = FindObjectOfType<MusicManager>();

        ScreenInt = PlayerPrefs.GetInt("Screentogglestate");
        MuteInt = PlayerPrefs.GetInt("Mutetogglestate");

        if (ScreenInt == 1)
        {
            isFullScreen = true;
            FullScreenToggle.isOn = true;
        }
        else
        {
            FullScreenToggle.isOn = false;
        }

        if (MuteInt == 1)
        {

            isMuted = true;
            MM.menuState.setPaused(true);
            MM.caravanState.setPaused(true);
            MuteToggle.isOn = true;
        }
        else
        {
            isMuted = false;
            MM.menuState.setPaused(false);
            MM.caravanState.setPaused(false);
            MuteToggle.isOn = false;
        }

        ResolutionDropDown.onValueChanged.AddListener(new UnityAction<int>(index =>
        {
            PlayerPrefs.SetInt(resName, ResolutionDropDown.value);
            PlayerPrefs.Save();
        }));

        QualityDropDown.onValueChanged.AddListener(new UnityAction<int>(index =>
        {
            PlayerPrefs.SetInt(prefName, QualityDropDown.value);
            PlayerPrefs.Save();
        }));

    }

    void Start()
    {
        if(PlayerPrefs.HasKey("MVolume"))
        {
            MM.menuState.setVolume(PlayerPrefs.GetFloat("MVolume", 1f));
            MM.caravanState.setVolume(PlayerPrefs.GetFloat("MVolume",1f));
            VolumeSlider.value = PlayerPrefs.GetFloat("MVolume", 1f);
        }
        else
        {
            PlayerPrefs.SetFloat("MVolume", 1f);
            MM.menuState.setVolume(1f);
            MM.caravanState.setVolume(1f);
            VolumeSlider.value = 1f;
        }

        //VolMixer.SetFloat("Volume", PlayerPrefs.GetFloat("MVolume"));

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);
        }
        else
        {
            PlayerPrefs.SetFloat("SFXVolume", 1f);
            SFXVolumeSlider.value = 1f;
        }


        QualityDropDown.value = PlayerPrefs.GetInt(prefName, 3);

        Resolutions = Screen.resolutions;

        ResolutionDropDown.ClearOptions();

        List<string> Options = new List<string>();

        int CurrentResolutionIndex = 0;

        for (int i = 0; i < Resolutions.Length; i++)
        {
            string option = Resolutions[i].width + " x " + Resolutions[i].height;
            Options.Add(option);

            if (Resolutions[i].width == Screen.currentResolution.width && Resolutions[i].height == Screen.currentResolution.height)
            {
                CurrentResolutionIndex = i;
            }
        }

        ResolutionDropDown.AddOptions(Options);
        ResolutionDropDown.value = PlayerPrefs.GetInt(resName, CurrentResolutionIndex);
        ResolutionDropDown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("MVolume", volume);
        //VolMixer.SetFloat("Volume", PlayerPrefs.GetFloat("MVolume"));
        MM.menuState.setVolume(PlayerPrefs.GetFloat("MVolume"));
        MM.caravanState.setVolume(PlayerPrefs.GetFloat("MVolume"));
    }

    public void SetSFXVolume(float volume)
    {
        if (isMuted == false)
            MM.sfxVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        //VolMixer.SetFloat("Volume", PlayerPrefs.GetFloat("MVolume"));
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution ChangeResolution = Resolutions[resolutionIndex];
        Screen.SetResolution(ChangeResolution.width, ChangeResolution.height, Screen.fullScreen);
    }


    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex, true);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        if (isFullScreen == false)
        {
            PlayerPrefs.SetInt("Screentogglestate", 0);
        }
        else
        {
            isFullScreen = true;
            PlayerPrefs.SetInt("Screentogglestate", 1);
        }
    }

    public void MutePressed(bool isMute)
    {
        MM.menuState.setPaused(isMute);
        MM.caravanState.setPaused(isMute);


        Debug.Log(AudioListener.pause);

        if (isMute == false)
        {
            MM.sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
            PlayerPrefs.SetInt("Mutetogglestate", 0);
        }
        else
        {
            isMute = true;
            MM.sfxVolume = 0;
            PlayerPrefs.SetInt("Mutetogglestate", 1);
        }
    }

    //public void SetVoluem(float volume)
    //{
    //    menuState.setVolume(volume)
    //}
}
