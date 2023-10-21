using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandAnimator : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] string gripAnimParameter = "Grip";
    [SerializeField] string triggerAnimParameter = "Trigger";
    [SerializeField] ActionBasedController controller;

    private float triggerTarget;
    private float gripTarget;

    private void Update()
    {
        gripTarget = controller.selectActionValue.action.ReadValue<float>();
        triggerTarget = controller.activateActionValue.action.ReadValue<float>();
        gripTarget = Mathf.Clamp(gripTarget, 0, 1);
        triggerTarget = Mathf.Clamp(triggerTarget, 0, 1);
        AnimateHand();
    }

    private void AnimateHand()
    {
        _animator.SetFloat(gripAnimParameter, gripTarget);
        _animator.SetFloat(triggerAnimParameter, triggerTarget);
    }
}
