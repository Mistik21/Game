using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager Instance;
    public AudioSource sfxSource;
    //пока (или не пока) громкость меняется только через инспектор в менеджере звуков
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

    public AudioClip emptyMagazineSound;
    public float emptyMagazineVolume = 0.5f;

    public AudioClip reloadPistolSound;
    public float reloadPistolVolume = 0.5f;

    public AudioClip reloadRifleSound;
    public float reloadRifleVolume = 0.5f;

    public AudioClip reloadStaffSound;
    public float reloadStaffVolume = 0.5f;

    public AudioClip shotPistolSound;
    public float shotPistolVolume = 0.5f;

    public AudioClip shotRifleSound;
    public float shotRifleVolume = 0.5f;

    public AudioClip shotStaffSound;
    public float shotStaffVolume = 0.5f;

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

    public void PlayEmptyMagazine()
    {
        PlaySound(emptyMagazineSound, emptyMagazineVolume);
    }

    public void PlayReloadPistol()
    {
        PlaySound(reloadPistolSound, reloadPistolVolume);
    }

    public void PlayReloadRifle()
    {
        PlaySound(reloadRifleSound, reloadRifleVolume);
    }

    public void PlayReloadStaff()
    {
        PlaySound(reloadStaffSound, reloadStaffVolume);
    }

    public void PlayShotPistol()
    {
        PlaySound(shotPistolSound, shotPistolVolume);
    }

    public void PlayShotRifle()
    {
        PlaySound(shotRifleSound, shotRifleVolume);
    }

public void PlayShotStaff()
{
    PlaySound(shotStaffSound, shotStaffVolume);
}

    void Start()
    {
        userVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
    
    public void PlaySound(AudioClip clip, float volume, float minInterval = 0.05f)
    {
        if (clip == null || sfxSource == null) return;
        
        // Не проигрывать тот же звук чаще чем раз в minInterval секунд
        if (clip == lastPlayedClip && Time.time - lastPlayedTime < minInterval) return;
        
        lastPlayedClip = clip;
        lastPlayedTime = Time.time;
        sfxSource.PlayOneShot(clip, volume * userVolume);
    }
    
    public void SetSFXVolume(float volume)
    {
        userVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
