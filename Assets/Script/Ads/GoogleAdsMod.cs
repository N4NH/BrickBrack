using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAdsMod : MonoBehaviour
{
    private BannerView bannerView;

    private string adUnitId = "ca-app-pub-1666762810401308/3098652924";

    void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            LoadBanner();
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
}