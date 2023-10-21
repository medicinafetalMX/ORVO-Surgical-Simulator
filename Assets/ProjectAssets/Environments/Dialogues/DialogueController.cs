using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class DialogueController : MonoBehaviour
{
    public UnityAction OnDialogueCompleted;

    [SerializeField] TextMeshProUGUI _dialogueText;
    [SerializeField] AudioSource _dialogueAudio;
    [SerializeField] float _speed;

    private string dialogue;
    private IEnumerator ReadTask;

    private IEnumerator ReadDialogue()
    {
        int dialogueCount = dialogue.Length;
        int letterCounter = 0;
        while (letterCounter < dialogueCount)
        {
            yield return new WaitForSeconds(_speed);
            _dialogueText.text += dialogue[letterCounter];
            letterCounter++;
        }
        while (_dialogueAudio.isPlaying)
            yield return null;
        OnDialogueCompleted?.Invoke();
    }

    public void ForceStop()
    {
        if (ReadTask != null)
            StopCoroutine(ReadTask);
        _dialogueText.text = dialogue;
        _dialogueAudio.Stop();
    }

    public void StartDialogue(string dialogue, AudioClip dialogueAudio = null)
    {
        this.dialogue = dialogue;
        _dialogueText.text = "";
        _dialogueAudio.Stop();
        _dialogueAudio.clip = dialogueAudio;
        if (dialogueAudio)
            _dialogueAudio.Play();
        ReadTask = ReadDialogue();
        StartCoroutine(ReadTask);
    }
}
