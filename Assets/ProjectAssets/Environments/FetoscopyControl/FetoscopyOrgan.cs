using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetoscopyOrgan : MonoBehaviour
{
    public FetoscopyTarget[] OptimizedTargets => _targets.ToArray();

    [SerializeField] float _targetsRadius = 0.01f;
    [SerializeField] float _targetsDamageRadius = 0.005f;
    [SerializeField] FetoscopyIndicatorScreen _screen;
    [SerializeField] FetoscopyTarget _targetPrefab;
    [SerializeField] private List<FetoscopyTarget> _targets = new List<FetoscopyTarget>();

   
    private bool isDamagingOrgan = false;
    private float damageTimer = 0;
    private float waitTimer = 0;
    private const float damageMaxTime = 1f;
    private const float waitLaserTime = 0.1f;
    private FetoscopyTarget currentTarget;
    private List<FetoscopyTargetPointData> targetsData = new List<FetoscopyTargetPointData>();

    private void Update()
    {
        if (currentTarget)
        {
            if (currentTarget.IsAlive)
                currentTarget.Damage();
            else
            {
                _screen.ReportTargetKill();
                currentTarget = null;
            }
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitLaserTime)
            {
                currentTarget.StopDamage();
                if (!currentTarget.IsAlive)
                {
                    _screen.ReportTargetKill();
                }
                currentTarget = null;
            }
        } 
        else if (isDamagingOrgan)
        {
            damageTimer += Time.deltaTime;
            if(damageTimer>= damageMaxTime)
            {
                SurgeryMechanic.Instance.DecreaseScore();
                damageTimer = 0;
            }
        }
    }

    private void OptimizeTargets()
    {
        List<FetoscopyTargetPointData> toRemove = new List<FetoscopyTargetPointData>();
        for (int i = 0; i < targetsData.Count; i++)
        {
            FetoscopyTargetPointData currentData = targetsData[i];
            for (int j = 0; j < targetsData.Count; j++)
            {
                if (i == j)
                    continue;
                FetoscopyTargetPointData toCompare = targetsData[j];
                if (currentData.type == toCompare.type 
                    && Vector3.Distance(currentData.position, toCompare.position) <= _targetsRadius
                    && !toRemove.Contains(currentData))
                {
                    toRemove.Add(toCompare);
                }
            }
        }

        for (int i = 0; i < toRemove.Count; i++)
        {
            if (targetsData.Contains(toRemove[i]))
                targetsData.Remove(toRemove[i]);
        }

        toRemove.Clear();
    }

    public void Damage(Vector3 point)
    {
        waitTimer = 0;
        foreach(var target in _targets)
        {
            if (target.IsAlive && Vector3.Distance(target.transform.position, point) <= _targetsDamageRadius)
            {
                currentTarget = target;
                isDamagingOrgan = false;
                break;
            }
        }
        if(currentTarget == null)
        {
            isDamagingOrgan = true;
        }
    }

    public void StopDamage()
    {
        isDamagingOrgan = false;
    }

    public void SetTargets(FetoscopyTarget[] targetDatas)
    {
        _targets.AddRange(targetDatas);
    }

    public void SetTargetsData(FetoscopyTargetPointData[] targetDatas)
    {
        targetsData.AddRange(targetDatas);
    }

    public void LoadTargets()
    {
        OptimizeTargets();
        foreach(var targetData in targetsData)
        {
            var target = Instantiate(_targetPrefab);
            target.transform.position = targetData.position;
            target.type = targetData.type;
            target.transform.SetParent(transform);
            _targets.Add(target);
        }
    }

    public void CleanTargets()
    {
        foreach (var target in _targets)
            target.gameObject.SetActive(false);
        _targets.Clear();
        targetsData.Clear();
    }

    public void DestroyTargets()
    {
        for (int i = 0; i < _targets.Count; i++)
            Destroy(_targets[i].gameObject);
    }

    public void ResetTargets()
    {
        foreach (var target in _targets)
            target.life = 1;
    }
}
