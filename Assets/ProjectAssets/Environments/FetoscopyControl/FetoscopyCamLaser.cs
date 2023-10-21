using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FetoscopyCamLaser : MonoBehaviour
{
    [SerializeField] Transform _localPositionOn;
    [SerializeField] Transform _localPositionOff;
    [SerializeField] Transform _camLaser;
    private void Start()
    {
        _camLaser.transform.localPosition = _localPositionOff.localPosition;
    }
    public void TurnOn()
    {
        _camLaser.DOLocalMove(_localPositionOn.localPosition, 1f);
    }

    public void TurnOff()
    {
        _camLaser.DOLocalMove(_localPositionOff.localPosition, 1f);
    }
}
