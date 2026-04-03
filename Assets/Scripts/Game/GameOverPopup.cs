using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPopup : MonoBehaviour
{
    public GameObject gameOverPopup;
    public GameObject loosePopup;
    public GameObject newBestScorePopup;
 
    void Start()
    {
        gameOverPopup .SetActive(false);
    }
    private void OnEnable(){
    GameEvent.GameOver += OnGameOverPopup;
    GameManager.Instance.ShowInterstitialAd();
    }
    private void OnDisable()
    {
        GameEvent.GameOver -= OnGameOverPopup;  
    }
    private void OnGameOverPopup(bool isNewBestScore)
    {
        gameOverPopup.SetActive(true);
        loosePopup.SetActive(true);
        newBestScorePopup.SetActive(true);
    }
}

