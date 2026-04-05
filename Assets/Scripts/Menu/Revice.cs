using UnityEngine;

public class Revice : MonoBehaviour
{
    [Header("UI & Game Component")]
    public GameObject gameOverPanel; // Kéo thả cái GameOverPanel vào đây
    public Grid gridManager;         // Kéo thả thẻ chứa script Grid vào đây

    public void OnReviveButtonClicked()
    {
        // 1. Tạm thời mô phỏng hành động Xem Quảng Cáo ở đây. (Bạn tự chèn tuỳ ý sau này)
        Debug.Log("GỌI QUẢNG CÁO TẠI ĐÂY - CHỜ QUẢNG CÁO CHẠY XONG THÌ THỰC THI PHẦN DƯỚI");

        // Giả sử xem quảng cáo thành công, thực thi hàm hồi sinh
        ExecuteRevive();
    }

    private void ExecuteRevive()
    {
        // 2. Tắt màn hình Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // 3. Xóa 3 hàng hoặc cột bất kỳ liền nhau để có chỗ trống
        if (gridManager != null)
        {
            gridManager.ClearLinesForRevive();
        }
    }
}
