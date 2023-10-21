using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(ProceduralRoots))]
public class ProceduralRootsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate"))
        {
            (target as ProceduralRoots).Generate();
        }
    }
}
#endif
public class ProceduralRoots : MonoBehaviour
{
    public bool EndsInBranch => endsInBranch;
    public Vector3 EndPoint { get
        {
            return transform.TransformPoint(_lineRenderer.GetPosition(_lineRenderer.positionCount - 1));
        } 
    }
    public int Iteration;
    public float BranchSize;
    public float NormalOffset = 0;
    public Vector3 InitialDir;
    public Transform Parent;
    public Vector3[] Positions => positions.ToArray();
    public ProceduralRoots[] Branches => branches.ToArray();

    [SerializeField] int _minBranches;
    [SerializeField] int _maxBranches;
    [SerializeField] int _maxBranchSteps;
    [SerializeField] int _minBranchSteps;
    [SerializeField] float _sizeDecrease;
    [SerializeField] float _width = 0.01f;
    [SerializeField] GameObject _branchPrefab;
    [SerializeField] LineRenderer _lineRenderer;

    [Header("Projection")]
    [SerializeField] bool _projectLines;
    [SerializeField] float _Yoffset;
    [SerializeField] float _maxDistance;
    [SerializeField] LayerMask _projectionMask;

    private bool endsInBranch = false;
    private List<Vector3> positions = new List<Vector3>();
    private List<ProceduralRoots> branches = new List<ProceduralRoots>();
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(EndPoint, 0.001f);
    }
    private void Awake()
    {
        InitialDir = transform.forward;
    }
    
    public void CreateRoot(Vector3 position, Vector3 newDir)
    {
        var branchObject = Instantiate(_branchPrefab);
        branchObject.transform.position = transform.TransformPoint(position);
        branchObject.transform.rotation = transform.rotation;
        var branch = branchObject.GetComponent<ProceduralRoots>();
        branch.Parent = Parent;
        branch.transform.SetParent(Parent);
        branch.InitialDir = newDir.normalized;
        branch.Iteration = Iteration - 1;
        branch.BranchSize = BranchSize - _sizeDecrease;
        branch.Generate();
        branch.SetMaterial(_lineRenderer.material);
        this.branches.Add(branch);
    }
    public void Generate()
    {
        if (Iteration > 0)
        {
            bool lastIteration = Iteration == 1;
            int branches = Random.Range(_minBranches, _maxBranches+1);
            int steps = Random.Range(_minBranchSteps, _maxBranchSteps+1);
            positions = new List<Vector3>(); 
            Vector3 branchPos = Vector3.zero;
            Vector3 newDir = Vector3.zero;

            positions.Add(branchPos);
            _lineRenderer.widthMultiplier = Iteration * _width;

            for (int i = 0; i < steps; i++)
            {
                newDir = (InitialDir + (transform.right * Random.Range(-0.5f,0.5f))).normalized;
                branchPos += newDir * BranchSize;
                if (!lastIteration && branches > 0 && Random.value>0.5f)
                {
                    branches--;
                    Vector3 dirRight = -Vector3.Cross(newDir, Vector3.up).normalized;
                    Vector3 branchDir = newDir + dirRight * Random.Range(-0.5f,0.5f);
                    CreateRoot(branchPos,branchDir);
                }
                positions.Add(branchPos);
            }
            if (!lastIteration && branches > 0)
            {
                endsInBranch = true;
                if (branches > 1)
                {
                    float angleSteps = 1f / branches;
                    for (int i = 0; i < branches; i++)
                    {
                        Vector3 dirRight = -Vector3.Cross(newDir, Vector3.up).normalized;
                        CreateRoot(branchPos, newDir + dirRight * ((i+1 * angleSteps) - 0.5f));
                    }
                }
                else
                {
                    Vector3 dirRight = -Vector3.Cross(newDir, Vector3.up).normalized;
                    CreateRoot(branchPos, newDir + dirRight * Random.Range(-0.5f, 0.5f));
                }
            }
            _lineRenderer.positionCount = positions.Count;
            _lineRenderer.SetPositions(positions.ToArray());
        }
    }

    public void SetMaterial(Material material)
    {
        _lineRenderer.material = material;
    }

    public void ProjectPoints()
    {
        RaycastHit hit;
        Ray ray;
        int positionIndex = 0;
        foreach (var point in positions.ToArray())
        {
            ray = new Ray(transform.TransformPoint(point) + Vector3.up * _Yoffset, Vector3.down);
            if (Physics.Raycast(ray, out hit, _maxDistance, _projectionMask))
            {
                Vector3 projectPoint = transform.InverseTransformPoint(hit.point) + hit.normal * NormalOffset;
                positions[positionIndex] = projectPoint;
            }
            else
                Debug.LogError("Cant project point on " + name+ " "+positionIndex);
            positionIndex++;
        }
        _lineRenderer.SetPositions(positions.ToArray());
    }
}
