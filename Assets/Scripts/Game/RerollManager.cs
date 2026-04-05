using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RerollManager : MonoBehaviour
{
    [Header("Giao diện UI")]
    public TMP_Text rerollText;          // Kéo thẻ TextMeshPro hiển thị số 3, 2, 1 vào đây
    public GameObject adsIcon;       // (Tùy chọn) Nút hình Tivi/Video Ads. Kéo vào đây nếu có

    [Header("Hệ thống")]
    public ShapeStorage shapeStorage; // Kéo thẻ ShapeStorage của bạn chứa 3 mảnh hình vào đây

    private int freeRerolls = 3;

    private void Start()
    {
        freeRerolls = 3;
        UpdateUI();
    }

    public void OnRerollButtonClicked()
    {
        // [TÍNH NĂNG CHỐNG BUG]: 
        // Chỉ cho phép Reroll khi người chơi không cầm mảnh ghép nào lơ lửng trên tay.
        if (shapeStorage != null)
        {
            foreach (var shape in shapeStorage.shapeList)
            {
                // Nếu mảnh ghép đang sáng (chưa thả xuống lưới) VÀ đang không tọa lạc tĩnh tại khay
                if (shape.IsAnyOfShapeSquareActive() && !shape.IsOnStartPosition())
                {
                    Debug.Log("Vui lòng thả khối đang cầm xuống lưới hoặc trả về khay trước khi Reroll!");
                    return; 
                }
            }
        }

        // Phân luồng: Còn lượt -> Đổi luôn. Hết lượt -> Coi Ads.
        if (freeRerolls > 0)
        {
            freeRerolls--;
            UpdateUI();
            ExecuteReroll();
        }
        else
        {
            // Tận dụng lại hàm Ads bạn đã tạo ở bài Code trước (Revive)
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ShowRewardedAd(ExecuteReroll);
            }
            else
            {
                Debug.Log("[Quảng cáo] Đang chờ xem quảng cáo để đổi khối...");
                // Note cho bạn: Nếu đang Test trên máy tính k gắn Ads, bạn có thể Tạm bôi xóa 2 dấu '//' ở dòng dưới để test dễ
                // ExecuteReroll();
            }
        }
    }

    private void ExecuteReroll()
    {
        // 1. Bắn tín hiệu sang ShapeStorage để nó tự động đẻ 3 hình khối ngẫu nhiên mới!
        GameEvent.RequestNewShape?.Invoke();

        // 2. Chắc cốp: Bật sáng đèn tất cả mảnh ghép để tránh bị tối màu do dính dư âm.
        if (shapeStorage != null)
        {
            foreach (var shape in shapeStorage.shapeList)
            {
                shape.ActivateShape();
            }
        }
    }

    private void UpdateUI()
    {
        // Hiển thị số lượt lên chữ (Ví dụ: "3")
        if (rerollText != null)
        {
            rerollText.text = freeRerolls.ToString();
        }

        // Tính năng Tự động xịn: Nếu hết 3 lượt, nó sẽ giấu Số đi, và móc cái Ảnh Tivi Quảng cáo lên!
        if (adsIcon != null)
        {
            bool hetLuotCày = (freeRerolls <= 0);
            
            // Hiện icon Ads nếu hết lượt
            adsIcon.SetActive(hetLuotCày);
            
            // Ẩn chữ số đếm nếu hiện UI ảnh Ads
            if(rerollText != null) 
            {
                 rerollText.gameObject.SetActive(!hetLuotCày);
            }
        }
    }
}
