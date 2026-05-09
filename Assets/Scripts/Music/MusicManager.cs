using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    
    public AudioSource calmTrack;
    public AudioSource combatTrack;
    
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
    
    void Start()
    {
        calmTrack.Play();
        combatTrack.Play();

        userVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        calmTrack.volume = targetCalmVolume * userVolume;
        combatTrack.volume = targetCombatVolume * userVolume;
    }
    
    void Update()
    {
    calmTrack.volume = Mathf.Lerp(calmTrack.volume, targetCalmVolume * userVolume, Time.deltaTime * 1.5f);
    combatTrack.volume = Mathf.Lerp(combatTrack.volume, targetCombatVolume * userVolume, Time.deltaTime * 1.5f);
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

        calmTrack.volume = targetCalmVolume;
        combatTrack.volume = targetCombatVolume;
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
}