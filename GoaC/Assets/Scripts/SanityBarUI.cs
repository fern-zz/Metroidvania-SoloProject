using UnityEngine;
using UnityEngine.UI;

public class SanityBarUI : MonoBehaviour
{
    public Image sanityImage;
    public Sprite[] sanityFrames;

    private int currentSanity;
    private int maxSanity;

    public void SetSanity(int current, int max)
    {
        maxSanity = max;
        currentSanity = current;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (maxSanity == 0 || sanityFrames.Length == 0) return;

        float percent = currentSanity / (float)maxSanity;

        int index = Mathf.Clamp(
            sanityFrames.Length - 1 - Mathf.FloorToInt(percent * sanityFrames.Length),
            0,
            sanityFrames.Length - 1
        );

        sanityImage.sprite = sanityFrames[index];
    }
}
