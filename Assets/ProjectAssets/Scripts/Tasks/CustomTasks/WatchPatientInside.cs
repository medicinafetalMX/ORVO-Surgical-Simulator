using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchPatientInside : TaskData
{
    [SerializeField] PatientInsideVisibility _patientInsideVisibility;
    [SerializeField] FlyingTool _transducerFlyingTool;
    [SerializeField] Transform _indicatorsContainer;
    [SerializeField] Image _mainScreen;
    [SerializeField] Image _sideScreenTv;

    public override void Init()
    {
        base.Init();
        _patientInsideVisibility.MakeVisible();
        VerifyDifficultyAndToggleIndicators();
        TurnOffScreens();
        StartCoroutine(WaitInvisibility());
    }

    public override void CompleteTask()
    {
        base.CompleteTask();
    }

    private IEnumerator WaitInvisibility()
    {
        yield return new WaitForSeconds(3);
        _transducerFlyingTool.Unlock();
        CompleteTask();
    }

    private void VerifyDifficultyAndToggleIndicators()
    {
        DifficultyState difficultyState = FindObjectOfType<DifficultyState>();
        if (difficultyState == null) { return; }

        float alphaValue = difficultyState.SessionDifficulty == DifficultyMode.Facil ? 1f : 0f;
        foreach (Transform child in _indicatorsContainer)
        {
            Image indicatorImage = child.gameObject.GetComponent<Image>();
            if (indicatorImage != null)
            {
                indicatorImage.color = new Color(indicatorImage.color.r,
                    indicatorImage.color.g, indicatorImage.color.b, alphaValue);

            }
        }
    }

    private void TurnOffScreens()
    {
        _mainScreen.color = new Color(_mainScreen.color.r,
            _mainScreen.color.g, _mainScreen.color.b, 0f);
        _sideScreenTv.color = new Color(_sideScreenTv.color.r,
            _sideScreenTv.color.g, _sideScreenTv.color.b, 0f);
    }
}
