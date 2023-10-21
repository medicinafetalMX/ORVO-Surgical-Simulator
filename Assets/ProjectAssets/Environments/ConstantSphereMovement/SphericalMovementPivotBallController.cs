using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalMovementPivotBallController : GrabbableObjectChecker
{
    [SerializeField] Transform _sphericalMovement;
    [SerializeField] Transform _fetoscope;
    private Rigidbody body;
    private Vector3 sphericalFetoscopeOffset;
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        sphericalFetoscopeOffset = _sphericalMovement.position - _fetoscope.position;
        AddHandListeners();
    }

    private void Update()
    {
        if (isGrabbed)
        {
            _sphericalMovement.transform.position = body.position;
            _fetoscope.transform.position = _sphericalMovement.transform.position + sphericalFetoscopeOffset;
        }
    }
}
