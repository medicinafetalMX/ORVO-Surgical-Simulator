using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(ObjectTransformReplicator))]
public class ObjectTransformReplicatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Replicate transform"))
        {
            (target as ObjectTransformReplicator).ReplicateTransform();
        }
    }
}
#endif
public class ObjectTransformReplicator : MonoBehaviour
{
    public void ReplicateTransform()
    {
        var replica = new GameObject(name + "_transformReplica");
        replica.transform.position = transform.position;
        replica.transform.rotation = transform.rotation;
        replica.transform.localScale = transform.localScale;
    }
}
