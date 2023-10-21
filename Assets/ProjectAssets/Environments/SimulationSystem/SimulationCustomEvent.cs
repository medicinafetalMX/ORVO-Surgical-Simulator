using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SimulationEvent))]
public abstract class SimulationCustomEvent : MonoBehaviour
{
    public UnityAction OnEventComplete;
    public SimulationEvent Reference;
    public abstract void Init();
    public abstract void ForceEnd();
    public abstract void End();
}
