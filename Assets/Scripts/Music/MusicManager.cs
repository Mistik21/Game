using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    
    public AudioSource calmTrack;
    public AudioSource combatTrack;
    
    private float targetCalmVolume = 0.5f;
    private float targetCombatVolume = 0f;
    
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
        calmTrack.volume = targetCalmVolume;
        combatTrack.volume = targetCombatVolume;
    }
    
    void Update()
    {
        calmTrack.volume = Mathf.Lerp(calmTrack.volume, targetCalmVolume, Time.deltaTime * 1.5f);
        combatTrack.volume = Mathf.Lerp(combatTrack.volume, targetCombatVolume, Time.deltaTime * 1.5f);
    }
    
    public void EnterCombat()
    {
        targetCalmVolume = 0f;
        targetCombatVolume = 0.5f;
    }
    
    public void ExitCombat()
    {
        targetCalmVolume = 0.5f;
        targetCombatVolume = 0f;
    }

    public void ResetMusic()
    {
        targetCalmVolume = 0.5f;
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
}