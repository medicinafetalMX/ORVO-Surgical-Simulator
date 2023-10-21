using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFlyingToolPivotSCE : SimulationCustomEvent
{
    [SerializeField] FlyingTool _flytingTool;
    [SerializeField] Transform _target;

    public override void End()
    {
        OnEventComplete?.Invoke();
    }

    public override void ForceEnd()
    {
    }

    public override void Init()
    {
        _flytingTool.SetFollowPose(_target);
    }
}
