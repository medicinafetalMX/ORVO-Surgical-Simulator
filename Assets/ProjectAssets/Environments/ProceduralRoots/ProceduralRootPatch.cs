using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(ProceduralRootPatch))]
public class ProceduralRootPatchEditor: Editor
{
    public Vector3 startPos;
    public Vector3 endPos;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(20);
        startPos = EditorGUILayout.Vector3Field("Start position", startPos);
        endPos = EditorGUILayout.Vector3Field("End pos", endPos);
        if (GUILayout.Button("Generate"))
            (target as ProceduralRootPatch).Generate(startPos,endPos);
    }
}
#endif

public class ProceduralRootPatch : MonoBehaviour
{
    public Vector3 Intersection => intersection;
    [SerializeField] Material _arteryMaterial;
    [SerializeField] Material _veinMaterial;
    [SerializeField] LineRenderer _patch;
    [SerializeField] LayerMask _projectionMask;
    [SerializeField] float _fractionSize;
    [SerializeField] float _heightVariation;
    [SerializeField] float _width;
    private LineRenderer arterieLine;
    private LineRenderer veinLine;
    private Vector3 intersection;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(intersection, 0.001f);
    }
    private void DrawLine(LineRenderer line,Vector3 startPos,Vector3 endPos)
    {
        Vector3 dir = endPos - startPos;
        Vector3 normal = Vector3.Cross(dir.normalized, Vector3.up);
        int pointsAmount = (int)(dir.magnitude / _fractionSize);
        if (pointsAmount < 2)
            pointsAmount = 2;
        line.positionCount = pointsAmount;
        line.SetPosition(0, startPos);
        for (int i = 1; i < pointsAmount; i++)
        {
            line.SetPosition(i,startPos + dir.normalized * _fractionSize * i + normal.normalized * Random.Range(0,_heightVariation) * (i%2==0?1:-1));
        }
        line.SetPosition(pointsAmount - 1, endPos);
    }

    public void Generate(Vector3 veinPos, Vector3 arteriePos)
    {
        veinPos = transform.InverseTransformPoint(veinPos);
        arteriePos = transform.InverseTransformPoint(arteriePos);

        Vector3 dir = arteriePos - veinPos;
        Vector3 normal = Vector3.Cross(dir.normalized, Vector3.up);
        if (arterieLine == null)
        {
            arterieLine = Instantiate(_patch, transform);
            arterieLine.material = _arteryMaterial;
            arterieLine.widthMultiplier = _width;
        }
        if (veinLine == null)
        {
            veinLine = Instantiate(_patch, transform);
            veinLine.material = _veinMaterial;
            veinLine.widthMultiplier = _width;
        }
        float veinLenght = dir.magnitude - Random.value * dir.magnitude;
        intersection = veinPos + dir.normalized * veinLenght;
        intersection += normal.normalized * (Random.value > 0.5f ? 1 : -1) * _heightVariation;
        DrawLine(veinLine, veinPos, intersection);
        DrawLine(arterieLine, intersection, arteriePos);
        intersection = transform.TransformPoint(intersection);
        veinLine.gameObject.SetActive(true);
        arterieLine.gameObject.SetActive(true);
    }

    public void ProjectPoints(float veinOffset,float arterieOffset)
    {
        RaycastHit hit;
        Ray ray;
        Vector3[] veinPositions = new Vector3[veinLine.positionCount];
        Vector3[] arteriePositions = new Vector3[arterieLine.positionCount];
        veinLine.GetPositions(veinPositions);
        arterieLine.GetPositions(arteriePositions);
        int positionIndex = 0;
        for (int i = 0; i < veinPositions.Length; i++)
        {
            bool isIntersection = i == veinPositions.Length - 1;
            Vector3 point = transform.TransformPoint(veinPositions[i]);
            ray = new Ray(point + Vector3.up * 0.1f, Vector3.down);
            if (Physics.Raycast(ray, out hit, 1, _projectionMask))
            {
                Vector3 projectPoint = hit.point + hit.normal * (isIntersection?0:veinOffset);
                veinPositions[positionIndex] = transform.InverseTransformPoint(projectPoint);
            }
            positionIndex++;
        }
        veinLine.SetPositions(veinPositions);
        positionIndex = 0;
        for (int i = 0; i < arteriePositions.Length; i++)
        {
            Vector3 point = transform.TransformPoint(arteriePositions[i]);
            bool isIntersection = i == 0;
            ray = new Ray(point + Vector3.up * 0.1f, Vector3.down);
            if (Physics.Raycast(ray, out hit, 1, _projectionMask))
            {
                Vector3 projectPoint = hit.point + hit.normal * (isIntersection?0:arterieOffset);
                arteriePositions[positionIndex] = transform.InverseTransformPoint(projectPoint);
            }
            positionIndex++;
        }
        arterieLine.SetPositions(arteriePositions);
    }
}
