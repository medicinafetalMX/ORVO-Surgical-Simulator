using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoundController : MonoBehaviour
{
    [Range(0, 100)] public float Amount;
    [SerializeField] Vector2 _shape1Limits;
    [SerializeField] Vector2 _shape2Limits;
    [SerializeField] Vector2 _shape3Limits;
    [SerializeField] Vector2 _shape4Limits;
    [SerializeField] Vector2 _shape5Limits;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }
    private void Update()
    {
        skinnedMeshRenderer.SetBlendShapeWeight(1, ClampLimits(_shape1Limits));
        skinnedMeshRenderer.SetBlendShapeWeight(2, ClampLimits(_shape2Limits));
        skinnedMeshRenderer.SetBlendShapeWeight(3, ClampLimits(_shape3Limits));
        skinnedMeshRenderer.SetBlendShapeWeight(4, ClampLimits(_shape4Limits));
        skinnedMeshRenderer.SetBlendShapeWeight(5, ClampLimits(_shape5Limits));
    }
    private float ClampLimits(Vector2 limits)
    {
        float amount = Mathf.Clamp(Amount, limits.x, limits.y);
        amount -= limits.x;
        float limit = limits.y - limits.x;
        return (amount / limit) * 100;
    }

}
