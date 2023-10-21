using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierRoute : MonoBehaviour
{
    [SerializeField] Transform[] _controlPoints;
    [SerializeField] [Range(0, 1)] float _evaluate;
    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t+=0.05f)
        {
            if (_evaluate >= t && _evaluate < t + 0.05f)
            {
                t = _evaluate;
                Gizmos.color = Color.cyan;
            }
            else
                Gizmos.color = Color.gray;
            Vector3 bezierPoint = Evaluate(t);
            Gizmos.DrawSphere(bezierPoint, 0.01f);
        }

        Gizmos.DrawLine( new Vector3(_controlPoints[0].position.x, _controlPoints[0].position.y),
            new Vector3(_controlPoints[1].position.x, _controlPoints[1].position.y));
        Gizmos.DrawLine(new Vector3(_controlPoints[2].position.x, _controlPoints[2].position.y),
            new Vector3(_controlPoints[3].position.x, _controlPoints[3].position.y));
    }

    public Vector3 Evaluate(float t)
    {
        float bezierX = Mathf.Pow((1 - t), 3) * _controlPoints[0].position.x + 3 * Mathf.Pow((1 - t), 2) * t * _controlPoints[1].position.x + 3 * Mathf.Pow((1 - t), 1) * Mathf.Pow(t, 2) * _controlPoints[2].position.x + Mathf.Pow(t, 3) * _controlPoints[3].position.x;
        float bezierY = Mathf.Pow((1 - t), 3) * _controlPoints[0].position.y + 3 * Mathf.Pow((1 - t), 2) * t * _controlPoints[1].position.y + 3 * Mathf.Pow((1 - t), 1) * Mathf.Pow(t, 2) * _controlPoints[2].position.y + Mathf.Pow(t, 3) * _controlPoints[3].position.y;
        float bezierZ = Mathf.Pow((1 - t), 3) * _controlPoints[0].position.z + 3 * Mathf.Pow((1 - t), 2) * t * _controlPoints[1].position.z + 3 * Mathf.Pow((1 - t), 1) * Mathf.Pow(t, 2) * _controlPoints[2].position.z + Mathf.Pow(t, 3) * _controlPoints[3].position.z;
        Vector3 bezierPoint = new Vector3(bezierX, bezierY, bezierZ);
        return bezierPoint;
    }
}
