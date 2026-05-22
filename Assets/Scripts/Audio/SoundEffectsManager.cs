using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager Instance;
    public AudioSource sfxSource;

    public AudioClip roomEnterSound;
    public float roomEnterVolume = 0.5f;

    public AudioClip roomClearSound;
    public float roomClearVolume = 0.5f;

    public AudioClip hurtSound;
    public float hurtVolume = 0.5f;

    public AudioClip coinSound;
    public float coinVolume = 0.5f;

    public AudioClip itemPurchaseSound;
    public float itemPurchaseVolume = 0.5f;
    
    public AudioClip buttonClick;
    public float buttonClickVolume = 0.5f;

    public AudioClip weaponPickupSound;
    public float weaponPickupVolume = 0.5f;

    public AudioClip teleportSound;
    public float teleportVolume = 0.5f;

    private float userVolume = 0.7f;
    private AudioClip lastPlayedClip;

    private float lastPlayedTime;
    
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
    public void PlayRoomEnter()
    {
        PlaySound(roomEnterSound, roomEnterVolume);
    }
    
    public void PlayRoomClear()
    {
        PlaySound(roomClearSound, roomClearVolume);
    }
    
    public void PlayHurt()
    {
        PlaySound(hurtSound, hurtVolume);
    }
    
    public void PlayCoin()
    {
        PlaySound(coinSound, coinVolume);
    }
    
    public void PlayItemPurchase()
    {
        PlaySound(itemPurchaseSound, itemPurchaseVolume);
    }
    
    public void PlayButtonClick()
    {
        PlaySound(buttonClick, buttonClickVolume);
    }
    
    public void PlayWeaponPickup()
    {
        PlaySound(weaponPickupSound, weaponPickupVolume);
    }
    
    public void PlayTeleport()
    {
        PlaySound(teleportSound, teleportVolume);
    }

    void Start()
    {
        userVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
    
    public void PlaySound(AudioClip clip, float volume, float minInterval = 0.2f)
    {
        if (clip == null || sfxSource == null) return;
        
        // Не проигрывать тот же звук чаще чем раз в minInterval секунд
        if (clip == lastPlayedClip && Time.time - lastPlayedTime < minInterval) return;
        
        lastPlayedClip = clip;
        lastPlayedTime = Time.time;
        sfxSource.PlayOneShot(clip, volume * userVolume);
    }
    
    public void SetVolume(float volume)
    {
        userVolume = volume;
    }
}
