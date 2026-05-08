using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public Slider musicSlider;
    
    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicSlider.value = savedVolume;
        MusicManager.Instance?.SetMusicVolume(savedVolume);

        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }
    
    void OnMusicVolumeChanged(float value)
    {
        MusicManager.Instance?.SetMusicVolume(value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
}