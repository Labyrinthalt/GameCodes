using UnityEngine;
using UnityEngine.UI;

public class HeartIconController : MonoBehaviour
{
    [Header("UI Reference")]
    public Image heartImage;

    [Header("Sprites (Order: 0 HP to 100 HP)")]
    public Sprite[] healthSprites;

    [Header("Health Settings")]
    public float maxHealth;
    public float currentHealth { get; private set; }

    void Start()
    {
        if (heartImage == null)
            heartImage = GetComponent<Image>();

        UpdateHealthUI(currentHealth);
    }

    public void UpdateHealthUI(float health)
    {
        currentHealth = Mathf.Clamp(health, 0f, maxHealth);

        if (healthSprites == null || healthSprites.Length == 0) return;

        float healthPercent = currentHealth / maxHealth;

        int spriteIndex = Mathf.FloorToInt(healthPercent * (healthSprites.Length - 1));
        spriteIndex = Mathf.Clamp(spriteIndex, 0, healthSprites.Length - 1);

        heartImage.sprite = healthSprites[spriteIndex];
    }

    public void TakeDamage(float amount)
    {
        UpdateHealthUI(currentHealth - amount);
    }
}