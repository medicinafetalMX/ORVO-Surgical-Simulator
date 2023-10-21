using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalMovement : MonoBehaviour
{
    public float StabAmount => (currentRadius - _radius.x) / (_radius.y - _radius.x);

    [SerializeField] Vector2 _radius;
    [SerializeField] Transform _target;
    [SerializeField] Transform _fixedPos;
    [SerializeField] Transform _forwardPointer;
    [SerializeField] Vector3 _offset;

    [SerializeField] private float currentRadius;
    private Vector3 direction;
    private Vector3 currentUp;
    private Vector3 currentFwd;
    private Vector3 fixedPosition;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _radius.x);
        Gizmos.DrawWireSphere(transform.position, _radius.y);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, currentRadius);

        CalculateDirections();
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(fixedPosition, 0.01f);
        Gizmos.DrawSphere(transform.position + direction.normalized * currentRadius, 0.01f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(fixedPosition, fixedPosition + currentUp * 0.05f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(fixedPosition, fixedPosition + currentFwd * 0.05f);
    }

    private void Update()
    {
        CalculateRadius();
        CalculateDirections();
        Quaternion newRot = Quaternion.LookRotation(currentFwd, currentUp);
        newRot *= Quaternion.Euler(_offset);
        _fixedPos.transform.rotation = newRot;
    }

    private void CalculateDirections()
    {
        currentUp = direction.normalized;
        fixedPosition = transform.position + direction.normalized * currentRadius;
        Vector3 tmpPos = fixedPosition + _forwardPointer.forward * 0.05f;
        Vector3 tmpDir = (tmpPos - transform.position).normalized;
        Vector3 fixedTmpPos = transform.position + tmpDir * currentRadius;
        currentFwd = (fixedTmpPos - fixedPosition).normalized;
    }

    private void CalculateRadius()
    {
        direction = _target.position - transform.position;
        currentRadius = Mathf.Clamp(direction.magnitude, _radius.x, _radius.y);
    }
}
