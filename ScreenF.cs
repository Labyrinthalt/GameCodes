using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        // Fade in from black when the scene starts
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeOutAndSwitch(int sceneIndex)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = timer / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // Load the new scene after the fade is complete
        SceneManager.LoadScene(sceneIndex, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private IEnumerator FadeIn()
    {
        float timer = fadeDuration;
        canvasGroup.alpha = 1f;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            canvasGroup.alpha = timer / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}