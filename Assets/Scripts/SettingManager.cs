using UnityEngine;

public class SettingManager : MonoBehaviour
{
    [Header("Gắn Panel cài đặt vào đây")]
    public GameObject settingPanel;

    private void Start()
    {
        // Khi game vừa bắt đầu, ẩn bảng Setting đi
        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
        }
    }

    public void OpenSettingPanel()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(true);
        }
    }

    public void CloseSettingPanel()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
        }
    }
}
