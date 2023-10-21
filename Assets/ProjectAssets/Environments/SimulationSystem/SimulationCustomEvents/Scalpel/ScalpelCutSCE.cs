using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalpelCutSCE : SimulationCustomEvent
{
    [SerializeField] GameObject _lever;
    [SerializeField] GameObject _realScalpel;
    [SerializeField] Transform _fakeScalpel;
    [SerializeField] float _leverTargetRotation;
    private bool isCheckingRotation = false;

    private void Update()
    {
        if (isCheckingRotation)
        {
            float zAngle = _lever.transform.rotation.eulerAngles.z;
            if (zAngle >= _leverTargetRotation)
            {
                End();
            }
        }
    }

    private void SwitchScalpels()
    {
        _realScalpel.transform.position = _fakeScalpel.transform.position;
        _lever.SetActive(false);
        _realScalpel.SetActive(true);
    }

    public override void End()
    {
        isCheckingRotation = false;
        SwitchScalpels();
        OnEventComplete?.Invoke();
    }

    public override void ForceEnd()
    {
        _lever.transform.rotation = Quaternion.Euler(0, 0, _leverTargetRotation);
        SwitchScalpels();
        isCheckingRotation = false;
    }

    public override void Init()
    {
        PlayerHandsManager.Instance.ReleaseObject(_realScalpel);
        _realScalpel.SetActive(false);
        _lever.SetActive(true);
        isCheckingRotation = true;
    }
}
