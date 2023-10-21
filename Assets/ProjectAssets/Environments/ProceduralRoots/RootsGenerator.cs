using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using LinearAlgebra;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(RootsGenerator))]
public class RootsGeneratorEditor : Editor
{
    private RootsGenerator generator;
    private void OnEnable()
    {
        generator = target as RootsGenerator;
    }
    private IEnumerator Generate()
    {
        generator.DestroyTree();
        yield return generator.Build();
        yield return generator.ProjectPoints();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate"))
        {
            generator.StartCoroutine(Generate());
        }
    }
}
#endif
public class RootsGenerator : MonoBehaviour
{
    public FetoscopyTargetPointData[] Intersections =>intersections.ToArray();

    [SerializeField] Transform _placenta;
    [SerializeField] Transform _firstOrigin;
    [SerializeField] Transform _secondOrigin;
    [SerializeField] Material _veinMaterial;
    [SerializeField] Material _arterieMaterial;
    [SerializeField] ProceduralRoots _root;
    [SerializeField] ProceduralRootPatch _patch;

    [Header("Projection")]
    [SerializeField] bool _projectPoints;
    [SerializeField] float _Yoffset;
    [SerializeField] float _maxDistance;
    [SerializeField] LayerMask _projectionMask;
    private struct BranchDir
    {
        public Vector3 begin;
        public Vector3 end;
    }
    private bool intersectionsCalculated = false;
    private ProceduralRoots firstVein,firstArterie,secondVein,secondArterie;
    private Dictionary<ProceduralRoots, BranchDir[]> directions = new Dictionary<ProceduralRoots, BranchDir[]>();
    private Dictionary<ProceduralRoots,Vector3[]> ends = new Dictionary<ProceduralRoots, Vector3[]>();
    private List<FetoscopyTargetPointData> intersections = new List<FetoscopyTargetPointData>();
    private List<ProceduralRootPatch> patches = new List<ProceduralRootPatch>();
    private GameObject firstVeinContainer, firstArterieContainer, secondVeinContainer, secondArterieContainer,patchesContainer;
    private void OnDrawGizmosSelected()
    {
        if (intersectionsCalculated)
        {
            foreach (var intersection in intersections)
            {
                switch (intersection.type)
                {
                    case CirculationIntersectionType.ArterieArterie:
                        Gizmos.color = Color.blue;
                        break;
                    case CirculationIntersectionType.ArterieVein:
                        Gizmos.color = new Color(1, 0, 1);
                        break;
                    case CirculationIntersectionType.VeinVein:
                        Gizmos.color = Color.red;
                        break;
                    default:
                        break;
                }
                Gizmos.DrawSphere(intersection.position, 0.001f);
            }
            Gizmos.color = Color.green;
            foreach(var end in ends)
            {
                Vector3[] points = end.Value;
                foreach(var point in points)
                    Gizmos.DrawSphere(point, 0.001f);
            }
        }
    }

