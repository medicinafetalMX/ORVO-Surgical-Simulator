using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabUltrasoundReplicator : MonoBehaviour
{
    [SerializeField] float _maxDistance;
    [SerializeField] StabAnimationVRController _stab;
    private Vector3 initialPosition;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up * _maxDistance);
    }

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        transform.position = initialPosition + transform.up * _stab.StabAmount * _maxDistance;
    }
}
