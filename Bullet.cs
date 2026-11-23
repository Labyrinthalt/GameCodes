using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int bulletDamage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet touched: " + collision.gameObject.name);

        // Check the object we hit, and if not there, check its parent object!
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy == null)
        {
            enemy = collision.GetComponentInParent<Enemy>();
        }

        if (enemy != null)
        {
            enemy.TakeDamage(bulletDamage);
            Destroy(gameObject); 
        }
    }
}