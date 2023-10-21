using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationEvent : MonoBehaviour
{
    public SimulationManager SimulationManager;
    public DialogueData[] Dialogues;
    [SerializeField] SimulationCustomEvent _initSimulationCustomEvent;
    [SerializeField] SimulationCustomEvent _waitSimulationCustomEvent;
    [SerializeField] SimulationCustomEvent _endSimulationCustomEvent;

    private void Start()
    {
        if (_waitSimulationCustomEvent)
        {
            _waitSimulationCustomEvent.OnEventComplete += End;
            _waitSimulationCustomEvent.Reference = this;
        }
    }

    public void Init()
    {
        if(_initSimulationCustomEvent)
            _initSimulationCustomEvent.Init();
        if(_waitSimulationCustomEvent)
            _waitSimulationCustomEvent.Init();
    }

    public void ForceEnd()
    {
        if (_initSimulationCustomEvent)
            _initSimulationCustomEvent.ForceEnd();
        if (_waitSimulationCustomEvent)
            _waitSimulationCustomEvent.ForceEnd();
        if (_endSimulationCustomEvent)
            _endSimulationCustomEvent.ForceEnd();
    }

    public void End()
    {
        if(_waitSimulationCustomEvent)
            _waitSimulationCustomEvent.OnEventComplete -= End;
        if (_endSimulationCustomEvent)
            _endSimulationCustomEvent.Init();
        SimulationManager.CompleteSimulationEvent(this);
    }
}
