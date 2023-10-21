using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollSetter : MonoBehaviour
{
    public float Roll;
    [SerializeField] Transform _target;
    private void Update()
    {
        ExtractRoll();
        _target.transform.rotation = Quaternion.Euler(Vector3.up * Roll);
    }

    private void ExtractRoll()
    {
        Quaternion rot = transform.rotation;
        Roll = Mathf.Atan2(2 * rot.w * rot.y - 2 * rot.x * rot.z, 1 - 2 * rot.y * rot.y - 2 * rot.z * rot.z) * Mathf.Rad2Deg;
    }
}
