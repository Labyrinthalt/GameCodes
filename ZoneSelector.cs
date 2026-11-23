using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneSelector : MonoBehaviour
{
    public static bool needsSpawn;

    public void LoadZone(string sceneName)
    {
        Time.timeScale = 1f;
        needsSpawn = true;
        SceneManager.LoadScene(sceneName);
    }
}