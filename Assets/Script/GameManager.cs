using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GoogleAdsMod adsMod;
    public BoardManager board;
    public PieceUI[] pieces;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            adsMod.LoadInterstitialAd();
        }
        else
        {
            Destroy(gameObject);
        }
    }
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
        isGameOver = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (board != null)
            board.ResetBoard();

        if (pieces != null)
        {
            for (int i = 0; i < pieces.Length; i++)
            {
                if (pieces[i] != null)
                {
                    pieces[i].gameObject.SetActive(true);
                    pieces[i].SetShape(PieceLibrary.RandomShape());
                    
                    // Để an toàn, trả nó về đúng vị trí đầu tiên của khay (hoặc RectTransform tự handle)
                    var rect = pieces[i].GetComponent<RectTransform>();
                    DraggablePiece draggable = pieces[i].GetComponent<DraggablePiece>();
                    // DraggablePiece tự kéo về lúc OnEndDrag rồi nên thường không bị lỗi pos
                }
            }
        }
    }
    public void ShowAds()
    {
        if (adsMod != null)
        {
            adsMod.ShowInterstitialAd();
        }
    }
    public void resetAds()
    {
        if (adsMod != null)
        {
            adsMod.DestroyInterstitialAd();
            adsMod.LoadInterstitialAd();
        }
    }
}