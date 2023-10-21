using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticGrabbableObject : MonoBehaviour
{
    private Rigidbody body;
    private bool isLeftGrabbing = false;
    private bool isRightGrabbing = false;
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        PlayerHandsManager.Instance.OnGrab.AddListener(CheckGrab);
        PlayerHandsManager.Instance.OnRelease.AddListener(CheckRelease);
        body.useGravity = false;
        MakeKinematic();
    }

    private void OnDestroy()
    {
        PlayerHandsManager.Instance.OnGrab.RemoveListener(CheckGrab);
        PlayerHandsManager.Instance.OnRelease.RemoveListener(CheckRelease);
    }

    private void CheckRelease(GameObject grabbedObject, HandSides side)
    {
        if (grabbedObject == gameObject)
        {
            Debug.Log("Remove permission");
            if (side == HandSides.LeftHand)
                isLeftGrabbing = false;
            else
                isRightGrabbing = false;
            if (!isLeftGrabbing && !isRightGrabbing)
            {
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
                MakeKinematic();
            }
        }
    }

    private void CheckGrab(GameObject grabbedObject, HandSides side)
    {
        if (grabbedObject == gameObject)
        {
            Debug.Log("Grand permission");
            if (side == HandSides.LeftHand)
                isLeftGrabbing = true;
            else
                isRightGrabbing = true;
            RemoveKinematic();
        }
    }

    private void MakeKinematic()
    {
        body.isKinematic = true;
    }

    private void RemoveKinematic()
    {
        body.isKinematic = false;
    }
}
