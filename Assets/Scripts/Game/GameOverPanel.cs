using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject NewBestScore;
    void Awake()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            
        }
    }
    void Start()
    {
        NewBestScore.SetActive(false);
        GameEvent.GameOver += OnGameOver;
    }
    private void OnDestroy()
    {
        GameEvent.GameOver -= OnGameOver;
    }
    private void OnGameOver(bool isNewBestScore)
    {
        if(isNewBestScore)
        {
            NewBestScore.SetActive(true);
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
