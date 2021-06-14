using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;
public class SettingsMenu : MonoBehaviour
{
    public AudioMixerGroup master;    
    [SerializeField] Slider volGeneral;
    [SerializeField] Slider volMusic;
    [SerializeField] Slider volSFX;
    [SerializeField] Dropdown resolutionDropdown;
    [SerializeField] Toggle fullScreenToggle;

    [Header("Buttons")]
    [SerializeField] Button newGameButton;
    [SerializeField] Button closeButton;

    private int screenInt;
    Resolution[] resolutions;
    const string resName = "resolutionoption";

    private void Awake()
    {
        screenInt = PlayerPrefs.GetInt("toggleState");
        if (screenInt == 1)
        {
            fullScreenToggle.isOn = true;
        }
        else
        {
            fullScreenToggle.isOn = false;
        }
        resolutionDropdown.onValueChanged.AddListener(new UnityAction<int>(index =>
        {
           PlayerPrefs.SetInt(resName, resolutionDropdown.value);
           PlayerPrefs.Save();
        }));

    }
    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        InitializedValues();
        InitializedValuesResolution();
        InitializeButtons();
    }
    private void InitializeButtons()
    {
        newGameButton.onClick.AddListener(()=> GlobalVariables.instance.GoToMenuMessage());
        closeButton.onClick.AddListener(() => GlobalVariables.instance.ClosingApp());
    }
    private void InitializedValues()
    {
        volGeneral.value = PlayerPrefs.GetFloat("volG", 1f);
        volMusic.value = PlayerPrefs.GetFloat("volM", 1f);
        volSFX.value = PlayerPrefs.GetFloat("volS", 1f);
        master.audioMixer.SetFloat("volume", Desibel(PlayerPrefs.GetFloat("volG")));
        master.audioMixer.SetFloat("volMusic", Desibel(PlayerPrefs.GetFloat("volM")));
        master.audioMixer.SetFloat("volSFX", Desibel(PlayerPrefs.GetFloat("volS")));
    }
    private void InitializedValuesResolution()
    {
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if ((resolutions[i].width % 16.0f) == 0 && (resolutions[i].height % 9.0f) == 0)
            {
                options.Add(option);
            }
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt(resName, currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution reso = resolutions[resolutionIndex];
        Screen.SetResolution(reso.width, reso.height, Screen.fullScreen);
    }
    public void SetVolume(float vol)
    {
        PlayerPrefs.SetFloat("volG", vol);
        master.audioMixer.SetFloat("volume", Desibel(PlayerPrefs.GetFloat("volG")));
    }
    public void SetVolumeMusic(float vol)
    {
        PlayerPrefs.SetFloat("volM", vol);
        // master.audioMixer.SetFloat("volMusic", Mathf.Log10(vol) * 20);
        master.audioMixer.SetFloat("volMusic", Desibel(PlayerPrefs.GetFloat("volM")));
    }
    public void SetVolumeSFX(float vol)
    {
        PlayerPrefs.SetFloat("volS", vol);
        //master.audioMixer.SetFloat("volSFX", Mathf.Log10(vol) * 20);
        master.audioMixer.SetFloat("volSFX", Desibel(PlayerPrefs.GetFloat("volS")));
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        if (isFullScreen == false)
        {
            PlayerPrefs.SetInt("toggleState", 0);
        }
        else
        {
            PlayerPrefs.SetInt("toggleState", 1);
        }
    }
    private float Desibel(float a)
    {
        return Mathf.Log10(a) * 20;
    }
}
