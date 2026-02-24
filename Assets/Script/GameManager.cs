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

        bool anyPlaceable = false;
        int validPiecesCount = 0;

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null || !pieces[i].gameObject.activeInHierarchy) continue;
            if (pieces[i].shape == null || pieces[i].shape.Length == 0) continue;

            validPiecesCount++;

            if (board.CanPlaceAnywhere(pieces[i].shape))
            {
                anyPlaceable = true;
                break;
            }
        }

        // Chỉ kết luận Game Over nếu có ít nhất 1 piece hợp lệ đang hiển thị và KHÔNG THỂ đặt bất cứ piece nào
        if (validPiecesCount > 0 && !anyPlaceable)
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