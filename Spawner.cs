using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private float spawnInterval = 3f; 

    private float spawnTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnGrenade();
            spawnTimer = 0f;
        }
    }

    void SpawnGrenade()
    {
        Instantiate(grenadePrefab, transform.position, Quaternion.identity);
    }
}