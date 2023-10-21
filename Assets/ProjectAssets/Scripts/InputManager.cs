using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public static UnityEvent OnGripRightPressed;
    public static UnityEvent OnGripRightReleased;
    public static UnityEvent<float> OnGripRightUpdated = new UnityEvent<float>();
    public static UnityEvent OnGripLeftPressed;
    public static UnityEvent OnGripLeftReleased;
    public static UnityEvent<float> OnGripLeftUpdated = new UnityEvent<float>();
    public static UnityEvent OnTriggerRightPressed;
    public static UnityEvent OnTriggerRightReleased;
    public static UnityEvent<float> OnTriggerRightUpdated = new UnityEvent<float>();
    public static UnityEvent OnTriggerLeftPressed;
    public static UnityEvent OnTriggerLeftReleased;
    public static UnityEvent<float> OnTriggerLeftUpdated = new UnityEvent<float>();

    [SerializeField] Vector2 _gripThreshold = new Vector2(0.1f,0.9f);
    [SerializeField] Vector2 _triggerThreshold = new Vector2(0.1f, 0.9f);
    
    private XRGenericController inputActions;
    private bool leftGripPressed;
    private bool rightGripPressed;
    private bool leftTriggerPressed;
    private bool rightTriggerPressed;
    private float leftHandGripValue;
    private float leftHandTriggerValue;
    private float rightHandGripValue;
    private float rightHandTriggerValue;
    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else if(Instance!= this)
        {
            Destroy(gameObject);
        }
        inputActions = new XRGenericController();
        inputActions.RightHand.Grip.performed += PressGripRight;
        inputActions.RightHand.Trigger.performed += PressTriggerRight;
        inputActions.LeftHand.Grip.performed += PressGripLeft;
        inputActions.LeftHand.Trigger.performed += PressTriggerLeft;
        inputActions.Enable();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void OnDestroy()
    {
        inputActions.Dispose();
    }

    private void PressTriggerLeft(InputAction.CallbackContext obj)
    {
        leftHandTriggerValue = obj.ReadValue<float>();
        if (leftHandTriggerValue > _triggerThreshold.x && leftHandTriggerValue < _triggerThreshold.y)
        {
            OnTriggerLeftUpdated.Invoke(leftHandTriggerValue);
            leftTriggerPressed = false;
        }
        if (!leftTriggerPressed && leftHandTriggerValue > _triggerThreshold.y)
        {
            OnTriggerLeftPressed.Invoke();
            leftTriggerPressed = true;
        }
        else if (leftTriggerPressed && leftHandTriggerValue < _triggerThreshold.x)
        {
            OnTriggerLeftReleased.Invoke();
            leftTriggerPressed = false;
        }
    }

    private void PressGripLeft(InputAction.CallbackContext obj)
    {
        leftHandGripValue = obj.ReadValue<float>();
        if (leftHandGripValue > _gripThreshold.x && leftHandGripValue < _gripThreshold.y)
        {
            OnGripLeftUpdated.Invoke(leftHandGripValue);
            leftGripPressed = false;
        }
        if (!leftGripPressed && leftHandGripValue > _gripThreshold.y)
        {
            OnGripLeftPressed.Invoke();
            leftGripPressed = true;
        }
        else if (leftGripPressed && leftHandGripValue < _gripThreshold.x)
        {
            OnGripLeftReleased.Invoke();
            leftGripPressed = false;
        }
    }

    private void PressTriggerRight(InputAction.CallbackContext obj)
    {
        rightHandTriggerValue = obj.ReadValue<float>();
        if (rightHandTriggerValue > _triggerThreshold.x && rightHandTriggerValue < _triggerThreshold.y)
        {
            OnTriggerRightUpdated.Invoke(rightHandTriggerValue);
            rightTriggerPressed = false;
        }
        if (!rightTriggerPressed && rightHandTriggerValue > _triggerThreshold.y)
        {
            OnTriggerRightPressed.Invoke();
            rightTriggerPressed = true;
        }
        else if (rightTriggerPressed && rightHandTriggerValue < _triggerThreshold.x)
        {
            OnTriggerRightReleased.Invoke();
            rightTriggerPressed = false;
        }
    }

    private void PressGripRight(InputAction.CallbackContext obj)
    {
        rightHandGripValue = obj.ReadValue<float>();
        if(rightHandGripValue>_gripThreshold.x && rightHandGripValue< _gripThreshold.y)
        {
            OnGripRightUpdated.Invoke(rightHandGripValue);
            rightGripPressed = false;
        }
        if (!rightGripPressed && rightHandGripValue>_gripThreshold.y)
        {
            OnGripRightPressed.Invoke();
            rightGripPressed = true;
        }else if(rightGripPressed && rightHandGripValue < _gripThreshold.x)
        {
            OnGripRightReleased.Invoke();
            rightGripPressed = false;
        }
    }

}
