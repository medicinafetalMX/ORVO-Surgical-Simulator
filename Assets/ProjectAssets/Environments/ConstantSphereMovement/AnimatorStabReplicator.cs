using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStabReplicator : MonoBehaviour
{
    [SerializeField] SphericalMovement _sphericalMovement;
    [SerializeField] Animator _animator;
    [SerializeField] string _animatorParameter;

    private void Update()
    {
        float stabAmount = _sphericalMovement.StabAmount;
        _animator.SetFloat(_animatorParameter, stabAmount);
    }

}
