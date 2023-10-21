using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAndPlaceInstruction : Instruction
{
    [Header("Components")]
    [SerializeField] GameObject _indicator;
    [SerializeField] GameObject _comparator;
    [SerializeField] GameObject _objectToGrab;

    [Header("Distance")]
    [SerializeField] float _minDistance;
    [SerializeField] float _minAngle;

    private bool isObjectGrabbed = false;

    private void Start()
    {
        Connect();
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    private void Update()
    {
        if (isObjectGrabbed)
            CheckObjectDistance();
    }

    private void CheckGrab(GameObject grabbedObject, HandSides side)
    {
        if(grabbedObject == _objectToGrab)
        {
            isObjectGrabbed = true;
            ShowHand(side);
        }
    }

    private void CheckObjectDistance()
    {
        float distance = Vector3.Distance(_indicator.transform.position, _comparator.transform.position);
        float angle = Quaternion.Angle(_indicator.transform.rotation, _comparator.transform.rotation);
        if (distance <= _minDistance && angle <= _minAngle)
        {
            Debug.Log("Object placed");
            isObjectGrabbed = false;
            _indicator.SetActive(false);
            Disconnect();
        }
    }

    private void ShowHand(HandSides side)
    {
        GrabbedObject indicatorGrabbedObject = _indicator.GetComponent<GrabbedObject>();
        indicatorGrabbedObject.Grab(side);
        _indicator.SetActive(true);
    }

    private void HideHands(GameObject releasedObject,HandSides side)
    {
        if (releasedObject == _objectToGrab)
        {
            isObjectGrabbed = false;
            _indicator.SetActive(false);
        }
    }

    public override void Connect()
    {
        PlayerHandsManager.Instance.OnGrab.AddListener(CheckGrab);
        PlayerHandsManager.Instance.OnRelease.AddListener(HideHands);
    }

    public override void Disconnect()
    {
        PlayerHandsManager.Instance.OnGrab.RemoveListener(CheckGrab);
        PlayerHandsManager.Instance.OnRelease.RemoveListener(HideHands);
        if (_continueInstruction)
            _continueInstruction.Connect();
    }
}
