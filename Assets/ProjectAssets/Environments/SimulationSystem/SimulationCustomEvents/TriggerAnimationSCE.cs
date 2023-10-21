using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimationSCE : SimulationCustomEvent
{
    [SerializeField] Animator _animator;
    [SerializeField] string _triggerName;

    private IEnumerator WaitForAnimationEnd()
    {
        float time = _animator.GetCurrentAnimatorStateInfo(0).length + _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        yield return new WaitForSeconds(time);
        End();
    }

    public override void End()
    {
        OnEventComplete?.Invoke();
    }

    public override void ForceEnd()
    {
        
    }

    public override void Init()
    {
        _animator.SetTrigger(_triggerName);
        StartCoroutine(WaitForAnimationEnd());
    }
}
