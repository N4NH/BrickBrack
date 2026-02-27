using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BoardManager board;
    public RectTransform rectTransform;

    [Header("Drag Offset")]
    public float dragOffsetY = 10; // Khoảng cách bay lên so với con trỏ (tính theo hệ World Space)

    private Vector3 startPos;
    private PieceUI pieceUI;
    private GhostPreview ghost;
    private Camera mainCamera;

    void Awake()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        pieceUI = GetComponent<PieceUI>();
        ghost = GetComponent<GhostPreview>();
        mainCamera = Camera.main;
        startPos = rectTransform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rectTransform.position;
        Vector3 world = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        world.z = 0f;
        world.y += dragOffsetY; // Dịch lên trần
        if (ghost != null) ghost.ShowAtWorld(world);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Khi kéo, di chuyển bằng Screen point và cộng thẳng eventData.delta
        // Tính world pos cho Ghost chạy theo Offset
        Vector3 world = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        world.z = 0f;
        world.y += dragOffsetY;

        // Cập nhật vị trí UI (rect transform) để mảnh ghép dính lệch lên trên điểm chạm
        // Convert the offset world point back to local space of the canvas
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rectTransform.parent as RectTransform,
            Input.mousePosition,
            eventData.pressEventCamera,
            out Vector3 globalMousePos);
            
        // Áp dụng offset (đoán chừng theo canvas scale, hoặc dùng code tay) 
        // Thay vì cộng delta, ta set cứng theo chuột + offset để nó không bị trôi
        rectTransform.position = globalMousePos + new Vector3(0, dragOffsetY, 0);

        if (ghost != null) ghost.ShowAtWorld(world);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ghost != null) ghost.Hide();

        Vector3 world = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        world.z = 0f;
        world.y += dragOffsetY; // Xét lúc buông tay cũng phải có offset

        if (board.WorldToCell(world, out int ax, out int ay) &&
            pieceUI != null &&
            board.PlaceShape(pieceUI.shape, ax, ay))
        {
            // đặt thành công -> random shape mới + về slot
            pieceUI.SetShape(PieceLibrary.RandomShape());
            rectTransform.position = startPos;
            GameManager.Instance.CheckGameOver();
        }
        else
        {
            rectTransform.position = startPos;
        }
    }
}