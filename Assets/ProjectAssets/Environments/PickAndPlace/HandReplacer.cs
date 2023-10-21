using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandReplacer : MonoBehaviour
{
    [SerializeField] bool _hideReferenceWhenGrabbed = true;
    [SerializeField] GameObject _handReplace;
    [SerializeField] GameObject _reference;
    [SerializeField] GrabbedObject _grabConfiguration;
    public GameObject Grab(HandSides side,Transform hand)
    {
        _grabConfiguration.Grab(side);
        _handReplace.SetActive(true);
        _handReplace.transform.SetParent(hand);
        _handReplace.transform.localPosition = Vector3.zero;
        _handReplace.transform.localRotation = Quaternion.identity;
        if (_hideReferenceWhenGrabbed)
            _reference.SetActive(false);
        return _handReplace;
    }

    public void Release()
    {
        _handReplace.SetActive(false);
        _handReplace.transform.SetParent(null);
        _reference.SetActive(true);
    }
}
