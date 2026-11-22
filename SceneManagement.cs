using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    public string sceneName = "OldGrowth";
    public void Go() { SceneManager.LoadScene(sceneName); }
}