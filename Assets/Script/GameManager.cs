using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardManager board;
    public PieceUI[] pieces; // giữ như này cũng được, nhưng phải kéo đúng object có PieceUI

    private bool isGameOver = false;

    public void CheckGameOver()
    {
        if (isGameOver) return;

        // Nếu gán sai thì báo rõ, KHÔNG kết luận thua
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null)
            {
                Debug.LogError($"GameManager: pieces[{i}] is NULL. Kéo ĐÚNG object có component PieceUI vào mảng pieces.");
                return;
            }
            if (pieces[i].shape == null || pieces[i].shape.Length == 0)
            {
                Debug.LogError($"GameManager: pieces[{i}] shape NULL/empty. PieceUI chưa SetShape hoặc kéo sai object.");
                return;
            }
        }

        // Check thật: chỉ thua nếu cả 3 đều không đặt được
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
            Debug.Log("GAME OVER");
        }
    }
}