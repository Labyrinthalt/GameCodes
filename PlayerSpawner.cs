using System.Collections;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private IEnumerator Start()
    {
        // Maghintay ng isang frame para mabura muna ang duplicate na Player
        yield return null;

        string id = SpawnData.nextSpawnId;
        SpawnData.nextSpawnId = "";

        if (string.IsNullOrEmpty(id)) yield break;

        GameObject spawn = GameObject.Find(id);
        GameObject player = GameObject.FindWithTag("Player");

        if (spawn == null)
        {
            Debug.LogWarning("PlayerSpawner: walang SpawnPoint na ang pangalan ay '" + id + "'");
            yield break;
        }
        if (player == null)
        {
            Debug.LogWarning("PlayerSpawner: walang Player na may tag 'Player'");
            yield break;
        }

        // Pwesto at laki ayon sa SpawnPoint
        player.transform.position = spawn.transform.position;
        player.transform.localScale = spawn.transform.localScale;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }
}