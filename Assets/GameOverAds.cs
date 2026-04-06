using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverAds : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.ShowInterstitialAd();
    }
}
