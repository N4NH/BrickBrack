using UnityEngine;
using UnityEngine.UI;

public class BannerPadding : MonoBehaviour
{
    [SerializeField] private RectTransform CanvasRT;
    [SerializeField] private LayoutElement Banner_Padding;
    void Start()
    {
        Invoke("ApplyPadding", 1.5f);
    }

    public void ApplyPadding()
    {
        float pixelHeight = GameManager.Instance.GetBottomBannerHeight();

        if (pixelHeight > 0 && Banner_Padding != null)
        {
            float canvasHeight = CanvasRT.rect.height;
            float screenHeight = Screen.height;
            float finalPadding = pixelHeight * (canvasHeight / screenHeight);

            Banner_Padding.preferredHeight = finalPadding;
        }
    }
}