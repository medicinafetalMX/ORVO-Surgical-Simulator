using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstabCook : TaskData
{
    [SerializeField] StabAnimationVRController _cookStab;
    [SerializeField] Cook _cook;
    public override void Init()
    {
        base.Init();
        _cook.EnablePlasticTrocar();
        _cookStab.OnUnstab += CompleteTask;
    }

    public override void CompleteTask()
    {
        _cookStab.OnUnstab -= CompleteTask;
        base.CompleteTask();
    }
}
