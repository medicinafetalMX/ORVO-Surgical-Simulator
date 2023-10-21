using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTransducer : SimulationCustomEvent
{
    [SerializeField] FlyingTool _flyingTool;
    [SerializeField] Collider _transducerCollider;
    [SerializeField] Rigidbody _transducerBody;
    [SerializeField] Transform _transducerTransform;
    [SerializeField] Transform _transducerTarget;

    public override void End()
    {
        OnEventComplete?.Invoke();
    }

    public override void ForceEnd()
    {
    }

    public override void Init()
    {
        _flyingTool.enabled = false;
        _transducerBody.isKinematic = true;
        _transducerCollider.enabled = false;
        _transducerTransform.transform.position = _transducerTarget.transform.position;
        _transducerTransform.transform.rotation = _transducerTarget.transform.rotation;
    }
}
