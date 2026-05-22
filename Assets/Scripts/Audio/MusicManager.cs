using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    
    public AudioSource calmTrack;
    public AudioSource combatTrack;
    public bool isPlaying = false;
    
    private float targetCalmVolume = 1f;
    private float targetCombatVolume = 0f;
    private float userVolume = 0.5f;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartMusic()
    {
        isPlaying = true;
        userVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        calmTrack.volume = targetCalmVolume * userVolume;
        combatTrack.volume = targetCombatVolume * userVolume;

        calmTrack.Play();
        combatTrack.Play();
    }

    void Update()
    {
        if (isPlaying == true)
        {
            calmTrack.volume = Mathf.Lerp(calmTrack.volume, targetCalmVolume * userVolume, Time.deltaTime * 1.5f);
            combatTrack.volume = Mathf.Lerp(combatTrack.volume, targetCombatVolume * userVolume, Time.deltaTime * 1.5f);
        }
    }

    public void EnterCombat()
    {
        targetCalmVolume = 0f;
        targetCombatVolume = 1f;
    }

    public void ExitCombat()
    {
        targetCalmVolume = 1f;
        targetCombatVolume = 0f;
    }

    public void ResetMusic()
    {
        targetCalmVolume = 1f;
        targetCombatVolume = 0f;

        calmTrack.Stop();
        combatTrack.Stop();

        calmTrack.time = 0f;
        combatTrack.time = 0f;

        calmTrack.Play();
        combatTrack.Play();
        
        calmTrack.volume = targetCalmVolume;
        combatTrack.volume = targetCombatVolume;
    }

    public void TurnOffMusic()
    {   
        calmTrack.Stop();
        combatTrack.Stop();

        calmTrack.time = 0f;
        combatTrack.time = 0f;

        ExitCombat();
        isPlaying = false;
    }

    public void SetMusicVolume(float volume)
    {
        userVolume = volume;
        ApplyVolume();
    }

    public void ApplyVolume()
    {
        calmTrack.volume = targetCalmVolume * userVolume;
        combatTrack.volume = targetCombatVolume * userVolume;
    } 

    public void PauseMusic()
    {
        calmTrack.Pause();
        combatTrack.Pause();
    }

    public void ResumeMusic()
    {
        calmTrack.UnPause();
        combatTrack.UnPause();
    }
}