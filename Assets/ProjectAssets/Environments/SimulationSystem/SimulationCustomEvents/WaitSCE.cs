using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitSCE : SimulationCustomEvent
{
    [SerializeField] float _waitTime;
    private bool isWaiting = false;
    private float timer;

    private void Update()
    {
        if(isWaiting)
        {
            timer += Time.deltaTime;
            if(timer>= _waitTime)
            {
                timer = 0;
                isWaiting = false;
                End();
            }
        }
    }
    public override void End()
    {
        Debug.Log("WaitSCE: Waited for " + _waitTime + " seconds");
        OnEventComplete?.Invoke();
    }

    public override void ForceEnd()
    {
        isWaiting = false;
        timer = 0;
    }

    public override void Init()
    {
        isWaiting = true;
    }
}
