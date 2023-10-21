using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BisturiCut : TaskData
{
    [SerializeField] BisturiLeverController _bisturiLevelController;
    [SerializeField] FlyingTool _cook;
    public override void Init()
    {
        base.Init();
        _bisturiLevelController.OnCutFinished += CompleteTask;
    }

    public override void CompleteTask()
    {
        _bisturiLevelController.OnCutFinished -= CompleteTask;
        _cook.Unlock();
        base.CompleteTask();
    }
}
