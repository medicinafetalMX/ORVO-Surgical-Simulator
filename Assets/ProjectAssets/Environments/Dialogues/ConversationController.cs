using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(ConversationController))]
public class ConversationControllerEditor : Editor
{
    private ConversationController controller;
    private void OnEnable()
    {
        controller = target as ConversationController;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Start conversation"))
            controller.StartConversation();
    }
}
#endif

public class ConversationController : MonoBehaviour
{
    public UnityAction OnConversationCompleted;

    [SerializeField] DialogueData[] _dialogues;
    [SerializeField] DialogueController _dialogueController;
    [SerializeField] float _timeBtwDialogues = 1;

    private Queue<DialogueData> dialogues = new Queue<DialogueData>();

    private void Start()
    {
        foreach (var dialogue in _dialogues)
            dialogues.Enqueue(dialogue);
        _dialogueController.OnDialogueCompleted += NextDialogue;
    }

    private void NextDialogue()
    {
        if (dialogues.Count > 0)
        {
            DialogueData dialogue = dialogues.Dequeue();
            StartCoroutine(LaunchNextDialogue(dialogue));
        }
        else
        {
            OnConversationCompleted?.Invoke();
        }
    }

    private IEnumerator LaunchNextDialogue(DialogueData dialogue)
    {
        yield return new WaitForSeconds(_timeBtwDialogues);
        _dialogueController.StartDialogue(dialogue.dialogue, dialogue.dialogueSound);
    }

    public void ForceEndConversation()
    {
        _dialogueController.ForceStop();
        dialogues.Clear();
    }

    public void StartConversation()
    {
        NextDialogue();
    }

    public void SetDialogues(DialogueData[] newDialogues)
    {
        _dialogues = newDialogues;
        dialogues = new Queue<DialogueData>();
        foreach (var dialogue in _dialogues)
            dialogues.Enqueue(dialogue);
    }
} 
