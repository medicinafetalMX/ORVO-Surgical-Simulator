using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObjectSnapper : MonoBehaviour
{
    private StabAnimationVRController stab;
    private FlyingTool flyingTool;
    private void Awake()
    {
        stab = GetComponent<StabAnimationVRController>();
        flyingTool = GetComponent<FlyingTool>();
    }
    public void Snap(Transform target)
    {
        if (stab && stab.IsStabbed)
            return;
        if (flyingTool && flyingTool.IsLock)
            return;
        transform.position = target.position;
        transform.rotation = Quaternion.LookRotation(target.forward, target.up);
    }
}
