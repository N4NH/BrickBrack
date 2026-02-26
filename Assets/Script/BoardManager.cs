using UnityEngine;
using TMPro;

public class BoardManager : MonoBehaviour
{
    public int score = 0;
    public TMP_Text scoreText;

    public int width = 9;
    public int height = 9;

    [Header("Board -> UI Frame Bind")]
    public RectTransform boardFrameUI;   // kéo BoardFrameUI vào đây
    public Camera worldCamera;           // thường là Main Camera
    public float paddingPercent = 0.02f; // chừa mép khung 2% (tuỳ)

    public GameObject blockPrefab;

    private Transform[,] grid;

    // --- runtime computed ---
    private Vector3 originWorld; // góc trái-dưới của board (world)
    public float cellSize = 1f;  // sẽ auto set theo frame

    void Start()
    {
        grid = new Transform[width, height];
        UpdateScoreUI();

        if (worldCamera == null) worldCamera = Camera.main;

        FitBoardToUIFrame();
    }

    void FitBoardToUIFrame()
    {
        if (boardFrameUI == null || worldCamera == null)
        {
            Debug.LogWarning("Missing boardFrameUI or worldCamera -> dùng cellSize thủ công.");
            originWorld = Vector3.zero;
            return;
        }

        // Lấy 4 góc world của UI Rect
        Vector3[] corners = new Vector3[4];
        boardFrameUI.GetWorldCorners(corners); 
        // corners: 0=BL, 1=TL, 2=TR, 3=BR (world space của UI)

        // Convert UI-world -> screen -> world của game camera
        // (vì corners đang theo world của Canvas, ta đưa về screen rồi ra world camera)
        Vector3 blScreen = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
        Vector3 trScreen = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

        Vector3 blWorld = worldCamera.ScreenToWorldPoint(new Vector3(blScreen.x, blScreen.y, -worldCamera.transform.position.z));
        Vector3 trWorld = worldCamera.ScreenToWorldPoint(new Vector3(trScreen.x, trScreen.y, -worldCamera.transform.position.z));

        float w = trWorld.x - blWorld.x;
        float h = trWorld.y - blWorld.y;

        // chừa padding cho đẹp
        float padW = w * paddingPercent;
        float padH = h * paddingPercent;
        w -= padW * 2f;
        h -= padH * 2f;

        // cellSize theo cạnh nhỏ hơn để khỏi tràn
        float sizeX = w / width;
        float sizeY = h / height;
        cellSize = Mathf.Min(sizeX, sizeY);

        // origin: góc trái-dưới + padding + (cellSize/2 nếu sprite pivot ở giữa)
        originWorld = blWorld + new Vector3(padW, padH, 0f);

        // Nếu block sprite pivot CENTER (thường vậy) thì cộng nửa ô để nó nằm trong ô
        originWorld += new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0f);

        // (optional) log để check
        Debug.Log($"FitBoardToUIFrame -> cellSize={cellSize}, origin={originWorld}");
    }

    public bool WorldToCell(Vector3 worldPos, out int x, out int y)
    {
        Vector3 local = worldPos - originWorld;
        x = Mathf.RoundToInt(local.x / cellSize);
        y = Mathf.RoundToInt(local.y / cellSize);
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    public Vector3 CellToWorld(int x, int y)
    {
        return originWorld + new Vector3(x * cellSize, y * cellSize, 0f);
    }

    public bool CanPlaceSingle(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height && grid[x, y] == null);
    }

    public bool PlaceSingleAt(int x, int y)
    {
        if (!CanPlaceSingle(x, y)) return false;

        Vector3 pos = CellToWorld(x, y);
        GameObject block = Instantiate(blockPrefab, pos, Quaternion.identity);
        grid[x, y] = block.transform;

        ResolveLines();
        return true;
    }

    public bool CanPlaceShape(Vector2Int[] shape, int ax, int ay)
    {
        for (int i = 0; i < shape.Length; i++)
        {
            int x = ax + shape[i].x;
            int y = ay + shape[i].y;
            if (x < 0 || x >= width || y < 0 || y >= height) return false;
            if (grid[x, y] != null) return false;
        }
        return true;
    }

    public bool PlaceShape(Vector2Int[] shape, int ax, int ay)
    {
        if (!CanPlaceShape(shape, ax, ay)) return false;

        for (int i = 0; i < shape.Length; i++)
        {
            int x = ax + shape[i].x;
            int y = ay + shape[i].y;

            Vector3 pos = CellToWorld(x, y);
            GameObject block = Instantiate(blockPrefab, pos, Quaternion.identity);
            grid[x, y] = block.transform;
        }

        ResolveLines();
        return true;
    }

    public bool CanPlaceAnywhere(Vector2Int[] shape)
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (CanPlaceShape(shape, x, y)) return true;

        return false;
    }

    void ResolveLines()
    {
        bool[] fullRows = new bool[height];
        bool[] fullCols = new bool[width];

        int rowsCount = 0;
        int colsCount = 0;

        for (int y = 0; y < height; y++)
        {
            bool full = true;
            for (int x = 0; x < width; x++)
                if (grid[x, y] == null) { full = false; break; }

            fullRows[y] = full;
            if (full) rowsCount++;
        }

        for (int x = 0; x < width; x++)
        {
            bool full = true;
            for (int y = 0; y < height; y++)
                if (grid[x, y] == null) { full = false; break; }

            fullCols[x] = full;
            if (full) colsCount++;
        }

        if (rowsCount == 0 && colsCount == 0) return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null) continue;
                if (fullRows[y] || fullCols[x])
                {
                    Destroy(grid[x, y].gameObject);
                    grid[x, y] = null;
                }
            }
        }

        score += (rowsCount + colsCount) * 100;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}