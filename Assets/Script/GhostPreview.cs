using System.Collections.Generic;
using UnityEngine;

public class GhostPreview : MonoBehaviour
{
    public BoardManager board;              // kéo Board vào
    public PieceUI pieceUI;                 // tự lấy nếu null

    private readonly List<GameObject> ghosts = new();
    private bool showing = false;

    void Awake()
    {
        if (pieceUI == null) pieceUI = GetComponent<PieceUI>();
    }

    public void ShowAtWorld(Vector3 worldPos)
    {
        if (board == null || pieceUI == null || pieceUI.shape == null) return;

        if (!board.WorldToCell(worldPos, out int ax, out int ay))
        {
            Hide();
            return;
        }

        bool canPlace = board.CanPlaceShape(pieceUI.shape, ax, ay);

        // Nếu chưa có ghost thì tạo
        if (!showing)
        {
            CreateGhostObjects(pieceUI.shape.Length);
            showing = true;
        }

        // Update vị trí + màu
        for (int i = 0; i < pieceUI.shape.Length; i++)
        {
            int x = ax + pieceUI.shape[i].x;
            int y = ay + pieceUI.shape[i].y;

            // nếu out-of-board thì cũng coi như invalid
            bool inside = (x >= 0 && x < board.width && y >= 0 && y < board.height);
            Vector3 pos = board.CellToWorld(x, y);

            ghosts[i].transform.position = pos;

            var sr = ghosts[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                // xanh nếu ok + inside, đỏ nếu fail
                sr.color = (canPlace && inside) ? new Color(0f, 1f, 0f, 0.45f)
                                                : new Color(1f, 0f, 0f, 0.45f);
            }

            ghosts[i].SetActive(inside); // ngoài board thì ẩn cho đỡ rối
        }
    }

    public void Hide()
    {
        if (!showing) return;
        for (int i = 0; i < ghosts.Count; i++)
            if (ghosts[i] != null) Destroy(ghosts[i]);
        ghosts.Clear();
        showing = false;
    }

    void CreateGhostObjects(int count)
    {
        // tạo ghost từ blockPrefab nhưng tắt collider nếu có
        for (int i = 0; i < count; i++)
        {
            GameObject g = Instantiate(board.blockPrefab);
            g.name = "GhostCell";
            // cho ghost nằm riêng (optional)
            // g.transform.SetParent(null);

            // tắt collider nếu có
            var col2D = g.GetComponent<Collider2D>();
            if (col2D != null) col2D.enabled = false;

            ghosts.Add(g);
        }
    }
}