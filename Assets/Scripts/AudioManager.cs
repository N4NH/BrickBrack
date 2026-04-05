using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource; 
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip placeBlockClip; 
    public AudioClip clearLineClip;

    public bool isMusicOn = true;
    public bool isSoundOn = true;
    public bool isVibrateOn = true;

    private void Awake()
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
        LoadSettings();
    }

    private void Start()
    {
        if (isMusicOn && bgmSource.clip != null)
        {
            bgmSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (isSoundOn && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void Vibrate()
    {
        if (isVibrateOn)
        {
            Handheld.Vibrate();
        }
    }

    // --- CÁC HÀM DÙNG CHO NÚT BẤM TRONG SETTING ---
    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        if (isMusicOn) bgmSource.Play(); else bgmSource.Stop();
        PlayerPrefs.SetInt("Music", isMusicOn ? 1 : 0);
    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt("Sound", isSoundOn ? 1 : 0);
    }

    public void ToggleVibrate()
    {
        isVibrateOn = !isVibrateOn;
        PlayerPrefs.SetInt("Vibrate", isVibrateOn ? 1 : 0);
    }

    private void LoadSettings()
    {
        isMusicOn = PlayerPrefs.GetInt("Music", 1) == 1;
        isSoundOn = PlayerPrefs.GetInt("Sound", 1) == 1;
        isVibrateOn = PlayerPrefs.GetInt("Vibrate", 1) == 1;
    }
}