using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BoardManager board;
    public RectTransform rectTransform;

    private Vector3 startPos;
    private PieceUI pieceUI;
    private GhostPreview ghost;
    void Awake()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        pieceUI = GetComponent<PieceUI>();
        ghost = GetComponent<GhostPreview>();
        startPos = rectTransform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rectTransform.position;
        Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        world.z = 0f;
        if (ghost != null) ghost.ShowAtWorld(world);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position += (Vector3)eventData.delta;
        Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        world.z = 0f;
        if (ghost != null) ghost.ShowAtWorld(world);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ghost != null) ghost.Hide();

        Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        world.z = 0f;

        if (board.WorldToCell(world, out int ax, out int ay) &&
            pieceUI != null &&
            board.PlaceShape(pieceUI.shape, ax, ay))
        {
            // đặt thành công -> random shape mới + về slot
            pieceUI.SetShape(PieceLibrary.RandomShape());
            rectTransform.position = startPos;
            FindObjectOfType<GameManager>().CheckGameOver();
        }
        else
        {
            rectTransform.position = startPos;
        }
    }
}