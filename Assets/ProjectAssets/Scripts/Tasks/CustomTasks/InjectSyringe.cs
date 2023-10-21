using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjectSyringe : TaskData
{
    [SerializeField] Syringe _syringe;
    [SerializeField] FlyingTool _bisturi;
    public override void Init()
    {
        base.Init();
        _syringe.OnInject += CompleteTask;
    }
    public override void CompleteTask()
    {
        _bisturi.Unlock();
        _syringe.OnInject -= CompleteTask;
        base.CompleteTask();
    }
}
