using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatBar : MonoBehaviour
{
    [Header("UI Components")]
    public Image barImage;
    public TMP_Text percentText;

    [Header("Sprites (Order: 0% to 100%)")]
    public Sprite[] statSprites;

    [Header("Settings")]
    public float maxValue = 100f;
    public float currentValue = 100f;
    public float decayAmount = 1f;
    public float decayInterval = 25f;

    [Header("Health Penalty (damage lang kapag PAREHONG 0 ang hunger at thirst)")]
    public float damageAmount = 1f;

    private PlayerHealth playerHealth;
    private StatBar otherBar;
    private float timer;

    private void Start()
    {
        currentValue = maxValue;
        playerHealth = FindAnyObjectByType<PlayerHealth>();

        // Hanapin ang kabilang bar (hunger <-> thirst)
        StatBar[] bars = FindObjectsByType<StatBar>();
        foreach (StatBar bar in bars)
        {
            if (bar != this) otherBar = bar;
        }

        UpdateUI();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= decayInterval)
        {
            timer = 0f;
            TickDecay();
        }
    }

    private void TickDecay()
    {
        if (currentValue > 0)
        {
            currentValue = Mathf.Max(0, currentValue - decayAmount);
            UpdateUI();
        }

        bool iAmDamageSource = otherBar == null || GetEntityId() < otherBar.GetEntityId();
        bool bothEmpty = currentValue <= 0f && (otherBar == null || otherBar.currentValue <= 0f);

        if (iAmDamageSource && bothEmpty && playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }

    public void ModifyValue(float amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, 0, maxValue);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (statSprites != null && statSprites.Length > 0 && barImage != null)
        {
            float percent = Mathf.Clamp01(currentValue / maxValue);
            int spriteIndex = Mathf.FloorToInt(percent * (statSprites.Length - 1));
            spriteIndex = Mathf.Clamp(spriteIndex, 0, statSprites.Length - 1);
            barImage.sprite = statSprites[spriteIndex];
        }

        if (percentText != null)
            percentText.text = $"{Mathf.RoundToInt(currentValue)}%";
    }
}