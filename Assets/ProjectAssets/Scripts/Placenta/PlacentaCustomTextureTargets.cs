using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(PlacentaCustomTextureTargets))]
public class PlacentaCustomTextureTargetsEditor : Editor
{
    private PlacentaCustomTextureTargets placenta;
    private int placentaIndex;
    private void OnEnable()
    {
        placenta = target as PlacentaCustomTextureTargets;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Placenta index");
        placentaIndex = EditorGUILayout.IntField(placentaIndex);
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Set placenta"))
            placenta.SetPlacenta(placentaIndex);

    }
}
#endif
public class PlacentaCustomTextureTargets : MonoBehaviour
{
    [System.Serializable]
    public class PlacentaTextureAndTargets
    {
        public Texture2D Texture;
        public Texture2D NormalTexture;
        public FetoscopyTarget[] Targets;
    }
    [SerializeField] FetoscopyOrgan _placenta;
    [SerializeField] Material _placentaMaterial;
    [SerializeField] List<PlacentaTextureAndTargets> placentas = new List<PlacentaTextureAndTargets>();
    private void Start()
    {
        _placentaMaterial.EnableKeyword("_NORMALMAP");
    }
    public void LoadRandomPlacenta()
    {
        int randomSelection = Random.Range(0, placentas.Count);
        SetPlacenta(randomSelection);
    }

    public void SetPlacenta(int placentaIndex)
    {
        if (placentaIndex >= placentas.Count)
            return;
        Debug.Log("Playing level " + placentaIndex);
        PlacentaTextureAndTargets placentaLevel = placentas[placentaIndex];
        _placentaMaterial.SetTexture("_MainTex", placentaLevel.Texture);
        _placentaMaterial.SetTexture("_BumpMap", placentaLevel.NormalTexture);
        _placenta.CleanTargets();
        _placenta.SetTargets(placentaLevel.Targets);

        _placenta.ResetTargets();
        foreach (var target in placentaLevel.Targets)
            target.gameObject.SetActive(true);
    }
}
