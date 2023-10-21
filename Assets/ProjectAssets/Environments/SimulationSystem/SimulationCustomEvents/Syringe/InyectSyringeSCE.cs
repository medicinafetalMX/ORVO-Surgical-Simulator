using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InyectSyringeSCE : SimulationCustomEvent
{
    [SerializeField] Animator _syringeAnimation;
    [SerializeField] FlyingTool _flyingTool;
    [SerializeField] InputActionReference _injectInput;
    
    private bool isCheckingSyringeGrab = false;

    private void Inject(InputAction.CallbackContext obj)
    {
        if(isCheckingSyringeGrab && _flyingTool.IsGrabbed)
        {
            _syringeAnimation.SetBool("Inject", true);
            StartCoroutine(WaitEndOfAnimation());
        }
    }

    private IEnumerator WaitEndOfAnimation()
    {
        yield return new WaitForSeconds(3);
        End();
    }

    public override void End()
    {
        isCheckingSyringeGrab = false;
        OnEventComplete?.Invoke();
    }

    public override void ForceEnd()
    {
        isCheckingSyringeGrab = false;
        _syringeAnimation.SetBool("Inject",true);
    }

    public override void Init()
    {
        isCheckingSyringeGrab = true;
        _injectInput.action.performed += Inject;
    }
}