    private void CalculateIntersections()
    {
        intersectionsCalculated = false;
        intersections.Clear();

        BranchDir[] firstVeinLines = directions[firstVein];
        BranchDir[] firstArterieLines = directions[firstArterie];
        BranchDir[] secondVeinLines = directions[secondVein];
        BranchDir[] secondArterieLines = directions[secondArterie];

        for (int i = 0; i < firstVeinLines.Length; i++)
        {
            BranchDir veinDir = firstVeinLines[i];
            for (int j = 0; j < secondVeinLines.Length; j++)
            {
                BranchDir secondVeinDir = secondVeinLines[j];
                Vector2 intersectPoint;
                if(LineLineIntersection.IsIntersecting(out intersectPoint,veinDir.begin, veinDir.end, secondVeinDir.begin, secondVeinDir.end))
                {
                    Vector3 intersection = new Vector3(intersectPoint.x, 0, intersectPoint.y);
                    intersections.Add(new FetoscopyTargetPointData()
                    {
                        position = intersection,
                        type = CirculationIntersectionType.VeinVein
                    });
                }
            }
            for (int j = 0; j < secondArterieLines.Length; j++)
            {
                BranchDir secondArterieDir = secondArterieLines[j];
                Vector2 intersectPoint;
                if (LineLineIntersection.IsIntersecting(out intersectPoint, veinDir.begin, veinDir.end, secondArterieDir.begin, secondArterieDir.end))
                {
                    Vector3 intersection = new Vector3(intersectPoint.x, 0, intersectPoint.y);
                    intersections.Add(new FetoscopyTargetPointData()
                    {
                        position = intersection,
                        type = CirculationIntersectionType.ArterieVein
                    });
                }
            }
        }

        for (int i = 0; i < firstArterieLines.Length; i++)
        {
            BranchDir arterieDir = firstArterieLines[i];
            for(int j = 0; j < secondVeinLines.Length; j++)
            {
                BranchDir secondVeinDir = secondVeinLines[j];
                Vector2 intersectPoint;
                if (LineLineIntersection.IsIntersecting(out intersectPoint, arterieDir.begin, arterieDir.end, secondVeinDir.begin, secondVeinDir.end))
                {
                    Vector3 intersection = new Vector3(intersectPoint.x, 0, intersectPoint.y);
                    intersections.Add(new FetoscopyTargetPointData()
                    {
                        position = intersection,
                        type = CirculationIntersectionType.ArterieVein
                    });
                }
            }
            for (int j = 0; j < secondArterieLines.Length; j++)
            {
                BranchDir secondArterieDir = secondArterieLines[j];
                Vector2 intersectPoint;
                if (LineLineIntersection.IsIntersecting(out intersectPoint, arterieDir.begin, arterieDir.end, secondArterieDir.begin, secondArterieDir.end))
                {
                    Vector3 intersection = new Vector3(intersectPoint.x, 0, intersectPoint.y);
                    intersections.Add(new FetoscopyTargetPointData()
                    {
                        position = intersection,
                        type = CirculationIntersectionType.ArterieArterie
                    });
                }
            }
        }
        Debug.Log($"Intersections : {intersections.Count}");

        intersectionsCalculated = true;
    }

    private void GeneratePatches()
    {
        int missingIntersections = 10 - intersections.Count;
        Debug.Log("Generating " + missingIntersections + " patches");
        int firstVeinIntersections = missingIntersections - Random.Range(0, missingIntersections);
        int firstArterieIntersections = missingIntersections - firstVeinIntersections;

        Vector3[] firstVeinEnds = ends[firstVein];
        Vector3[] secondArterieEnds = ends[secondArterie];
        int[] indexes = new int[firstVeinEnds.Length];
        ArrayExtensions.FillArraySequence(indexes);
        RandomExtensions.Shuffle(new System.Random(), indexes);
        for (int i = 0; i < firstVeinIntersections; i++)
        {
            int veinIndex = i;
            if (veinIndex >= firstVeinEnds.Length)
                veinIndex -= firstVeinEnds.Length;
            int index = indexes[veinIndex];
            Vector3 veinEnd = firstVeinEnds[index];
            int secondArterieIndex = 0;
            float endsDistance = 10000;
            for (int j = 0; j < secondArterieEnds.Length; j++)
            {
                float distance = Vector3.Distance(secondArterieEnds[j], veinEnd);
                if (distance < endsDistance)
                {
                    endsDistance = distance;
                    secondArterieIndex = j;
                }
            }
            Vector3 arterieEnd = secondArterieEnds[secondArterieIndex];
            var patch = CreatePatch(veinEnd, arterieEnd);
            intersections.Add(new FetoscopyTargetPointData()
            {
                type = CirculationIntersectionType.ArterieVein,
                position = patch.Intersection
            });
            patches.Add(patch);
        }

        Vector3[] firstArterieEnds = ends[firstArterie];
        Vector3[] secondVeinEnds = ends[secondVein];
        indexes = new int[firstArterieEnds.Length];
        ArrayExtensions.FillArraySequence(indexes);
        RandomExtensions.Shuffle(new System.Random(), indexes);
        for (int i = 0; i < firstArterieIntersections; i++)
        {
            int arterieIndex = i;
            if (arterieIndex >= firstArterieEnds.Length)
                arterieIndex -= firstArterieEnds.Length;
            int index = indexes[arterieIndex];
            Vector3 arterieEnd = firstArterieEnds[index];
            int secondVeinIndex = 0;
            float endsDistance = 10000;
            for (int j = 0; j < secondVeinEnds.Length; j++)
            {
                float distance = Vector3.Distance(secondVeinEnds[j], arterieEnd);
                if (distance < endsDistance)
                {
                    endsDistance = distance;
                    secondVeinIndex = j;
                }
            }
            Vector3 veinEnd = secondVeinEnds[secondVeinIndex];
            var patch = CreatePatch(veinEnd, arterieEnd);
            intersections.Add(new FetoscopyTargetPointData()
            {
                type = CirculationIntersectionType.ArterieVein,
                position = patch.Intersection
            });
            patches.Add(patch);
        }
    }

