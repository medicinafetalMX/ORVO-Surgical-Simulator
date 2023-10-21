using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickObjectSCE : SimulationCustomEvent
{
    [SerializeField] GameObject _objectToPick;

    private void OnDrawGizmosSelected()
    {
        if(_objectToPick)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _objectToPick.transform.position);
        }
    }
    private void CheckGrab(GameObject grabbedObject,HandSides side)
    {
        if(grabbedObject == _objectToPick)
        {
            End();
        }
    }

    public override void End()
    {
        Debug.Log("PickObjectSCE: Picked " + _objectToPick.name);
        PlayerHandsManager.Instance.OnGrab.RemoveListener(CheckGrab);
        OnEventComplete?.Invoke();
    }

    public override void ForceEnd()
    {
        PlayerHandsManager.Instance.OnGrab.RemoveListener(CheckGrab);
    }

    public override void Init()
    {
        PlayerHandsManager.Instance.OnGrab.AddListener(CheckGrab);
    }
}
