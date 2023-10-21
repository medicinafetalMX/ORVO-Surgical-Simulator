using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabVaina : TaskData
{
    [SerializeField] StabAnimationVRController _vaina;
    [SerializeField] FlyingTool _fetoscopeStraight;
    [SerializeField] FlyingTool _fetoscopeCurved;

    public override void Init()
    {
        base.Init();
        _vaina.OnStab += CompleteTask;
    }
    public override void CompleteTask()
    {
        _vaina.OnStab -= CompleteTask;
        _fetoscopeCurved.Unlock();
        _fetoscopeStraight.Unlock();
        base.CompleteTask();
    }
}