    private ProceduralRootPatch CreatePatch(Vector3 veinPos,Vector3 arteriePos)
    {
        var patch = Instantiate(_patch, patchesContainer.transform);
        patch.Generate(veinPos, arteriePos);
        return patch;
    }

    private IEnumerator Generate()
    {
        firstVeinContainer = new GameObject("First Vein");
        firstVein = Instantiate(_root, firstVeinContainer.transform);
        firstVein.Parent = firstVeinContainer.transform;
        firstVein.transform.position = _firstOrigin.transform.position;
        firstVein.transform.rotation = Quaternion.LookRotation(_firstOrigin.forward, Vector3.up);
        firstVein.name += "_Vein";
        firstVein.SetMaterial(_veinMaterial);
        firstVein.Generate();
        yield return new WaitForEndOfFrame();

        firstArterieContainer = new GameObject("First Arterie");
        firstArterie = Instantiate(_root, firstArterieContainer.transform);
        firstArterie.Parent = firstArterieContainer.transform;
        firstArterie.transform.position = _firstOrigin.transform.position;
        firstArterie.transform.rotation = Quaternion.LookRotation(_firstOrigin.forward, Vector3.up);
        firstArterie.name += "_Arterie";
        firstArterie.SetMaterial(_arterieMaterial);
        firstArterie.Generate();
        yield return new WaitForEndOfFrame();

        secondVeinContainer = new GameObject("Second Vein");
        secondVein = Instantiate(_root, secondVeinContainer.transform);
        secondVein.Parent = secondVeinContainer.transform;
        secondVein.transform.position = _secondOrigin.transform.position;
        secondVein.transform.rotation = Quaternion.LookRotation(_secondOrigin.forward, Vector3.up);
        secondVein.name += "_Vein";
        secondVein.SetMaterial(_veinMaterial);
        secondVein.Generate();
        yield return new WaitForEndOfFrame();

        secondArterieContainer = new GameObject("Second Arterie");
        secondArterie = Instantiate(_root, secondArterieContainer.transform);
        secondArterie.Parent = secondArterieContainer.transform;
        secondArterie.transform.position = _secondOrigin.transform.position;
        secondArterie.transform.rotation = Quaternion.LookRotation(_secondOrigin.forward, Vector3.up);
        secondArterie.name += "_Arterie";
        secondArterie.SetMaterial(_arterieMaterial);
        secondArterie.Generate();
        yield return new WaitForEndOfFrame();

        patchesContainer = new GameObject("Patches");
        patchesContainer.transform.SetParent(_placenta);

        firstVeinContainer.transform.SetParent(_placenta);
        firstArterieContainer.transform.SetParent(_placenta);
        secondVeinContainer.transform.SetParent(_placenta);
        secondArterieContainer.transform.SetParent(_placenta);
    }

    private IEnumerator StoreDirections()
    {
        directions.Clear();
        directions[firstVein] = ExtractDirections(firstVein);
        ends[firstVein] = ExtractEnds(firstVein);
        yield return new WaitForEndOfFrame();
        directions[secondVein] = ExtractDirections(secondVein);
        ends[secondVein] = ExtractEnds(secondVein);
        yield return new WaitForEndOfFrame();
        directions[firstArterie] = ExtractDirections(firstArterie);
        ends[firstArterie] = ExtractEnds(firstArterie);
        yield return new WaitForEndOfFrame();
        directions[secondArterie] = ExtractDirections(secondArterie);
        ends[secondArterie] = ExtractEnds(secondArterie);
        yield return new WaitForEndOfFrame();
    }

