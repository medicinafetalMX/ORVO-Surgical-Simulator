using UnityEngine;

[System.Serializable]
public class DialogueData
{
    [TextArea(0,5)]public string dialogue;
    public AudioClip dialogueSound;
}