using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;

    public int damage = 1;

    public int maxHealth = 3;
    private int currentHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Itala na patay na ito para hindi na bumalik pagkabalik sa scene
        WorldObjectId worldId = GetComponent<WorldObjectId>();
        if (worldId != null) worldId.MarkRemoved();

        Destroy(gameObject);
    }
}