    private Vector3[] ExtractEnds(ProceduralRoots root)
    {
        List<Vector3> ends = new List<Vector3>();
        if (root.EndsInBranch)
        {
            foreach (var branch in root.Branches)
                ends.AddRange(ExtractEnds(branch));
        }
        else
        {
            if (root.Branches.Length > 0)
            {
                foreach (var branch in root.Branches)
                    ends.AddRange(ExtractEnds(branch));
            }
            Vector3 endPos = root.EndPoint;
            ends.Add(endPos);
        }
        return ends.ToArray();
    }

    private BranchDir[] ExtractDirections(ProceduralRoots root)
    {
        List<BranchDir> directions = new List<BranchDir>();
        for (int i = 0; i < root.Positions.Length - 1; i++)
        {
            BranchDir trunk = new BranchDir()
            {
                begin = root.transform.TransformPoint(root.Positions[i]),
                end = root.transform.TransformPoint(root.Positions[i + 1])
            };
            directions.Add(trunk);
        }
        if (root.Branches.Length > 0)
        {
            foreach (var branch in root.Branches)
            {
                directions.AddRange(ExtractDirections(branch));
            }
        }
        return directions.ToArray();
    }

    public IEnumerator Build()
    {
        Debug.Log("Building placenta");
        yield return Generate();
        yield return StoreDirections();
        CalculateIntersections();
        if (intersections.Count < 10)
        {
            GeneratePatches();
        }
        Debug.Log($"Intersections: {intersections.Count}");
    }

    public void DestroyTree()
    {
        intersections.Clear();
        ends.Clear();
        patches.Clear();
        if(firstArterieContainer)
            Destroy(firstArterieContainer);
        if(secondArterieContainer)
            Destroy(secondArterieContainer);
        if(firstVeinContainer)
            Destroy(firstVeinContainer);
        if(secondVeinContainer)
            Destroy(secondVeinContainer);
        if (patchesContainer)
            Destroy(patchesContainer);
    }

    private void ProjectPatches(float veinNormalOffset, float arterieNormalOffset)
    {
        foreach(var patch in patches)
        {
            patch.ProjectPoints(veinNormalOffset,arterieNormalOffset);
        }
    }

    private void ProjectRoot(ProceduralRoots root)
    {
        foreach(var branch in root.Branches)
        {
            branch.NormalOffset = root.NormalOffset;
            branch.ProjectPoints();
            ProjectRoot(branch);
        }
    }

    public IEnumerator ProjectPoints()
    {
        Ray ray;
        RaycastHit hit;
        int positionIndex;
        Vector3 placentaHeight = Vector3.up * _placenta.transform.position.y;

        positionIndex = 0;
        foreach(var point in intersections.ToArray())
        {
            ray = new Ray(point.position + placentaHeight + Vector3.up * _Yoffset, Vector3.down);
            if(Physics.Raycast(ray,out hit,_maxDistance,_projectionMask))
            {
                Vector3 projectPoint = hit.point;
                intersections[positionIndex] = new FetoscopyTargetPointData
                {
                    position = projectPoint,
                    type = point.type
                };
            }
            positionIndex++;
            yield return new WaitForEndOfFrame();
        }

        float veinNormalOffset = 0.001f;
        float arterieNormalOffset = 0.002f;

        firstVein.NormalOffset = veinNormalOffset;
        firstVein.ProjectPoints();
        ProjectRoot(firstVein);
        yield return new WaitForEndOfFrame();
        firstArterie.NormalOffset = arterieNormalOffset;
        firstArterie.ProjectPoints();
        ProjectRoot(firstArterie);
        yield return new WaitForEndOfFrame();
        secondVein.NormalOffset = veinNormalOffset;
        secondVein.ProjectPoints();
        ProjectRoot(secondVein);
        yield return new WaitForEndOfFrame();
        secondArterie.NormalOffset = arterieNormalOffset;
        secondArterie.ProjectPoints();
        ProjectRoot(secondArterie);
        yield return new WaitForEndOfFrame();
        ProjectPatches(veinNormalOffset,arterieNormalOffset);
        yield return new WaitForEndOfFrame();
    }
}
