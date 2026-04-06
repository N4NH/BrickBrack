using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    void Awake()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }
    void Start()
    {
        GameEvent.GameOver += OnGameOver;
    }
    private void OnDestroy()
    {
        GameEvent.GameOver -= OnGameOver;
    }
    private void OnGameOver(bool isNewBestScore)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ShowInterstitialAd();
        }
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
