using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LinearAlgebra;
public class CheckLinesIntersect : MonoBehaviour
{
    [SerializeField] LineRenderer _lineA;
    [SerializeField] LineRenderer _lineB;
    [SerializeField] MeshRenderer _indicator;
    [SerializeField] Material _positive;
    [SerializeField] Material _negative;
    public bool isIntersecting = false;
    Vector3 a0, a1, b0, b1,intersect;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(a0, 0.1f);
        Gizmos.DrawSphere(a1, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(b0, 0.1f);
        Gizmos.DrawSphere(b1, 0.1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(intersect, 0.1f);
    }
    private void Update()
    {
        a0 = _lineA.transform.TransformPoint(_lineA.GetPosition(0));
        a1 = _lineA.transform.TransformPoint(_lineA.GetPosition(1));
        b0 = _lineB.transform.TransformPoint(_lineB.GetPosition(0));
        b1 = _lineB.transform.TransformPoint(_lineB.GetPosition(1));

        Vector2 intersectPoint;
        isIntersecting = LineLineIntersection.IsIntersecting(out intersectPoint, a0, a1, b0, b1);
        intersect = new Vector3(intersectPoint.x, 0, intersectPoint.y);
        _indicator.material = isIntersecting ? _positive : _negative;
    }
}
