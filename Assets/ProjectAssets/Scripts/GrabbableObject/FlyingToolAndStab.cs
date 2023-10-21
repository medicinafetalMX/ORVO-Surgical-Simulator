using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingToolAndStab : MonoBehaviour
{
    private StabAnimationVRController stab;
    private FlyingTool flyingTool;
    private void Awake()
    {
        flyingTool = GetComponent<FlyingTool>();
        stab = GetComponent<StabAnimationVRController>();
        stab.OnStab += CheckStab;
        stab.OnUnstab += CheckUnstab;
    }
    private void OnDestroy()
    {
        stab.OnStab -= CheckStab;
        stab.OnUnstab -= CheckUnstab;
    }
    private void CheckUnstab()
    {
        flyingTool.IsTargetFollowAuth = true;
    }

    private void CheckStab()
    {
        flyingTool.IsTargetFollowAuth = false;
    }
}
