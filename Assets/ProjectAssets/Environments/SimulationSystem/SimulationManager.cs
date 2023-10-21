using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(SimulationManager))]
public class SimulationManagerEditor : Editor
{
    private SimulationManager simulationManager;
    private Object EventsContainer;
    private void OnEnable()
    {
        simulationManager = target as SimulationManager;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EventsContainer = EditorGUILayout.ObjectField(EventsContainer,typeof(GameObject),true);
        if(GUILayout.Button("Update events"))
        {
            if (EventsContainer != null)
            {
                SimulationEvent[] events = (EventsContainer as GameObject).GetComponentsInChildren<SimulationEvent>();
                simulationManager.SetSimulationEvents(events);
            }
        }
        if(GUILayout.Button("Force next event"))
        {
            simulationManager.ForceNextSimulationEvent();
        }
    }
}
#endif

public class SimulationManager : MonoBehaviour
{
    public UnityAction OnSimulationEnded;

    [SerializeField] SimulationEvent[] _simulationEvents;
    [SerializeField] ConversationController _conversationController;

    private bool isTalking = false;
    private bool isWaitingEndConversation = false;
    private SimulationEvent currentSimulationEvent;
    private Queue<SimulationEvent> simulationEvents = new Queue<SimulationEvent>();
    private void Start()
    {
        _conversationController.OnConversationCompleted += EventConversationEnded;
        StartSimulation();
    }

    private void OnDestroy()
    {
        _conversationController.OnConversationCompleted -= EventConversationEnded;
    }

    private void EventConversationEnded()
    {
        Debug.Log("Conversation ended " + currentSimulationEvent.name);
        isTalking = false;
        if (isWaitingEndConversation)
            NextSimulationEvent();
    }

    private void NextSimulationEvent()
    {
        if (simulationEvents.Count > 0)
        {
            isWaitingEndConversation = false;
            currentSimulationEvent = simulationEvents.Dequeue();
            Debug.Log("Playing simulation event:" + currentSimulationEvent.name);
            if (currentSimulationEvent.Dialogues.Length > 0)
            {
                _conversationController.SetDialogues(currentSimulationEvent.Dialogues);
                _conversationController.StartConversation();
                isTalking = true;
            }
            currentSimulationEvent.Init();
        }
        else
        {
            EndSimulation();
        }
    }

    private void EndSimulation()
    {
        Debug.Log("Simulation ended");
        OnSimulationEnded?.Invoke();
    }

    public void CompleteSimulationEvent(SimulationEvent simulationEvent)
    {
        Debug.Log("<b>Event completed</b>: " + simulationEvent.name);
        if(simulationEvent == currentSimulationEvent)
        {
            if (!isTalking)
                NextSimulationEvent();
            else
                isWaitingEndConversation = true;
        }
        else
        {
            Debug.LogError("Simulation event error: Error on simulation events order: Current "+currentSimulationEvent.name+" received "+simulationEvent.name);
        }
    }

    public void ForceNextSimulationEvent()
    {
        Debug.Log("ForceSimulation:" +currentSimulationEvent.name + " forced to end ");
        if (isTalking)
            _conversationController.ForceEndConversation();
        currentSimulationEvent.ForceEnd();
        NextSimulationEvent();
    }

    public void SetSimulationEvents(SimulationEvent[] events)
    {
        _simulationEvents = events;
    }

    public void StartSimulation()
    {
        foreach (var simulationEvent in _simulationEvents)
            simulationEvents.Enqueue(simulationEvent);
        NextSimulationEvent();
    }
}
