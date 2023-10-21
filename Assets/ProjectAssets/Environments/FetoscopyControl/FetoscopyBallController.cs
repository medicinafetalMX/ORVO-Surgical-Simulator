using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetoscopyBallController : GrabbableObjectChecker
{
    [SerializeField] FetoscopyController _fetoscopyController;
    [SerializeField] float _speed;
    private Rigidbody body;
    private Transform target;
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, _speed * Time.deltaTime);
        }
    }

    private void Start()
    {
        AddHandListeners();
    }

    public override void CheckGrab(GameObject grabbedObject, HandSides side)
    {
        base.CheckGrab(grabbedObject, side);
        if (isLastInteractedObj)
        {
            _fetoscopyController.GrandControlPermissions(side);
            if(side == HandSides.LeftHand)
                target = PlayerHandsManager.Instance.LeftHand.Controller.transform;
            else
                target = PlayerHandsManager.Instance.RightHand.Controller.transform;
        }
    }

    public override void CheckRelease(GameObject grabbedObject, HandSides side)
    {
        base.CheckRelease(grabbedObject, side);
        if (isLastInteractedObj)
        {
            if (!isGrabbed)
            {
                target = null;
                _fetoscopyController.RemoveControlPermissions(side);
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
        }
    }
}
