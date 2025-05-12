using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image healthImage;
    public Sprite[] healthFrames;
    private int previousHealth;

    private int maxHealth;
    private int currentHealth;
    private bool isGlowing = false;
    private float glowTimer = 0f;
    private float glowSpeed = 0.1f;
    private int glowFrame = 9;

    public void SetHealth(int current, int max)
    {
        if (previousHealth < max && current == max)
        {
            isGlowing = true;
            glowFrame = 9;
            glowTimer = 0f;
        }

        previousHealth = current;
        maxHealth = max;
        currentHealth = current;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            healthImage.sprite = healthFrames[0];
        }
        else if (isGlowing)
        {
            glowTimer += Time.deltaTime;
            if (glowTimer >= glowSpeed)
            {
                glowTimer = 0f;
                glowFrame++;
                if (glowFrame > 16)
                {
                    glowFrame = 8;
                    isGlowing = false;
                }
                healthImage.sprite = healthFrames[glowFrame];
            }
        }
        else
        {
            int frame = (currentHealth == maxHealth) ? 8 :
            Mathf.Clamp(Mathf.FloorToInt((currentHealth / (float)maxHealth) * 8), 1, 7);
            healthImage.sprite = healthFrames[frame];
        }
    }
}
