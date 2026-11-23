using UnityEngine;

public enum Speaker { Collin, NPC }

[System.Serializable]
public class DialogueLine
{
    public Speaker speaker;
    [TextArea(2, 8)] public string text;
    public AudioClip voice;
}