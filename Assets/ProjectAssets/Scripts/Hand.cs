using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    [SerializeField] float _animSpeed;
    [SerializeField] GameObject _followObject;
    [SerializeField] float _followSpeed = 30f;
    [SerializeField] float _rotateSpeed = 100f;
    [SerializeField] Vector3 _positionOffset;
    [SerializeField] Vector3 _rotationOffset;
    private Animator animator;
    private float gripTarget;
    private float triggerTarget;
    private string gripAnimParameter = "Grip";
    private string triggerAnimParameter = "Trigger";
    private Transform followTarget;
    private Rigidbody body;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        followTarget = _followObject.transform;
        body = GetComponent<Rigidbody>();
        body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.mass = 20;

        //Teleport hands
        body.position = followTarget.position;
        body.rotation = followTarget.rotation;
    }

    private void Update()
    {
        AnimateHand();
        PhysicsMove();
    }

    private void AnimateHand()
    {
        animator.SetFloat(gripAnimParameter, gripTarget);
        animator.SetFloat(triggerAnimParameter, triggerTarget);
    }
    private void PhysicsMove()
    {
        //Position
        var offsetPos = followTarget.position + _positionOffset;
        var distance = Vector3.Distance(offsetPos, transform.position);
        body.velocity = (followTarget.position - transform.position).normalized * (_followSpeed * distance);

        //Rotation
        var offsetRotation = followTarget.rotation * Quaternion.Euler(_rotationOffset);
        var quaternion = offsetRotation * Quaternion.Inverse(body.rotation);
        quaternion.ToAngleAxis(out float angle, out Vector3 axis);
        body.angularVelocity = angle * axis * Mathf.Deg2Rad * _rotateSpeed;
    }
    internal void SetTrigger(float v)
    {
        triggerTarget = v;
    }

    internal void SetGrip(float v)
    {
        gripTarget = v;
    }
}
