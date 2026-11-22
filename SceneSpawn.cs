using System.Collections;
using UnityEngine;

public class SceneSpawn : MonoBehaviour
{
    private IEnumerator Start()
    {
        if (!ZoneSelector.needsSpawn) yield break;
        ZoneSelector.needsSpawn = false;

        // Maghintay ng isang frame para mabura muna ang duplicate na Player
        yield return null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("SceneSpawn: walang Player na may tag 'Player'");
            yield break;
        }

        player.transform.position = transform.position;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }
}