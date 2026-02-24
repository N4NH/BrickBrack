using UnityEngine;
using UnityEngine.UI;

public class PieceUI : MonoBehaviour
{
    public RectTransform container;     // chính nó
    public Image cellPrefab;            // prefab ô nhỏ (UI Image)
    public float uiCellSize = 30f;

    [HideInInspector] public Vector2Int[] shape;

    private readonly System.Collections.Generic.List<Image> cells = new();

    void Awake()
    {
        if (container == null) container = GetComponent<RectTransform>();
    }

    public void SetShape(Vector2Int[] newShape)
    {
        shape = newShape;

        // clear old
        for (int i = 0; i < cells.Count; i++)
            if (cells[i] != null) Destroy(cells[i].gameObject);
        cells.Clear();

        // spawn new cells
        for (int i = 0; i < shape.Length; i++)
        {
            Image c = Instantiate(cellPrefab, container);
            RectTransform rt = c.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(uiCellSize, uiCellSize);
            rt.anchoredPosition = new Vector2(shape[i].x * uiCellSize, shape[i].y * uiCellSize);
            cells.Add(c);
        }

        // auto size container
        int maxX = 0, maxY = 0;
        for (int i = 0; i < shape.Length; i++)
        {
            if (shape[i].x > maxX) maxX = shape[i].x;
            if (shape[i].y > maxY) maxY = shape[i].y;
        }
        container.sizeDelta = new Vector2((maxX + 1) * uiCellSize, (maxY + 1) * uiCellSize);
    }
    void Start()
{
    if (shape == null || shape.Length == 0)
        SetShape(PieceLibrary.RandomShape());
}
}