using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private float radius;
    [SerializeField] private int explosionForce;
    [SerializeField] private float explosionTime;
    [SerializeField] private GameObject explosionPrefab;

    private float explosionTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 42f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        explosionTimer += Time.fixedDeltaTime;
        if (explosionTimer >= explosionTime)
        {
            //do explosion
            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, radius, interactLayer);

            foreach(Collider2D coll in collisions)
            {
                if(coll.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    Vector3 direction = rb.transform.position - transform.position;
                    rb.AddForce(direction.normalized * explosionForce);
                }
            }

            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
