using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    void Awake()
    {
        gameOverPanel.SetActive(false);
    }
    void Start()
    {
        GameEvent.GameOver += OnGameOver;
    }
    private void OnGameOver(bool isNewBestScore)
    {
        GameManager.Instance.ShowInterstitialAd();
        gameOverPanel.SetActive(true);
    }
}
