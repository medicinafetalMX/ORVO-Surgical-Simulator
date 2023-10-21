using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charola : MonoBehaviour
{
    [SerializeField] FlyingTool[] _tools;

    private List<GameObject> pivots = new List<GameObject>();
    private void Awake()
    {
        foreach(var tool in _tools)
        {
            var pivot = new GameObject(tool.name + "_pivot");
            pivot.transform.SetParent(transform);
            pivot.transform.position = tool.transform.position;
            pivot.transform.rotation = tool.transform.rotation;
            tool.SetFollowPose(pivot.transform);
            pivots.Add(pivot);
        }
    }
}
