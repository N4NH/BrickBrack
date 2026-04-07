using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class GoogleAdsMob : MonoBehaviour
{
    private float _lastAdTimestamp = -999f;
    [SerializeField] private float cooldownTime = 30f;
    public void LoadAllAds()
    {
        MobileAds.Initialize((InitializationStatus status) =>
        {
            LoadBottomBanner();
            LoadInterstitialAd();
            LoadAppOpenAd();
            LoadRewardedAd();
        });
    }
    private void OnApplicationQuit()
    {
        _bottomBannerView?.Destroy();
        InterstitiaAd?.Destroy();
        _appOpenAd?.Destroy();
        _rewardedAd?.Destroy();
    }
    public bool CanShowAd()
    {
        float timePassed = Time.time - _lastAdTimestamp;
        return timePassed >= cooldownTime;
    }

    #region BANNER 
    private BannerView _bottomBannerView;
    private string Id_Banner1 = "ca-app-pub-1666762810401308/3098652924";
    public void ReShowBottomBanner()
    {
        if (_bottomBannerView == null)
        {
            LoadBottomBanner();
        }
        else
        {
            _bottomBannerView.Show();
        }
    }

    public void LoadBottomBanner()
    {
        if (_bottomBannerView != null) return;
        AdSize adaptiveSize = AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        _bottomBannerView = new BannerView(Id_Banner1, adaptiveSize, AdPosition.Bottom);
        _bottomBannerView.LoadAd(new AdRequest());
    }
    public float GetBottomBannerHeight()
    {
        if (_bottomBannerView != null)
        {
            return _bottomBannerView.GetHeightInPixels();
        }
        return 0;
    }

    #endregion


    #region INTERSTITIAL
    private string Id_InterstitiaAd = "ca-app-pub-1666762810401308/3063624792";
    private InterstitialAd InterstitiaAd;

    private void LoadInterstitialAd()
    {
        if (InterstitiaAd != null)
        {
            InterstitiaAd.Destroy();
            InterstitiaAd = null;
        }
        Debug.Log("Loading interstitial ad.");
        var adRequest = new AdRequest();
        InterstitialAd.Load(Id_InterstitiaAd, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Interstitial ad failed to load: " + error);
                    return;
                }
                Debug.Log("Interstitial ad loaded.");
                InterstitiaAd = ad;
                RegisterEventHandlers(ad);
            });
    }

    public void ShowInterstitialAd()
    {
        if (!CanShowAd())
        {
            Debug.Log("Ad is on cooldown. Please wait.");
            return;
        }

        if (InterstitiaAd != null && InterstitiaAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _lastAdTimestamp = Time.time;
            InterstitiaAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
            LoadInterstitialAd();
        }
    }

    private void RegisterEventHandlers(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad closed. Loading next one...");
            LoadInterstitialAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to show: " + error);
            LoadInterstitialAd();
        };
    }
    #endregion


    #region OpenAd
    private string _appOpenAdUnitId = "ca-app-pub-1666762810401308/8170110614";

    private AppOpenAd _appOpenAd;
    private DateTime _expireTime;

    public void LoadAppOpenAd()
    {
        if (_appOpenAd != null)
        {
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }

        Debug.Log("Loading App Open Ad...");
        var adRequest = new AdRequest();

        AppOpenAd.Load(_appOpenAdUnitId, adRequest, (AppOpenAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("App Open Ad failed to load: " + error);
                return;
            }

            Debug.Log("App Open Ad loaded.");
            _appOpenAd = ad;
            _expireTime = DateTime.Now.AddHours(4);
        });
    }

    public void ShowAppOpenAd()
    {
        if (!CanShowAd())
        {
            
            Debug.Log("Ad is on cooldown. Please wait.");
            return;
        }
        if (_appOpenAd != null && _appOpenAd.CanShowAd() && DateTime.Now < _expireTime)
        {
            Debug.Log("Showing App Open Ad.");
            _lastAdTimestamp = Time.time;
            _appOpenAd.Show();
        }
        else
        {
            LoadAppOpenAd();
        }
    }
    #endregion

    #region RewardedAd
    private string _rewardedAdUnitId = "ca-app-pub-1666762810401308/9606684476";
    private RewardedAd _rewardedAd;

    public void LoadRewardedAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading rewarded ad.");
        var adRequest = new AdRequest();

        RewardedAd.Load(_rewardedAdUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load: " + error);
                return;
            }
            Debug.Log("Rewarded ad loaded.");
            _rewardedAd = ad;

            RegisterRewardedHandlers(ad);
        });
    }

    public void ShowRewardedAd(Action onUserEarnedReward)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("User earned reward.");
                onUserEarnedReward?.Invoke();
            });
        }
        else
        {
            GameManager.Instance.AdsTB();
            Debug.LogError("Rewarded ad is not ready yet.");
            LoadRewardedAd();
        }
    }

    private void RegisterRewardedHandlers(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad closed.");
            LoadRewardedAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to show: " + error);
            LoadRewardedAd();
        };
    }
    #endregion

}