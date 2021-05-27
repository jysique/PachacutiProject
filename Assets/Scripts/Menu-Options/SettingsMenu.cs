using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    public AudioMixerGroup master;
    public Dropdown resolutionDropdown;
    [SerializeField] public Button btn1;
    [SerializeField] public Button btn2;

    Resolution[] resolutions;
    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetVolume(float vol)
    {
        //    master.audioMixer.SetFloat("volume", vol);
        master.audioMixer.SetFloat("volume", Mathf.Log10(vol)*20);
    }
    public void SetVolumeMusic(float vol)
    {
        //master.audioMixer.SetFloat("volMusic", vol);
        master.audioMixer.SetFloat("volMusic", Mathf.Log10(vol) * 20);
    }
    public void SetVolumeSFX(float vol)
    {
        //  master.audioMixer.SetFloat("volSFX", vol);
        master.audioMixer.SetFloat("volSFX", Mathf.Log10(vol) * 20);
        //  AudioManager.instance.ReadAndPlaySFX("ButtonPush");
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution reso = resolutions[resolutionIndex];
        Screen.SetResolution(reso.width,reso.height, Screen.fullScreen);
    }
}
