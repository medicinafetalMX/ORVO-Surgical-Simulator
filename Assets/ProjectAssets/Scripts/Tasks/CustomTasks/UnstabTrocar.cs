using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstabTrocar : TaskData
{
    [SerializeField] StabAnimationVRController _trocar;
    [SerializeField] FlyingTool _fetoscopeStraight;
    [SerializeField] FlyingTool _fetoscopeCurved;
    public override void Init()
    {
        _trocar.OnUnstab += CompleteTask;
        base.Init();
    }
    public override void CompleteTask()
    {
        _trocar.OnUnstab -= CompleteTask;
        _fetoscopeStraight.Unlock();
        _fetoscopeCurved.Unlock();
        base.CompleteTask();
    }
}
