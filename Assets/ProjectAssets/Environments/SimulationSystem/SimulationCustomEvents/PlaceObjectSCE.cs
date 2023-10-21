using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjectSCE : SimulationCustomEvent
{
    [SerializeField] GameObject _ghostObject;
    [SerializeField] GameObject _objectToPlace;
    [SerializeField] float _minDistance;
    [SerializeField] float _minAngle;
    private bool isPlacingObject = false;
    private bool alreadyEnded = false;

    private void OnDrawGizmosSelected()
    {
        if (_objectToPlace && _ghostObject)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _objectToPlace.transform.position);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_objectToPlace.transform.position, _ghostObject.transform.position);
        }
    }

    private void Start()
    {
        _ghostObject.SetActive(false);
    }

    private void Update()
    {
        if(isPlacingObject)
            CheckObjectDistance();
    }

    private void CheckObjectDistance()
    {
        float distance = Vector3.Distance(_ghostObject.transform.position, _objectToPlace.transform.position);
        float angle = Quaternion.Angle(_ghostObject.transform.rotation, _objectToPlace.transform.rotation);
        if (distance <= _minDistance && angle <= _minAngle && !alreadyEnded)
        {
            alreadyEnded = true;
            End();
        }
    }


    public override void End()
    {
        _ghostObject.SetActive(false);
        isPlacingObject = false;
        OnEventComplete?.Invoke();
    }

    public override void ForceEnd()
    {
        isPlacingObject = false;
        _ghostObject.SetActive(false);
        _objectToPlace.transform.position = _ghostObject.transform.position;
        _objectToPlace.transform.rotation = _ghostObject.transform.rotation;
    }

    public override void Init()
    {
        _ghostObject.SetActive(true);
        isPlacingObject = true;
    }
}
