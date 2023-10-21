using UnityEngine;

public class FlyingTool : GrabbableObjectChecker
{
    public bool IsLock;
    public Transform Target;
    public bool IsGrabbed => isGrabbed;
    public bool IsTargetFollowAuth = true;

    [SerializeField] float _moveSpeed = 5;
    [SerializeField] float _rotationSpeed = 5;
    [SerializeField] FlyingToolLock flyingToolLock;

    private bool isOnOriginPos = true;
    private bool isOnOriginRot = true;
    private Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
    }
    private void Start()
    {
        AddHandListeners();

        if (!Target)
        {
            Target = new GameObject(name + "_Target").transform;
            Target.transform.position = transform.position;
            Target.transform.rotation = transform.rotation;
        }

        if (flyingToolLock && IsLock)
            flyingToolLock.Lock();
        else if(flyingToolLock)
            flyingToolLock.Unlock();
    }

    private void Update()
    {
        if (IsTargetFollowAuth && !isGrabbed)
        {
            if (Vector3.Distance(transform.position, Target.position) <= 0.01f)
                isOnOriginPos = true;
            else
                isOnOriginPos = false;

            if (Quaternion.Angle(Target.rotation, transform.rotation) <= 10)
                isOnOriginRot = true;
            else
                isOnOriginRot = false;

            if (isOnOriginPos && isOnOriginPos)
                body.isKinematic = true;
            else
                body.isKinematic = false;
        }
    }

    private void FixedUpdate()
    {
        if (!IsGrabbed && IsTargetFollowAuth)
        {
            if (!isOnOriginPos)
            {
                FollowPosition();
            }

            if (!isOnOriginRot)
            {
                FollowRotation();
            }
        }
    }

    public override void CheckGrab(GameObject grabbedObject, HandSides side)
    {
        base.CheckGrab(grabbedObject, side);
        if (isLastInteractedObj)
            Grab();
    }

    private void FollowPosition()
    {
        var distance = Vector3.Distance(Target.position, transform.position);
        body.velocity = (Target.position - transform.position).normalized * (_moveSpeed * distance);
    }

    private void FollowRotation()
    {
        var quaternion = Target.rotation * Quaternion.Inverse(body.rotation);
        quaternion.ToAngleAxis(out float angle, out Vector3 axis);
        body.angularVelocity = angle * axis * Mathf.Deg2Rad * _rotationSpeed;
    }

    public void Grab()
    {
        if (!IsLock)
        {
            isOnOriginPos = false;
            isOnOriginRot = false;
            if(IsTargetFollowAuth)
                body.isKinematic = false;
        }else
        {
            flyingToolLock.TryPick();
        }
    }
    public void SetFollowPose(Transform target)
    {
        Target = target;
    }

    public void Lock()
    {
        if (flyingToolLock)
            flyingToolLock.Lock();
        IsLock = true;
    }

    public void Unlock()
    {
        if (flyingToolLock)
            flyingToolLock.Unlock();
        IsLock = false;
    }
}
