using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    [SerializeField] private string respawnSceneName = "Evergreen_Tent1"; 
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string spawnPointName = "PlayerSpawner";
    [SerializeField] private float respawnDelay = 2f;

    private bool respawning;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

  
    public void OnPlayerDied()
    {
        if (respawning) return;
        respawning = true;
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(respawnSceneName);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        GameObject spawn = GameObject.Find(spawnPointName);

        if (player != null && spawn != null)
            player.transform.position = spawn.transform.position;

        if (player != null)
        {
            
            var health = player.GetComponent<PlayerHealth>();
            if (health != null) health.ResetStats();
        }

        Time.timeScale = 1f;
        respawning = false;
    }
}