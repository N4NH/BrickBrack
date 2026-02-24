using UnityEngine;
using TMPro;

public class BoardManager : MonoBehaviour
{
    public int score = 0;
    public TMP_Text scoreText;

    public int width = 9;
    public int height = 9;
    public float cellSize = 1f;

    public GameObject blockPrefab;

    private Transform[,] grid;

    void Start()
    {
        grid = new Transform[width, height];
        UpdateScoreUI();
    }

    void PlaceBlockAtMouse()
    {
        if (Camera.main == null)
        {
            Debug.LogError("No Main Camera found. Tag your camera as MainCamera.");
            return;
        }

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;

        int x = Mathf.RoundToInt(worldPos.x / cellSize);
        int y = Mathf.RoundToInt(worldPos.y / cellSize);

        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            if (grid[x, y] == null)
            {
                Vector3 spawnPos = new Vector3(x * cellSize, y * cellSize, 0);
                GameObject block = Instantiate(blockPrefab, spawnPos, Quaternion.identity);
                grid[x, y] = block.transform;

                // ✅ chuẩn: check hàng + cột và xoá 1 lần (không double destroy)
                ResolveLines();
            }
        }
    }

    void ResolveLines()
    {
        bool[] fullRows = new bool[height];
        bool[] fullCols = new bool[width];

        int rowsCount = 0;
        int colsCount = 0;

        // Check rows
        for (int y = 0; y < height; y++)
        {
            bool full = true;
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] == null) { full = false; break; }
            }
            fullRows[y] = full;
            if (full) rowsCount++;
        }

        // Check cols
        for (int x = 0; x < width; x++)
        {
            bool full = true;
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null) { full = false; break; }
            }
            fullCols[x] = full;
            if (full) colsCount++;
        }

        if (rowsCount == 0 && colsCount == 0) return;

        // Clear once (no double-destroy)
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
        Debug.Log("Score: " + score);
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
    public bool WorldToCell(Vector3 worldPos, out int x, out int y)
{
    x = Mathf.RoundToInt(worldPos.x / cellSize);
    y = Mathf.RoundToInt(worldPos.y / cellSize);
    return (x >= 0 && x < width && y >= 0 && y < height);
}

public Vector3 CellToWorld(int x, int y)
{
    return new Vector3(x * cellSize, y * cellSize, 0f);
}

public bool CanPlaceSingle(int x, int y)
{
    return (x >= 0 && x < width && y >= 0 && y < height && grid[x, y] == null);
}

public bool PlaceSingleAt(int x, int y)
{
    if (!CanPlaceSingle(x, y)) return false;

    Vector3 spawnPos = new Vector3(x * cellSize, y * cellSize, 0);
    GameObject block = Instantiate(blockPrefab, spawnPos, Quaternion.identity);
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

        Vector3 pos = new Vector3(x * cellSize, y * cellSize, 0f);
        GameObject block = Instantiate(blockPrefab, pos, Quaternion.identity);
        grid[x, y] = block.transform;
    }

    ResolveLines();
    return true;
}

public bool CanPlaceAnywhere(Vector2Int[] shape)
{
    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            if (CanPlaceShape(shape, x, y))
                return true;
        }
    }
    return false;
}
}