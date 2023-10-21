using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
[RequireComponent(typeof(StabAnimationVRController))]
public class Syringe : GrabbableObjectChecker
{
    public UnityAction OnInject;

    [SerializeField] InputActionReference _leftButton;
    [SerializeField] InputActionReference _rightButton;
    [SerializeField] Animator _animator;

    private bool isInjectAllowed = false;
    private StabAnimationVRController stab;

    private void Awake()
    {
        stab = GetComponent<StabAnimationVRController>();
    }

    private void Start()
    {
        AddHandListeners();
        stab.OnStab += AllowInject;
        stab.OnUnstab += DenyInject;
    }

    private void FillSyringe()
    {
        _animator.SetBool("Inject", false);
    }

    private void Inject(InputAction.CallbackContext obj)
    {
        if (isInjectAllowed)
        {
            OnInject?.Invoke();
            _animator.SetBool("Inject",true);
            Invoke(nameof(FillSyringe), 5);
        }
    }

    private void DenyInject()
    {
        isInjectAllowed = false;
    }

    private void AllowInject()
    {
        isInjectAllowed = true;
    }

    private void OnDestroy()
    {
        stab.OnStab -= AllowInject;
        stab.OnUnstab -= DenyInject;
    }

    public override void CheckGrab(GameObject grabbedObject, HandSides side)
    {
        base.CheckGrab(grabbedObject, side);
        if (isLastInteractedObj)
        {
            if(side == HandSides.LeftHand)
                _leftButton.action.performed += Inject;
            else
                _rightButton.action.performed += Inject;
        }
    }

    public override void CheckRelease(GameObject grabbedObject, HandSides side)
    {
        base.CheckRelease(grabbedObject, side);
        if (isLastInteractedObj)
        {
            if (side == HandSides.LeftHand)
                _leftButton.action.performed -= Inject;
            else
                _rightButton.action.performed -= Inject;
        }
    }
}
