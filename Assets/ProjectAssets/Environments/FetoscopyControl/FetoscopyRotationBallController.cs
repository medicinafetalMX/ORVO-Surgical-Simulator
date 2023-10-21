using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetoscopyRotationBallController : GrabbableObjectChecker
{
    public float Roll;
    [SerializeField] Transform _target;
    [SerializeField] Transform _origin;
    [SerializeField] Transform _upOrigin;
    private float currentRoll;
    private HandSides currentSide;
    private Vector3 currentFwd;
    private void Start()
    {
        AddHandListeners();
    }
    private void Update()
    {
        if (isGrabbed)
        {
            ExtractRoll();
            _target.transform.rotation = Quaternion.LookRotation(Quaternion.AngleAxis(currentRoll + Roll, _upOrigin.up) * _upOrigin.forward, _upOrigin.up);
            //_target.transform.localRotation = Quaternion.Euler(Vector3.up * (Roll + currentRoll));
            transform.localRotation = _target.transform.localRotation;
        }
    }

    private void ExtractRoll()
    {
        Quaternion rot = _origin.transform.localRotation;
        Roll = Mathf.Atan2(2 * rot.w * rot.y - 2 * rot.x * rot.z, 1 - 2 * rot.y * rot.y - 2 * rot.z * rot.z) * Mathf.Rad2Deg;
    }
    public override void CheckGrab(GameObject grabbedObject, HandSides side)
    {
        base.CheckGrab(grabbedObject, side);
        if (isLastInteractedObj)
        {
            currentRoll = _target.localEulerAngles.y;
            currentFwd = _target.forward;
            currentSide = side;
            _origin = side == HandSides.LeftHand ? PlayerHandsManager.Instance.LeftHand.Controller.transform : PlayerHandsManager.Instance.RightHand.Controller.transform;
            PlayerHandsManager.Instance.HideHand(side);
        }
    }
    public override void CheckRelease(GameObject grabbedObject, HandSides side)
    {
        base.CheckRelease(grabbedObject, side);
        if (isLastInteractedObj)
        {
            transform.localRotation = Quaternion.identity;
            PlayerHandsManager.Instance.ShowHand(currentSide);
        }
    }
}
