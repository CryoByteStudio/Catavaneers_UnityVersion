using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{

    public Dropdown ResolutionDropDown;

    Resolution[] Resolutions; 

    public AudioMixer AudioMixer;

    private bool IsMuted;

    void Start()
    {

        IsMuted = false;

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
        ResolutionDropDown.value = CurrentResolutionIndex;
        ResolutionDropDown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        AudioMixer.SetFloat("Volume", volume);
    }

    public void SetResolution(int resolutionIndex)
    {
        
        Resolution ChangeResolution = Resolutions[resolutionIndex];
        Screen.SetResolution(ChangeResolution.width, ChangeResolution.height, Screen.fullScreen);
        Debug.Log(Resolutions[resolutionIndex]);
        Debug.Log(Screen.currentResolution);
    }


    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex,true);
        Debug.Log(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void MutePressed()
    {
        IsMuted = !IsMuted;
        AudioListener.pause = IsMuted;
    }
}
