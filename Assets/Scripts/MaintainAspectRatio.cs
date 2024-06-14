using UnityEngine;
using UnityEngine.UI;

public class MaintainAspectRatio : MonoBehaviour
{
    public RectTransform backgroundImageRect;

    void Start()
    {
        AdjustBackground();
    }

    void AdjustBackground()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float imageAspect = backgroundImageRect.rect.width / backgroundImageRect.rect.height;

        if (screenAspect >= imageAspect)
        {
            // Wider screen, match height
            backgroundImageRect.sizeDelta = new Vector2(backgroundImageRect.rect.height * screenAspect, backgroundImageRect.rect.height);
        }
        else
        {
            // Taller screen, match width
            backgroundImageRect.sizeDelta = new Vector2(backgroundImageRect.rect.width, backgroundImageRect.rect.width / screenAspect);
        }
    }

    void Update()
    {
        // Optional: Re-adjust on resolution change
        if (Screen.width != backgroundImageRect.rect.width || Screen.height != backgroundImageRect.rect.height)
        {
            AdjustBackground();
        }
    }
}
