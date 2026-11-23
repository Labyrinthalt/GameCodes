using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    [Tooltip("Dapat magkakaiba ang bawat uri: Player, Canvas, atbp.")]
    [SerializeField] private string persistentId = "Player";

    private static readonly Dictionary<string, GameObject> instances = new();

    private void Awake()
    {
        // Kung may kopya na ng ganito, ang bago ay buburahin
        if (instances.TryGetValue(persistentId, out GameObject existing)
            && existing != null && existing != gameObject)
        {
            Destroy(gameObject);
            return;
        }

        instances[persistentId] = gameObject;
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }
}