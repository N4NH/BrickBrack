using UnityEngine;

public class Revice : MonoBehaviour
{
    [Header("UI & Game Component")]
    public GameObject gameOverPanel; // Kéo thả cái GameOverPanel vào đây
    public Grid gridManager;         // Kéo thả thẻ chứa script Grid vào đây

    public void OnReviveButtonClicked()
    {
        GameManager.Instance.ShowRewardedAd(ExecuteRevive);
    }

    private void ExecuteRevive()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        if (gridManager != null)
        {
            gridManager.ClearLinesForRevive();
        }
    }
}
