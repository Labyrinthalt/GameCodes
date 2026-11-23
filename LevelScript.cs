using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class SpawnData
{
    public static string nextSpawnId = "";
}

public class LevelScript : MonoBehaviour
{
    [Tooltip("Pangalan ng scene (halimbawa GameScene). Ito ang gagamitin kung may laman.")]
    public string sceneName = "";

    [Tooltip("Lumang paraan. Ginagamit lang kung walang laman ang Scene Name.")]
    public int sceneBuildIndex;

    [Tooltip("Pangalan ng SpawnPoint sa susunod na scene kung saan lilitaw ang Player")]
    public string targetSpawnId = "";

    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            int index = ResolveIndex();
            if (index < 0) return;

            print("Switching Scene to index: " + index);

        
            SpawnData.nextSpawnId = targetSpawnId;

            ScreenFader fader = FindAnyObjectByType<ScreenFader>();
            if (fader == null)
            {
                SceneManager.LoadScene(index);
            }
            else
            {
                StartCoroutine(fader.FadeOutAndSwitch(index));
            }
        }
    }

    
    private int ResolveIndex()
    {
        if (string.IsNullOrEmpty(sceneName)) return sceneBuildIndex;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            if (Path.GetFileNameWithoutExtension(path) == sceneName) return i;
        }

        Debug.LogError("LevelScript: walang scene na '" + sceneName + "' sa Scene List");
        return -1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("Player entered trigger zone. Press E to travel.");
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("Player left trigger zone.");
            playerInRange = false;
        }
    }
}