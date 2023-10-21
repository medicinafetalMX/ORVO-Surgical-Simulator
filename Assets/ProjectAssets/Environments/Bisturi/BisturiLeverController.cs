using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
public class BisturiLeverController : MonoBehaviour
{
    public UnityAction OnCutFinished;

    public Transform Bisturi => _bisturi;

    [SerializeField] WoundController _woundController;
    [SerializeField] Transform _lever;
    [SerializeField] Transform _center;
    [SerializeField] Transform _bisturi;
    [SerializeField] Transform _wound;
    [SerializeField] float _maxAngle;
    [SerializeField] HingeJoint _hingeJoint;
    [SerializeField] Rigidbody _body;

    private float lastAmount = 0;
    private bool isCutFinished = false;
    private bool isCutting =true;
    private float radius;
    private Vector3 woundOffset;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_center.position, radius);
    }

    private void Start()
    {
        radius = (_bisturi.position - _center.position).magnitude;
        woundOffset = _wound.position - _bisturi.position;
    }
    private void Update()
    {
        if (isCutting)
        {
            CalculateCutAmount();
        }
    }
    private void CalculateCutAmount()
    {
        float leverAngle = _lever.localEulerAngles.x;
        if (leverAngle > 180f)
            leverAngle = 360 - leverAngle;
        float amount = Mathf.Clamp(leverAngle / _maxAngle,0,1);
        if (amount > lastAmount)
        {
            lastAmount = amount;
            _woundController.Amount = amount * 100;
        }
        if (amount >= 0.9f && !isCutFinished)
        {
            isCutFinished = true;
            OnCutFinished?.Invoke();
        }
    }

    public void CalculateRadius(Vector3 cutPoint)
    {
        radius = (cutPoint - _center.position).magnitude;
        _maxAngle = 360 * (0.01f / (2 * Mathf.PI * radius));
        
        JointLimits limits = _hingeJoint.limits;
        limits.min = -_maxAngle;
        _hingeJoint.limits = limits;

        _bisturi.localPosition = Vector3.up * radius;
        _wound.localPosition = Vector3.up * radius + woundOffset;
        _lever.transform.localRotation = Quaternion.identity;
    }

    public void CloseWound()
    {
        isCutting = false;
        float amount = 1;
        DOTween.To(() => amount, x => amount = x, 0, 2)
            .OnUpdate(() =>
            {
                _woundController.Amount = amount * 100;
            });
    }

    public void Reset()
    {
        isCutting = true;
        _bisturi.gameObject.SetActive(true);
        _woundController.Amount = 0;
        lastAmount = 0;
    }
}
