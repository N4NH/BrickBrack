using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public BoardManager board;
    public PieceUI[] pieces;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;

    private bool isGameOver = false;

    public void CheckGameOver()
    {
        if (isGameOver) return;

        // Nếu thiếu reference thì không kết luận thua
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null || pieces[i].shape == null || pieces[i].shape.Length == 0)
                return;
        }

        bool anyPlaceable = false;
        for (int i = 0; i < pieces.Length; i++)
        {
            if (board.CanPlaceAnywhere(pieces[i].shape))
            {
                anyPlaceable = true;
                break;
            }
        }

        if (!anyPlaceable)
        {
            isGameOver = true;
            ShowGameOver();
        }
    }

    void ShowGameOver()
    {
        if (finalScoreText != null)
            finalScoreText.text = "Score: " + board.score;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // chặn kéo/thả nữa (optional)
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}