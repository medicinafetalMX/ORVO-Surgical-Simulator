using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastObjectSCE : SimulationCustomEvent
{
    [SerializeField] Transform _raycastPointer;
    [SerializeField] GameObject _objectToRaycast;
    [SerializeField] float _raycastTime;
    [SerializeField] float _watchTime;

    private float timer;
    private float watchTimer;
    private bool isRaycasting = false;
    private bool isWatchingObject = false;

    private void OnDrawGizmosSelected()
    {
        if(_raycastPointer && _objectToRaycast)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _raycastPointer.position);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_objectToRaycast.transform.position, _raycastPointer.position);
        }
    }

    private void Update()
    {
        if (isRaycasting)
        {
            timer += Time.deltaTime;
            if (timer >= _raycastTime)
            {
                ShotRaycast();
                timer = 0;
            }
            if (isWatchingObject)
                watchTimer += Time.deltaTime;
            if(watchTimer>= _watchTime && isRaycasting)
            {
                isRaycasting = false;
                watchTimer = 0;
                End();
            }
        }
    }

    private void ShotRaycast()
    {
        Ray ray = new Ray(_raycastPointer.position, _raycastPointer.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider && hit.collider.gameObject == _objectToRaycast)
            {
                isWatchingObject = true;
            }
            else
            {
                isWatchingObject = false;
            }
        }
    }

    public override void End()
    {
        OnEventComplete?.Invoke();
    }

    public override void ForceEnd()
    {
        isRaycasting = false;
    }

    public override void Init()
    {
        isRaycasting = true;
    }
}
