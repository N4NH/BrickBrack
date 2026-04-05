using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GoogleAdsMob googleAdsMob;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            googleAdsMob.LoadAllAds();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (googleAdsMob != null)
        {
            googleAdsMob.ReShowBottomBanner();
        }
    }

    #region Ads
    public float GetBottomBannerHeight()
    {
        if (googleAdsMob != null)
        {
            return googleAdsMob.GetBottomBannerHeight();
        }
        return 0;
    }
    public void ShowInterstitialAd()
    {
        if (googleAdsMob != null)
        {
            googleAdsMob.ShowInterstitialAd();
        }
    }
    private void OnApplicationPause(bool paused)
    {
        if (!paused && googleAdsMob != null)
        {
            googleAdsMob.ShowAppOpenAd();
        }
    }
    public void ShowRewardedAd(System.Action onUserEarnedReward)
    {
        if (googleAdsMob != null)
        {
            googleAdsMob.ShowRewardedAd(onUserEarnedReward);
        }
    }
    #endregion


}
