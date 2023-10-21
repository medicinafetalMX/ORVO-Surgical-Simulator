using UnityEngine;

public class GorillaPhysicsHand : MonoBehaviour
{
    [Header("PID")]
    [SerializeField] float frequency = 50f;
    [SerializeField] float damping = 1f;
    [SerializeField] float rotFrequency = 100f;
    [SerializeField] float rotDamping = 0.9f;
    [SerializeField] Rigidbody _playerBody;
    [SerializeField] Transform target;
    [Header("Springs")]
    [SerializeField] float climbForce = 1000f;
    [SerializeField] float climbDrag = 500f;

    private bool isColliding;
    private Rigidbody body;
    private Vector3 previousPosition;
    private AudioSource audioSource;
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.maxAngularVelocity = float.PositiveInfinity;
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
        previousPosition = transform.position;
    }
    private void FixedUpdate()
    {
        PIDMovement();
        PIDRotation();
        if(isColliding)
            HookesLaw();
    }

    private void PIDMovement()
    {
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Vector3 force = (target.position - transform.position) * ksg + (_playerBody.velocity - body.velocity) * kdg;
        body.AddForce(force, ForceMode.Acceleration);
    }

    private void PIDRotation()
    {
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Quaternion q = target.rotation * Quaternion.Inverse(transform.rotation);
        if (q.w < 0)
        {
            q.x = -q.x;
            q.y = -q.y;
            q.z = -q.z;
            q.w = -q.w;
        }
        q.ToAngleAxis(out float angle, out Vector3 axis);
        axis.Normalize();
        axis *= Mathf.Deg2Rad;
        Vector3 torque = ksg * axis * angle + -body.angularVelocity * kdg;
        body.AddTorque(torque, ForceMode.Acceleration);
    }

    private void HookesLaw()
    {
        Vector3 displacementFromResting = transform.position - target.position;
        Vector3 force = displacementFromResting * climbForce;
        float drag = GetDrag();
        _playerBody.AddForce(force, ForceMode.Acceleration);
        _playerBody.AddForce(drag * -_playerBody.velocity * climbDrag, ForceMode.Acceleration);
    }

    private float GetDrag()
    {
        Vector3 handVelocity = (target.localPosition - previousPosition) / Time.fixedDeltaTime;
        float drag = 1 / handVelocity.magnitude + 0.01f;
        drag = Mathf.Clamp(drag, 0.03f, 1);
        previousPosition = transform.position;
        return drag;
    }
    private void OnCollisionEnter(Collision collision)
    {
        isColliding = true;
        audioSource.Play();
    }

    private void OnCollisionExit(Collision collision)
    {
        isColliding = false;
    }
}
