using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SettingsPanel : MonoBehaviour
{
    [Header("Вкладки")]
    public GameObject tabAudio;
    public GameObject tabGraphics;
    public Button buttonAudio;
    public Button buttonGraphics;
    
    [Header("Звук")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    
    [Header("Графика")]
    public Toggle fullscreenToggle;
    public TMP_Dropdown  resolutionDropdown;
    public TMP_Dropdown  qualityDropdown;
    
    private Resolution[] resolutions;
    
    void Start()
    {
        // ========== ЗВУК ==========
        float master = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        
        masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;
        
        AudioListener.volume = master;
        MusicManager.Instance?.SetMusicVolume(music);
        SoundEffectsManager.Instance?.SetSFXVolume(sfx);
        
        masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        
        // ========== ГРАФИКА ==========
        // Полный экран
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        
        // Качество графики
        qualityDropdown.ClearOptions();
        List<string> qualityOptions = new List<string>(QualitySettings.names);
        qualityDropdown.AddOptions(qualityOptions);
        int currentQuality = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
        qualityDropdown.value = currentQuality;
        QualitySettings.SetQualityLevel(currentQuality);
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
        
        // Разрешение экрана
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;
        
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);
            
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        ApplyResolution(resolutionDropdown.value);
        
        // Показываем первую вкладку
        ShowTabGraphics();
        
        buttonAudio.onClick.AddListener(ShowTabAudio);
        buttonGraphics.onClick.AddListener(ShowTabGraphics);
    }
    
    void ShowTabAudio()
    {
        tabAudio.SetActive(true);
        tabGraphics.SetActive(false);
    }
    
    void ShowTabGraphics()
    {
        tabAudio.SetActive(false);
        tabGraphics.SetActive(true);
    }
    
    // ========== ЗВУК ==========
    void OnMasterVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }
    
    void OnMusicVolumeChanged(float value)
    {
        MusicManager.Instance?.SetMusicVolume(value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
    
    void OnSFXVolumeChanged(float value)
    {
        SoundEffectsManager.Instance?.SetSFXVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
    
    // ========== ГРАФИКА ==========
    void OnFullscreenChanged(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }
    
    void OnQualityChanged(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("QualityLevel", index);
    }
    
    void OnResolutionChanged(int index)
    {
        ApplyResolution(index);
        PlayerPrefs.SetInt("ResolutionIndex", index);
    }
    
    void ApplyResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}