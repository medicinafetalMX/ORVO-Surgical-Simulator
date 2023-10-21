using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSurgeryArea : TaskData
{
    [SerializeField] SurgeryPlaceSelector _surgeryPlaceSelector;
    [SerializeField] FlyingTool _syringeTool;
    public override void Init()
    {
        base.Init();
        _surgeryPlaceSelector.OnSurgeryPlaceSelected += CompleteTask;
    }
    public override void CompleteTask()
    {
        _surgeryPlaceSelector.OnSurgeryPlaceSelected -= CompleteTask;
        _syringeTool.Unlock();
        base.CompleteTask();
    }
}
