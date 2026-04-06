using UnityEngine;
using UnityEngine.UI; // Phải thêm thư viện này để thao tác với Image

public class SettingManager : MonoBehaviour
{
    public GameObject settingPanel;

    [Header("Gắn các UI Image của nút bấm vào đây")]
    public Image musicIcon;
    public Image soundIcon;
    public Image vibrateIcon;

    [Header("Gắn các hình ảnh (Sprite) Trạng thái Bật/Tắt vào đây")]
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public Sprite vibrateOnSprite;
    public Sprite vibrateOffSprite;

    public void OpenSettingPanel()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(true);
        }
        UpdateUI();
    }

    public void CloseSettingPanel()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
        }
    }

    // --- CÁC HÀM GẮN VÀO SỰ KIỆN CLICK CỦA NÚT BẤM ---

    public void OnMusicButtonClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleMusic();
            UpdateUI();
        }
    }

    public void OnSoundButtonClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleSound();
            UpdateUI();
        }
    }

    public void OnVibrateButtonClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleVibrate();
            UpdateUI();
        }
    }

    // --- HÀM CẬP NHẬT HÌNH ẢNH ICON ---
    private void UpdateUI()
    {
        if (AudioManager.Instance == null) return;

        // Nếu isMusicOn là true thì dùng hình On, ngược lại dùng hình Off
        if (musicIcon != null)
            musicIcon.sprite = AudioManager.Instance.isMusicOn ? musicOnSprite : musicOffSprite;

        if (soundIcon != null)
            soundIcon.sprite = AudioManager.Instance.isSoundOn ? soundOnSprite : soundOffSprite;

        if (vibrateIcon != null)
            vibrateIcon.sprite = AudioManager.Instance.isVibrateOn ? vibrateOnSprite : vibrateOffSprite;
    }
}