using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(PlacentaPositioner))]
public class PlacentaPositionerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Random place"))
        {
            (target as PlacentaPositioner).RandomPlace();
        }
        if (GUILayout.Button("Default place"))
        {
            (target as PlacentaPositioner).DefaultPlace();
        }
    }
}
#endif
public class PlacentaPositioner : MonoBehaviour
{
    [SerializeField] Transform _placenta;
    [SerializeField] Transform[] _positions;
    [SerializeField] Transform _defaultPosition;
    public void RandomPlace()
    {
        int random = Random.Range(0, _positions.Length);
        Debug.Log(random);
        var selectedPos = _positions[random];
        _placenta.transform.position = selectedPos.position;
        _placenta.transform.rotation = selectedPos.rotation;
    }
    public void DefaultPlace()
    {
        _placenta.transform.position = _defaultPosition.position;
        _placenta.transform.rotation = _defaultPosition.rotation;
    }
}
