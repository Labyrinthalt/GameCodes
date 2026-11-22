using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    [Header("NPC")]
    public string npcName = "Bruce";
    public Sprite npcPortrait;

    [Header("Player")]
    public string playerName = "Collin";
    public Sprite playerPortrait;

    [Header("Lines")]
    public DialogueLine[] lines;

    [Header("Typing")]
    public float typingSpeed = 0.03f;
    public float maxCharDelay = 0.08f;
    public bool syncTypingToVoice = true;
}