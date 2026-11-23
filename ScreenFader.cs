using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Ensure screen starts black when loading a new scene, then fades in
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeOutAndSwitch(int sceneIndex)
    {
        float timer = 0f;
        canvasGroup.blocksRaycasts = true; // Prevent player clicks during fade

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // Load the target scene index
        SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator FadeIn()
    {
        float timer = fadeDuration;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false; // Allow player interaction again
    }
}