using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabTrocar : TaskData
{
    [SerializeField] GameObject _oldTrocar;
    [SerializeField] GameObject _newTrocar;
    [SerializeField] StabAnimationVRController _newTrocarStab;
    private bool isStabbed = false;
    public override void Init()
    {
        base.Init();
        _oldTrocar.SetActive(false);
        _newTrocar.SetActive(true);
        _newTrocarStab.OnStab += CheckStabAmount;
    }

    private void CheckStabAmount()
    {
        isStabbed = true;
    }
    public override void ExecuteTask()
    {
        if (isStabbed)
        {
            if (_newTrocarStab.StabAmount > 0.7f)
            {
                isStabbed = false;
                _newTrocar.SetActive(false);
                CompleteTask();
            }
        }
    }

    public override void CompleteTask()
    {
        _newTrocarStab.OnStab -= CheckStabAmount;
        base.CompleteTask();
    }
}
