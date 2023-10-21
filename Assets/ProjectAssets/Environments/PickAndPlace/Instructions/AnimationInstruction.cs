using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationInstruction : Instruction
{
    [SerializeField] Animator _animator;
    [SerializeField] string _stateName;
    [SerializeField] float _startDelay;

    private IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(_startDelay);
        _animator.Play(_stateName);
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length + _animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        _animator.Play("Idle");
        Disconnect();
    }
    public override void Connect()
    {
        PlayerHandsManager.Instance.HideHand(HandSides.RightHand);
        StartCoroutine(PlayAnimation());
    }

    public override void Disconnect()
    {
        PlayerHandsManager.Instance.ShowHand(HandSides.RightHand);
    }
}
