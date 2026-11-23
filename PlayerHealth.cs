using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth = 100f;
    public float currentHealth { get; private set; }
    public float MaxHealth => startingHealth;

    [Header("UI Reference")]
    [SerializeField] private HeartIconController heartIconController;

    private bool isDead;

    private void Awake()
    {
        currentHealth = startingHealth;
        isDead = false;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            TakeDamage(enemy.damage);
        }
    }

    public void TakeDamage(float _damage)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
        UpdateUI();
    }

   
    public void ResetStats()
    {
        CancelInvoke();
        currentHealth = startingHealth;
        isDead = false;

        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = true;

        UpdateUI();
    }

    void UpdateUI()
    {
        if (heartIconController != null)
        {
            heartIconController.maxHealth = startingHealth;
            heartIconController.UpdateHealthUI(currentHealth);
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.enabled = false;
        }

        Debug.Log("Player Died");

        if (RespawnManager.Instance != null)
            RespawnManager.Instance.OnPlayerDied();
        else
            Debug.LogWarning("Walang RespawnManager sa scene!");
    }
}