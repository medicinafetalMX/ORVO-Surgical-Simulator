using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCut : TaskData
{
    [SerializeField] BisturiLeverController _bisturiLevelController;
    [SerializeField] PatientRotatorAnimator _patientRotator;
    private IEnumerator WaitForCutClose()
    {
        yield return new WaitForSeconds(1);
        _bisturiLevelController.CloseWound();
        yield return new WaitForSeconds(2.5f);
        _patientRotator.Rotate("Top");
        CompleteTask();
    }
    public override void Init()
    {
        base.Init();
        StartCoroutine(WaitForCutClose());
    }
}
