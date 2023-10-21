using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(RandomForceApplier))]
public class RandomForceApplierEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Apply force"))
            (target as RandomForceApplier).ApplyForce();
    }
}
#endif
public class RandomForceApplier : MonoBehaviour
{
    [SerializeField] float _force;
    [SerializeField] float _angularForce;
    private Rigidbody body;
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    public void ApplyForce()
    {
        body.AddForce(new Vector3(Random.value, Random.value, Random.value).normalized*_force,ForceMode.Impulse);
        body.AddTorque(new Vector3(Random.value, Random.value, Random.value).normalized * _angularForce, ForceMode.Impulse);
    }
}
