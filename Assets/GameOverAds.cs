using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverAds : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.Vibrate();
        GameManager.Instance.ShowInterstitialAd();

    }
}
