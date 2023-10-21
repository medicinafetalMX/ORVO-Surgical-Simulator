using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
public class HandPhysical : MonoBehaviour
{
    public UnityAction<GameObject> OnGrab;
    public UnityAction<GameObject> OnRelease;

    public ActionBasedController Controller => _controller;

    [SerializeField] GameObject _handModel;
    [SerializeField] HandSides _side;
    [Header("Hands follow")]
    [SerializeField] ActionBasedController _controller;
    [SerializeField] Transform _handSnapper;
    [SerializeField] float _followSpeed = 30f;
    [SerializeField] float _rotateSpeed = 100f;
    [SerializeField] float _toleranceDistance = 0.2f;
    [Header("Hands offset")]
    [SerializeField] Vector3 _positionOffset;
    [SerializeField] Vector3 _rotationOffset;
    [Header("Grab settings")]
    [SerializeField] Transform _palm;
    [SerializeField] float _reachDistance = 0.1f;
    [SerializeField] float _jointDistance = 0.5f;
    [SerializeField] LayerMask _grabbableLayer;

    private Transform followTarget;
    private Rigidbody body;
    private bool isGrabbing;
    private GameObject heldObject;
    private Transform grabPoint;
    private FixedJoint handJoint, objectJoint;
    private HandReplacer handReplacer;
    private GameObject replaceHand;

    private void Start()
    {
        //Physics movement
        followTarget = _controller.transform;
        body = GetComponent<Rigidbody>();
        body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.mass = 20;
        body.maxAngularVelocity = 20;

        //Input setup
        _controller.selectAction.action.started += Grab;
        _controller.selectAction.action.canceled += Release;

        //Teleport hands
        body.position = followTarget.position;
        body.rotation = followTarget.rotation;
    }
    
    private void Update()
    {
        PhysicsMove();
        FollowTolerance();
    }

    private void FollowTolerance()
    {
        if(Vector3.Distance(transform.position,followTarget.position) > _toleranceDistance)
        {
            transform.position = followTarget.position;
        }
    }

    private void PhysicsMove()
    {
        //Position
        var offsetPos = followTarget.TransformPoint(_positionOffset);
        var distance = Vector3.Distance(offsetPos, transform.position);
        body.velocity = (followTarget.position - transform.position).normalized * (_followSpeed * distance);
        
        //Rotation
        var offsetRotation = followTarget.rotation * Quaternion.Euler(_rotationOffset);
        var quaternion = offsetRotation * Quaternion.Inverse(body.rotation);
        quaternion.ToAngleAxis(out float angle, out Vector3 axis);
        body.angularVelocity = angle * axis * Mathf.Deg2Rad * _rotateSpeed;
    }
    private void Grab(InputAction.CallbackContext obj)
    {
        if (isGrabbing || heldObject)
            return;

        Collider[] colliders = Physics.OverlapSphere(_palm.position, _reachDistance, _grabbableLayer);
        if (colliders.Length > 0)
        {
            Collider firstCollider = null;
            float distance = _reachDistance;
            for (int i = 0; i < colliders.Length; i++)
            {
                float currentDistance = Vector3.Distance(_palm.position, colliders[i].ClosestPoint(_palm.position));
                if (currentDistance < distance)
                {
                    firstCollider = colliders[i];
                    distance = currentDistance;
                }
            }
            GameObject firstObject = firstCollider.gameObject;
            Rigidbody objectBody = firstObject.GetComponent<Rigidbody>();
            if (objectBody)
            {
                heldObject = firstObject;
            }
            else
            {
                objectBody = firstObject.GetComponentInParent<Rigidbody>();
                if (objectBody)
                    heldObject = objectBody.gameObject;
                else
                    return;
            }
            
            handReplacer = firstObject.GetComponent<HandReplacer>();
            if (handReplacer)
            {
                ReplaceHand();
            }
            else
            {
                StartCoroutine(PhysicsGrab(firstCollider,objectBody));
            }
            OnGrab?.Invoke(heldObject);
        }
    }

    private void ReplaceHand()
    {
        replaceHand = handReplacer.Grab(_side,transform);
        _handModel.SetActive(false);
    }

    public void Release(InputAction.CallbackContext obj)
    {
        if (handJoint)
            Destroy(handJoint);
        if (objectJoint)
            Destroy(objectJoint);
        if (grabPoint)
            Destroy(grabPoint.gameObject);

        if (heldObject)
        {
            //Check if its a replacer
            if (handReplacer)
            {
                handReplacer.Release();
                _handModel.SetActive(true);
                replaceHand = null;
                handReplacer = null;
            }

            Rigidbody targetBody = heldObject.GetComponent<Rigidbody>();
            if (targetBody)
            {
                targetBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                targetBody.interpolation = RigidbodyInterpolation.None;
            }
            OnRelease?.Invoke(heldObject);
        }

        heldObject = null;
        isGrabbing = false;
        followTarget = _controller.gameObject.transform;
    }

    private IEnumerator PhysicsGrab(Collider collider, Rigidbody targetBody)
    {
        isGrabbing = true;

        //Move target
        GrabbableObjectSnapper snapper = targetBody.GetComponent<GrabbableObjectSnapper>();
        if (snapper)
        {
            snapper.Snap(_handSnapper);
        }

        //Create grab point
        grabPoint = new GameObject().transform;
        grabPoint.position = collider.ClosestPoint(_palm.position);
        grabPoint.parent = heldObject.transform;
        
        //Move hand to grab point
        followTarget = grabPoint;
        
        //Wait for hand to reach grab point
        while(grabPoint && Vector3.Distance(grabPoint.position,_palm.position) > _jointDistance && isGrabbing)
        {
            yield return new WaitForEndOfFrame();
        }

        //Freeze hand in object
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        targetBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        targetBody.interpolation = RigidbodyInterpolation.Interpolate;

        //Attach joints
        handJoint = gameObject.AddComponent<FixedJoint>();
        handJoint.connectedBody = targetBody;
        handJoint.breakForce = float.PositiveInfinity;
        handJoint.breakTorque = float.PositiveInfinity;
        handJoint.connectedMassScale = 1;
        handJoint.massScale = 1;
        handJoint.enableCollision = false;
        handJoint.enablePreprocessing = false;
        
        objectJoint = heldObject.AddComponent<FixedJoint>();
        objectJoint.connectedBody = body;
        objectJoint.breakForce = float.PositiveInfinity;
        objectJoint.breakTorque = float.PositiveInfinity;
        objectJoint.connectedMassScale = 1;
        objectJoint.massScale = 1;
        objectJoint.enableCollision = false;
        objectJoint.enablePreprocessing = false;

        //Reset follow target
        followTarget = _controller.gameObject.transform;
    }

    public void HideHand()
    {
        _handModel.SetActive(false);
        if (replaceHand)
            replaceHand.SetActive(false);
    }

    public void ShowHand()
    {
        if (replaceHand)
            replaceHand.SetActive(true);
        else
            _handModel.SetActive(true);
    }
}
