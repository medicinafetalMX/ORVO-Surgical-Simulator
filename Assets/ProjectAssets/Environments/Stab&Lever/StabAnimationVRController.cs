using UnityEngine;
using UnityEngine.Events;

public class StabAnimationVRController : GrabbableObjectChecker
{
    public UnityAction OnStab;
    public UnityAction OnUnstab;
    public bool IsStabbed { get; set; }
    public bool StabCoolDown { get { return coolDownTimer <= 0; } }
    public Transform BeginPoint => _stabBegin;
    public Transform FollowTarget { get; set; }
    public Vector3 StabPoint => beginStabPoint;
    public float StabAmount { get { return stabMagnitude>0?currentStabDistance / stabMagnitude : 0; } }

    [Header("Components")]
    [SerializeField] Transform _directionPointer;
    [SerializeField] Transform _stabLimit;
    [SerializeField] Transform _stabBegin;

    [Header("Properties")]
    [SerializeField][Range(0,1)] float _stabDistance;

    [Header("Unstab settings")]
    [SerializeField] private float unstabTime = 1f;
    [SerializeField] private float unstabMagnitude = 0.1f;

    private float unstabTimer;
    private float stabMagnitude;
    private float currentStabDistance;
    private float stabCoolDownTime = 1;
    private float coolDownTimer = 0;
    private Rigidbody body;
    private Vector3 initialHandPos;
    private Vector3 initialTransformPos;
    private Vector3 beginStabPoint;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(beginStabPoint,0.01f);
        _stabDistance = StabAmount;
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        stabMagnitude = (_stabBegin.position - _stabLimit.position).magnitude;
    }

    private void Start()
    {
        AddHandListeners();
    }

    private void Update()
    {
        if (isGrabbed && IsStabbed)
        {
            Vector3 targetOffset = FollowTarget.position - initialHandPos;
            Vector3 project = Vector3.Project(targetOffset, _directionPointer.forward);

            currentStabDistance = (_stabBegin.position - beginStabPoint).magnitude;

            float stabDirection = Vector3.Dot(_stabLimit.forward, project);
            bool outOfStab = Vector3.Dot(_stabBegin.position - beginStabPoint, _stabBegin.forward) < 0;
            
            if((currentStabDistance < stabMagnitude && !outOfStab) || 
                (currentStabDistance >= stabMagnitude && stabDirection < 0) || 
                (outOfStab && stabDirection > 0))
                transform.position = initialTransformPos + project;

            Vector3 endOffset = FollowTarget.position - _stabLimit.position;
            if (outOfStab && stabDirection < 0 && endOffset.magnitude > unstabMagnitude)
            {
                unstabTimer += Time.deltaTime;
                if (unstabTimer >= unstabTime)
                {
                    unstabTimer = 0;
                    Unstab();
                }
            }
        }
        if(coolDownTimer> 0)
        {
            coolDownTimer -= Time.deltaTime;
        }
    }

    private void Grab(Transform target)
    {
        FollowTarget = target;
        UpdateStabPositions();
    }

    private void Release(HandSides side)
    {
        if (side == HandSides.LeftHand)
        {
            if (isRightGrabbing)
            {
                Grab(PlayerHandsManager.Instance.LeftHand.Controller.transform);
            }
        }
        else if (side == HandSides.RightHand)
        {
            if (isLeftGrabbing)
            {
                Grab(PlayerHandsManager.Instance.RightHand.Controller.transform);
            }
        }
    }

    public void Translate(Vector3 stabBeginPos)
    {
        Vector3 dir = stabBeginPos - _stabBegin.position;
        transform.position += dir;
    }

    public void Stab()
    {
        coolDownTimer = stabCoolDownTime;
        body.isKinematic = true;
        UpdateStabPositions();
        IsStabbed = true;
        OnStab?.Invoke();
    }

    public void Unstab()
    {
        body.isKinematic = false;
        IsStabbed = false;
        OnUnstab?.Invoke();
    }

    public void RaycastStabPoint()
    {
        RaycastHit hit;
        Ray ray = new Ray(_stabBegin.position + _directionPointer.forward *0.01f, _directionPointer.forward);
        if(Physics.Raycast(ray,out hit,0.1f))
        {
            beginStabPoint = hit.point;
        }
        else
        {
            beginStabPoint = _stabBegin.position;
        }
    }

    public void SetStabPoint(Vector3 stabPosition)
    {
        beginStabPoint = stabPosition;
    }

    public void UpdateStabPositions()
    {
        initialHandPos = FollowTarget.transform.position;
        initialTransformPos = transform.position;
        currentStabDistance = (_stabBegin.position - beginStabPoint).magnitude;
    }

    public override void CheckGrab(GameObject grabbedObject, HandSides side)
    {
        base.CheckGrab(grabbedObject, side);
        if (isLastInteractedObj)
        {
            Transform target = side == HandSides.LeftHand ? PlayerHandsManager.Instance.LeftHand.Controller.transform : PlayerHandsManager.Instance.RightHand.Controller.transform;
            Grab(target);
        }
    }

    public override void CheckRelease(GameObject grabbedObject, HandSides side)
    {
        base.CheckRelease(grabbedObject, side);
        if (isLastInteractedObj)
        {
            if (!isGrabbed)
                Release(side);
        }
    }
}
