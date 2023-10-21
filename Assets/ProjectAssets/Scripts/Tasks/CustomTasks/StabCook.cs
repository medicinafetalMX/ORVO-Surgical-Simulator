using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabCook : TaskData
{
    [SerializeField] StabAnimationVRController _cook;
    private bool isStabbed = false;
    private void StartCheck()
    {
        isStabbed = true;
    }

    public override void ExecuteTask()
    {
        if (isStabbed)
        {
            if(_cook.StabAmount >= 0.9f)
            {
                isStabbed = false;
                CompleteTask();
            }
        }
    }
    public override void Init()
    {
        base.Init();
        _cook.OnStab += StartCheck;
    }
    public override void CompleteTask()
    {
        _cook.OnUnstab -= StartCheck;
        base.CompleteTask();
    }
}
