using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;
    public GameObject interactPrompt;

    [Header("Audio")]
    public AudioSource voiceSource;

    [Header("Input")]
    public bool useOwnInput = true;

    int dialogueIndex;
    bool isTyping, isDialogueActive, playerInRange;
    Coroutine typingRoutine;

    // Proteksyon laban sa agad-restart ng dialogue
    float lastEndTime = -10f;
    int lastInteractFrame = -1;

    void Start()
    {
        if (dialoguePanel) dialoguePanel.SetActive(false);
        if (interactPrompt) interactPrompt.SetActive(false);
    }

    void OnDisable()
    {
        if (isDialogueActive)
        {
            isDialogueActive = false;
            PauseController.SetPause(false);
        }
    }

    void Update()
    {
        if (!useOwnInput) return;

        bool ePressed = false;
        bool escPressed = false;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame) ePressed = true;
            if (Keyboard.current.escapeKey.wasPressedThisFrame) escPressed = true;
        }

#if ENABLE_LEGACY_INPUT_MANAGER
        if (Input.GetKeyDown(KeyCode.E)) ePressed = true;
        if (Input.GetKeyDown(KeyCode.Escape)) escPressed = true;
#endif

        if (escPressed && isDialogueActive)
        {
            EndDialogue();
            return;
        }

        if (ePressed && (playerInRange || isDialogueActive))
            Interact();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
        if (interactPrompt && !isDialogueActive) interactPrompt.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        if (interactPrompt) interactPrompt.SetActive(false);
    }

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null) { Debug.LogError("NPC: dialogueData is NULL"); return; }
        if (dialogueData.lines == null || dialogueData.lines.Length == 0) { Debug.LogError("NPC: walang lines"); return; }
        if (PauseController.IsGamePaused && !isDialogueActive) { Debug.LogWarning("NPC: naka-pause ang game"); return; }

        // Isang Interact() lang bawat frame (iwas double-call)
        if (lastInteractFrame == Time.frameCount) return;
        lastInteractFrame = Time.frameCount;

        // Bawal mag-start agad pagkatapos mag-end
        if (!isDialogueActive && Time.unscaledTime - lastEndTime < 0.3f) return;

        if (isDialogueActive) NextLine();
        else StartDialogue();
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        if (interactPrompt) interactPrompt.SetActive(false);
        dialoguePanel.SetActive(true);
        PauseController.SetPause(true);

        ShowLine();
    }

    void ShowLine()
    {
        DialogueLine line = dialogueData.lines[dialogueIndex];
        bool isPlayer = line.speaker == Speaker.Collin;

        nameText.SetText(isPlayer ? dialogueData.playerName : dialogueData.npcName);

        Sprite portrait = isPlayer ? dialogueData.playerPortrait : dialogueData.npcPortrait;
        if (portraitImage)
        {
            portraitImage.sprite = portrait;
            portraitImage.gameObject.SetActive(portrait != null);
        }

        if (voiceSource)
        {
            voiceSource.Stop();
            if (line.voice != null)
            {
                voiceSource.clip = line.voice;
                voiceSource.Play();
            }
        }

        if (typingRoutine != null) StopCoroutine(typingRoutine);
        typingRoutine = StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(DialogueLine line)
    {
        isTyping = true;
        dialogueText.SetText("");

        float delay = dialogueData.typingSpeed;
        if (line.voice != null && dialogueData.syncTypingToVoice && line.text.Length > 0)
            delay = Mathf.Min(line.voice.length / line.text.Length, dialogueData.maxCharDelay);

        foreach (char letter in line.text)
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(delay);
        }

        isTyping = false;
    }

    void NextLine()
    {
        if (isTyping)
        {
            if (typingRoutine != null) StopCoroutine(typingRoutine);
            dialogueText.SetText(dialogueData.lines[dialogueIndex].text);
            isTyping = false;
        }
        else if (dialogueIndex + 1 < dialogueData.lines.Length)
        {
            dialogueIndex++;
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        if (voiceSource) voiceSource.Stop();

        isTyping = false;
        isDialogueActive = false;
        lastEndTime = Time.unscaledTime;
        dialoguePanel.SetActive(false);
        PauseController.SetPause(false);

        if (interactPrompt && playerInRange) interactPrompt.SetActive(true);
    }
}