using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerHandsManager : MonoBehaviour
{
    public static PlayerHandsManager Instance;
    public UnityEvent<GameObject,HandSides> OnGrab;
    public UnityEvent<GameObject,HandSides> OnRelease;
    public UnityAction OnBothRelease;

    public HandPhysical LeftHand => _leftHand;
    public HandPhysical RightHand => _rightHand;

    [Header("Hands")]
    [SerializeField] HandPhysical _leftHand;
    [SerializeField] HandPhysical _rightHand;

    [Header("Movement")]
    [SerializeField] ActionBasedContinuousMoveProvider _locomotionMove;
    [SerializeField] ActionBasedSnapTurnProvider _locomotionTurn;

    private bool isLeftGrabbing = false;
    private bool isRightGrabbing = false;
    [Header("Grabbed objects")]
    [SerializeField] private GameObject rightGrabbedObject;
    [SerializeField] private GameObject leftGrabbedObject;

    private void Awake()
    {
        Instance = this;
        Connect();
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    private void Connect()
    {
        _leftHand.OnGrab += OnLeftHandGrab;
        _leftHand.OnRelease += OnLeftHandRelease;
        _rightHand.OnGrab += OnRightHandGrab;
        _rightHand.OnRelease += OnRightHandRelease;
    }

    private void Disconnect()
    {
        _leftHand.OnGrab -= OnLeftHandGrab;
        _leftHand.OnRelease -= OnLeftHandRelease;
        _rightHand.OnGrab -= OnRightHandGrab;
        _rightHand.OnRelease -= OnRightHandRelease;
    }
    private void OnLeftHandGrab(GameObject heldObject)
    {
        isLeftGrabbing = true;
        OnGrab?.Invoke(heldObject,HandSides.LeftHand);
        leftGrabbedObject = heldObject;
    }

    private void OnRightHandGrab(GameObject heldObject)
    {
        isRightGrabbing = true;
        OnGrab?.Invoke(heldObject,HandSides.RightHand);
        rightGrabbedObject = heldObject;
    }

    private void OnLeftHandRelease(GameObject heldObject)
    {
        isLeftGrabbing = false;
        OnRelease?.Invoke(heldObject,HandSides.LeftHand);
        if (!isRightGrabbing)
            OnBothRelease?.Invoke();
        leftGrabbedObject = null;
    }

    private void OnRightHandRelease(GameObject heldObject)
    {
        isRightGrabbing = false;
        OnRelease?.Invoke(heldObject,HandSides.RightHand);
        if (!isLeftGrabbing)
            OnBothRelease?.Invoke();
        rightGrabbedObject = null;
    }

    public void HideHand(HandSides side)
    {
        if (side == HandSides.LeftHand)
            _leftHand.HideHand();
        else
            _rightHand.HideHand();
    }

    public void ShowHand(HandSides side)
    {
        if (side == HandSides.LeftHand)
            _leftHand.ShowHand();
        else
            _rightHand.ShowHand();
    }

    public void ReleaseObject(GameObject objectToRelease)
    {
        if (objectToRelease == rightGrabbedObject)
            _rightHand.Release(new UnityEngine.InputSystem.InputAction.CallbackContext());
        if(objectToRelease == leftGrabbedObject)
            _leftHand.Release(new UnityEngine.InputSystem.InputAction.CallbackContext());
    }

    public void SetLocomotion(bool enable)
    {
        _locomotionMove.enabled = enable;
        _locomotionTurn.enabled = enable;
    }
}
