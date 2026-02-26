using UnityEngine;
using System.IO; // Thư viện bắt buộc để thao tác với File

public class SaveManager : MonoBehaviour
{
    // Tạo Singleton để có thể gọi SaveManager từ bất kỳ script nào khác
    public static SaveManager Instance { get; private set; }

    // Biến lưu trữ dữ liệu hiện tại của game
    public GameData currentData;

    // Đường dẫn file lưu trữ trên máy
    private string saveFilePath;

    private void Awake()
    {
        // Thiết lập Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Giữ SaveManager không bị hủy khi chuyển Scene

        // Application.persistentDataPath là đường dẫn an toàn nhất, hoạt động trên cả PC, Android, iOS
        saveFilePath = Application.persistentDataPath + "/brickbrack_save.json";
        
        // Tự động tải dữ liệu ngay khi game mở lên
        LoadGame();
    }

    // Hàm Lưu Game
    public void SaveGame()
    {
        // Chuyển Object C# thành chuỗi JSON (true: format cho dễ đọc)
        string json = JsonUtility.ToJson(currentData, true);
        
        // Ghi chuỗi JSON đó ra file vật lý trên máy tính/điện thoại
        File.WriteAllText(saveFilePath, json);
        
        Debug.Log("Đã lưu game tại: " + saveFilePath);
    }

    // Hàm Tải Game
    public void LoadGame()
    {
        // Kiểm tra xem file save đã tồn tại chưa
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            // Giải mã JSON ngược lại thành Object GameData
            currentData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Đã tải dữ liệu game thành công!");
        }
        else
        {
            Debug.Log("Không tìm thấy file save. Tạo dữ liệu mới.");
            currentData = new GameData();
        }
    }
    
    // Hàm test chức năng (Có thể xóa sau này)
    [ContextMenu("Xóa Save Hiện Tại")]
    public void DeleteSaveData()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            currentData = new GameData();
            Debug.Log("Đã xóa file save!");
        }
    }
}