using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance { get; private set; }

    private AudioSource audioSource;
    private SoundEffectLibrary soundEffectLibrary;

    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        audioSource = GetComponent<AudioSource>();
        soundEffectLibrary = GetComponent<SoundEffectLibrary>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    private void OnDestroy()
    {
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }

    public static void Play(string soundName)
    {
        if (Instance == null || Instance.audioSource == null || Instance.soundEffectLibrary == null)
            return;

        AudioClip audioClip = Instance.soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            Instance.audioSource.PlayOneShot(audioClip);
        }
    }

    public static void SetVolume(float volume)
    {
        if (Instance == null || Instance.audioSource == null)
            return;

        Instance.audioSource.volume = Mathf.Clamp01(volume);
    }

    private void OnSliderValueChanged(float value)
    {
        SetVolume(value);
    }
}