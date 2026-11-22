using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class IntroVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "GameScene";

    private bool loading;

    private void Start()
    {
        if (videoPlayer == null)
            videoPlayer = FindAnyObjectByType<VideoPlayer>();

        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void Update()
    {
        // Pindutin ang Space, Enter o Escape para laktawan ang video
        if (Keyboard.current != null &&
            (Keyboard.current.spaceKey.wasPressedThisFrame ||
             Keyboard.current.enterKey.wasPressedThisFrame ||
             Keyboard.current.escapeKey.wasPressedThisFrame))
        {
            LoadNext();
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        LoadNext();
    }

    private void LoadNext()
    {
        if (loading) return;
        loading = true;
        SceneManager.LoadScene(nextSceneName);
    }
}