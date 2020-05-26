using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;

public class SettingMenu : MonoBehaviour
{
    Resolution[] Resolutions; 

    public AudioMixer VolMixer;

    public Slider VolumeSlider;

    public Dropdown QualityDropDown;

    public Dropdown ResolutionDropDown;

    public Toggle FullScreenToggle;

    public Toggle MuteToggle;

    private bool isMuted;

    private int ScreenInt;

    private int MuteInt;

    private bool isFullScreen;

    const string prefName = "optionvalue";
    const string resName = "resolutionoption";

    private void Awake()
    {
        ScreenInt = PlayerPrefs.GetInt("Screentogglestate");
        MuteInt = PlayerPrefs.GetInt("Mutetogglestate");

        if(ScreenInt == 1)
        {
            isFullScreen = true;
            FullScreenToggle.isOn = true;
        }
        else
        {
            FullScreenToggle.isOn = false;
        }

        if(MuteInt == 1)
        {
            isMuted = true;
            MuteToggle.isOn = true;
        }
        else
        {
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
        VolumeSlider.value = PlayerPrefs.GetFloat("MVolume", 1f);
        VolMixer.SetFloat("Volume", PlayerPrefs.GetFloat("MVolume"));

        QualityDropDown.value = PlayerPrefs.GetInt(prefName, 3);

        Resolutions = Screen.resolutions;

        ResolutionDropDown.ClearOptions();

        List<string> Options = new List<string>();

        int CurrentResolutionIndex = 0;

        for(int i = 0; i < Resolutions.Length; i++)
        {
            string option = Resolutions[i].width + " x " + Resolutions[i].height;
            Options.Add(option);

            if (Resolutions[i].width == Screen.currentResolution.width && Resolutions[i].height == Screen.currentResolution.height)
            {
                CurrentResolutionIndex = i;
            }
        }

        ResolutionDropDown.AddOptions(Options);
        ResolutionDropDown.value = PlayerPrefs.GetInt(resName,CurrentResolutionIndex);
        ResolutionDropDown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("MVolume", volume);
        VolMixer.SetFloat("Volume", PlayerPrefs.GetFloat("MVolume"));
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution ChangeResolution = Resolutions[resolutionIndex];
        Screen.SetResolution(ChangeResolution.width, ChangeResolution.height, Screen.fullScreen);
    }


    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex,true);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        if(isFullScreen == false)
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
        AudioListener.pause = isMute;

        Debug.Log(AudioListener.pause);

        if(isMute == false)
        {
            PlayerPrefs.SetInt("Mutetogglestate", 0);
        }
        else
        {
            isMute = true;
            PlayerPrefs.SetInt("Mutetogglestate", 1);
        }
    }
}
