using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(PlacentaBaker))]
public class PlacentaBakerEditor : Editor
{
    private PlacentaBaker baker;
    private void OnEnable()
    {
        baker = target as PlacentaBaker;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Bake"))
            baker.Bake();
    }
}
#endif

public class PlacentaBaker : MonoBehaviour
{
    [SerializeField] PlacentaPositioner _placentaPositioner;
    [SerializeField] RootsGenerator _rootsGenerator;
    [SerializeField] Transform _placenta;
    [SerializeField] FetoscopyController _fetoscope;
    [SerializeField] FetoscopyOrgan _targetsGenerator;
    [SerializeField] FetoscopyIndicatorScreen _screen;
    [SerializeField] FetoscopyUIScreen _uiScreen;
    [SerializeField] BabysPoser _babysPoser;
    [SerializeField] PatientInsideVisibility _patientVisibility;
    [SerializeField] PlacentaCustomTextureTargets _customLevels;

    private bool lastVisibility;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        Bake();
    }

    private void CleanPlacenta()
    {
        Debug.Log("Cleaning previous placenta");
        _rootsGenerator.DestroyTree();
        _targetsGenerator.CleanTargets();
        _screen.CleanTargets();
        _uiScreen.ClearTargets();
    }

    public void BakeProcess()
    {
        lastVisibility = _patientVisibility.IsVisible;
        _patientVisibility.MakeVisible();
        CleanPlacenta();
        Debug.Log("Cleaning coords and repositioning placenta");
        _babysPoser.CleanCords();

        ///Procedural generation
        // Procedural Arteries & Veins generation 
        /*Debug.Log("Building roots");
         yield return _rootsGenerator.Build();
         Debug.Log("Projecting root points");
         yield return _rootsGenerator.ProjectPoints();*/

        //Targets based on procedural intersections
        /* Debug.Log("Creating targets");
         _targetsGenerator.SetTargetsData(_rootsGenerator.Intersections);
         _targetsGenerator.LoadTargets();
         Debug.Log("Setting targets on screen");*/

        ///Custom targets based on custom placenta textures
        _customLevels.LoadRandomPlacenta();

        _screen.SetFetoscopyTargets(_targetsGenerator.OptimizedTargets);
        _uiScreen.SetTargetAmounts(_targetsGenerator.OptimizedTargets);
        Debug.Log("Placing placenta in random position");
        _placentaPositioner.RandomPlace();
        _babysPoser.Pose();
        Debug.Log("Placenta baked with " + _targetsGenerator.OptimizedTargets.Length + " intersections");
        if (!lastVisibility)
            _patientVisibility.MakeInvisible();
    }
    public void Bake()
    {
        Debug.Log("Baking placenta");
        BakeProcess();
    }
}
