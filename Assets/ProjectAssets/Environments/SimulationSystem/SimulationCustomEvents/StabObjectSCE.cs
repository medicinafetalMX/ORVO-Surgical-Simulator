using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabObjectSCE : SimulationCustomEvent
{
    [SerializeField] StabAnimationVRController _stabObject;
    [SerializeField] Transform _startStabPosition;
    [SerializeField][Range(0,1)] float _stabAmountTarget;

    private bool isTargetAmountGreater;
    private bool isCheckingStab;

    private void Update()
    {
        if (isCheckingStab)
        {
            if (isTargetAmountGreater)
            {
                if (_stabObject.StabAmount >= _stabAmountTarget)
                    End();
            }
            else
            {
                if (_stabObject.StabAmount <= _stabAmountTarget)
                    End();
            }
        }
    }
    private void CalculateIsTargetAmountGreater()
    {
        isTargetAmountGreater = _stabAmountTarget > _stabObject.StabAmount;
    }
    public override void End()
    {
        isCheckingStab = false;
        OnEventComplete?.Invoke();
    }

    public override void ForceEnd()
    {
        isCheckingStab = false;
    }

    public override void Init()
    {
        _stabObject.transform.position = _startStabPosition.position;
        _stabObject.transform.rotation = _startStabPosition.rotation;
        CalculateIsTargetAmountGreater();
        isCheckingStab = true;
    }
}
