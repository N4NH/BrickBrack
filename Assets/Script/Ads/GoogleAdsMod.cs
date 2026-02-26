using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAdsMod : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd interstitialAd;

    // ID quảng cáo của bạn
    private string adUnitId = "ca-app-pub-1666762810401308/3063624792";

    private string interstitialAdUnitId = "ca-app-pub-1666762810401308/3098652924";

    void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            LoadBanner();
        });
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadInterstitialAd();
        });
    }

    private void LoadBanner()
    {
        int screenWidth = (int)(Screen.width / MobileAds.Utils.GetDeviceScale());
        AdSize adaptiveSize =
            AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(screenWidth);

        bannerView = new BannerView(
            adUnitId,
            adaptiveSize,
            AdPosition.Bottom
        );
        AdRequest request = new AdRequest();
        bannerView.LoadAd(request);
    }
    public void HideBanner()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
        }
    }
    public void ShowBanner()
    {
        if (bannerView != null)
        {
            bannerView.Show();
        }
    }

    // Huỷ quảng cáo khi thoát scene
    private void OnDestroy()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }
    public void LoadInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading Interstitial Ad...");

        AdRequest adRequest = new AdRequest();

        InterstitialAd.Load(interstitialAdUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Interstitial failed to load: " + error);
                    return;
                }

                Debug.Log("Interstitial loaded successfully");
                interstitialAd = ad;
            });
    }
    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial not ready yet");
        }
    }
    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
    }
}