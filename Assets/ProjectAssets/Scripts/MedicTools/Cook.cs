using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cook : MonoBehaviour
{
    [Header("Trocar")]
    [SerializeField] GameObject _staticTrocar;
    [SerializeField] GameObject _plasticTrocar;
    [SerializeField] GameObject _stababbleTrocar;
    [SerializeField] StabAnimationVRController _trocarStab;
    [SerializeField] FlyingTool _trocarFlyingTool;

    [Header("Cook")]
    [SerializeField] Collider[] _colliders;

    private SelectionMaterialChanger selectionMaterialChanger;
    private StabAnimationVRController stab;
    private FlyingTool flyingTool;
    private bool isStabbed = false;
    private bool isTrocarEnabled = false;

    private void Awake()
    {
        flyingTool = GetComponent<FlyingTool>();
        stab = GetComponent<StabAnimationVRController>();
        selectionMaterialChanger = GetComponent<SelectionMaterialChanger>();
    }

    private void Start()
    {
        _stababbleTrocar.SetActive(false);
        _plasticTrocar.SetActive(false);
        stab.OnStab += CheckStab;
        stab.OnUnstab += CheckUnstab;
        Reset();
    }

    private void OnDestroy()
    {
        stab.OnUnstab -= CheckStab;
        stab.OnUnstab -= CheckUnstab;
    }

    private void Update()
    {
        if (isStabbed)
        {
            if (stab.StabAmount >= 0.9f && !isTrocarEnabled)
            {
                isTrocarEnabled = true;
                StabTrocar();
            }
        }
    }

    private void CheckStab()
    {
        isStabbed = true;
    }

    private void CheckUnstab()
    {
        isStabbed = false;
    }

    private void StabTrocar()
    {
        _staticTrocar.SetActive(false);
        
        foreach (var collider in _colliders)
            collider.enabled = false;

        stab.enabled = false;
        flyingTool.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        selectionMaterialChanger.enabled = false;

        _trocarFlyingTool.IsTargetFollowAuth = false;
        _trocarFlyingTool.Target = flyingTool.Target;

        _stababbleTrocar.transform.position = _staticTrocar.transform.position;
        _stababbleTrocar.transform.rotation = _staticTrocar.transform.rotation;
        _trocarStab.FollowTarget = stab.FollowTarget;
        _trocarStab.SetStabPoint(stab.StabPoint);
        _trocarStab.Stab();
        _stababbleTrocar.SetActive(true);

    }

    public void EnablePlasticTrocar()
    {
        _staticTrocar.SetActive(false);
        _plasticTrocar.SetActive(true);
        _staticTrocar.SetActive(false);
        stab.enabled = true;
        flyingTool.enabled = true;
        selectionMaterialChanger.enabled = true;
        foreach (var collider in _colliders)
            collider.enabled = true;
    }

    public void Reset()
    {
        isTrocarEnabled = false;
        isStabbed = false;
        _stababbleTrocar.GetComponent<Rigidbody>().isKinematic = true;
        _stababbleTrocar.transform.position = _staticTrocar.transform.position;
        _stababbleTrocar.transform.rotation = _staticTrocar.transform.rotation;
        _stababbleTrocar.SetActive(false);
        _staticTrocar.SetActive(true);
    }
}
