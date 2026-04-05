using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public GameObject settingPanel;

    private void Start()
    {

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
