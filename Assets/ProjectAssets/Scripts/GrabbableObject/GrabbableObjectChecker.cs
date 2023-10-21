using UnityEngine;
using UnityEngine.Events;

public class GrabbableObjectChecker : MonoBehaviour
{
    public UnityAction OnGrab;
    public UnityAction OnRelease;

    protected bool isGrabbed = false;
    protected bool isLeftGrabbing = false;
    protected bool isRightGrabbing = false;
    protected bool isLastInteractedObj = false;

    public void AddHandListeners()
    {
        PlayerHandsManager.Instance.OnGrab.AddListener(CheckGrab);
        PlayerHandsManager.Instance.OnRelease.AddListener(CheckRelease);
    }

    protected void OnDestroy()
    {
        PlayerHandsManager.Instance.OnGrab.RemoveListener(CheckGrab);
        PlayerHandsManager.Instance.OnRelease.RemoveListener(CheckRelease);
    }
    public virtual void CheckGrab(GameObject grabbedObject, HandSides side)
    {
        isLastInteractedObj = gameObject == grabbedObject;
        if (isLastInteractedObj)
        {
            if (side == HandSides.LeftHand)
                isLeftGrabbing = true;
            else
                isRightGrabbing = true;
            if (!isGrabbed)
            {
                isGrabbed = true;
                OnGrab?.Invoke();
            }
        }

    }
    public virtual void CheckRelease(GameObject grabbedObject, HandSides side)
    {
        isLastInteractedObj = gameObject == grabbedObject;
        if (isLastInteractedObj)
        {
            if (side == HandSides.LeftHand)
                isLeftGrabbing = false;
            else
                isRightGrabbing = false;
            if (!isLeftGrabbing && !isRightGrabbing)
            {
                isGrabbed = false;
                OnRelease?.Invoke();
            }
        }
    }
}
