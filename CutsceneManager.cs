using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject cutsceneCanvas;
    [SerializeField] private VideoClip[] clips;
    [SerializeField] private string nextSceneName;
    [SerializeField] private GameObject[] disableDuringCutscene;
    [SerializeField] private GameObject bossToWatch;
    [SerializeField] private float delayAfterBossDeath = 2f;

    private int currentIndex;
    private bool playing;
    private bool bossWasAlive;
    private bool triggered;

    void Start()
    {
        cutsceneCanvas.SetActive(false);
        videoPlayer.Stop();
        bossWasAlive = bossToWatch != null;
    }

    public void PlayCutscene()
    {
        if (playing || clips == null || clips.Length == 0) return;

        playing = true;
        currentIndex = 0;

        foreach (var go in disableDuringCutscene)
            if (go) go.SetActive(false);

        cutsceneCanvas.SetActive(true);

        videoPlayer.loopPointReached -= OnVideoFinished;
        videoPlayer.loopPointReached += OnVideoFinished;
        PlayCurrent();
    }

    void PlayCurrent()
    {
        videoPlayer.Stop();
        videoPlayer.clip = clips[currentIndex];
        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        currentIndex++;
        if (currentIndex < clips.Length) PlayCurrent();
        else EndCutscene();
    }

    void EndCutscene()
    {
        videoPlayer.Stop();
        videoPlayer.loopPointReached -= OnVideoFinished;
        cutsceneCanvas.SetActive(false);
        playing = false;

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            DestroyPersistentObjects();
            SceneManager.LoadScene(nextSceneName);
        }
    }


    void DestroyPersistentObjects()
    {
        GameObject temp = new GameObject("temp");
        DontDestroyOnLoad(temp);
        Scene dontDestroyScene = temp.scene;
        foreach (var root in dontDestroyScene.GetRootGameObjects())
        {
            if (root != temp && root != gameObject)
                Destroy(root);
        }
        Destroy(temp);
    }

    void Update()
    {

        if (!triggered && bossWasAlive)
        {
            if (bossToWatch == null || !bossToWatch.activeInHierarchy)
            {
                triggered = true;
                Invoke(nameof(PlayCutscene), delayAfterBossDeath);
            }
        }

        // TEST ONLY: tanggalin pagkatapos KO  mag-test AHUHHH
        if (!playing && Input.GetKeyDown(KeyCode.F9)) PlayCutscene();


        if (playing && Input.GetKeyDown(KeyCode.Escape))
        {
            videoPlayer.Stop();
            OnVideoFinished(videoPlayer);
        }
    }
}