using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstabVaina : TaskData
{
    [SerializeField] StabAnimationVRController _vaina;
    public override void Init()
    {
        base.Init();
        _vaina.OnUnstab += CompleteTask;
    }

    public override void CompleteTask()
    {
        _vaina.OnUnstab -= CompleteTask;
        base.CompleteTask();
    }
}
