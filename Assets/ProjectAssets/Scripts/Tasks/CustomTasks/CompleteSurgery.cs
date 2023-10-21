using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteSurgery : TaskData
{
    [SerializeField] GameObject _fetoscopeStraight;
    [SerializeField] GameObject _fetoscopeCurved;
    [SerializeField] GameObject _cook;
    [SerializeField] GameObject _vaina;
    [SerializeField] GameObject _laparoscopeSurgeryStraight;
    [SerializeField] GameObject _laparoscopeStraightBallController;
    [SerializeField] GameObject _laparoscopeSurgeryCurved;
    [SerializeField] GameObject _laparoscopeCurvedBallController;
    [SerializeField] SurgeryTimer _surgeryTimer;

    private IEnumerator HideLaparoscopeSurgery()
    {
        yield return new WaitForSeconds(0.5f);
        _laparoscopeSurgeryStraight.SetActive(false);
        _laparoscopeSurgeryCurved.SetActive(false);
    }
    public override void Init()
    {
        base.Init();
        _surgeryTimer.StartChronometer();
        SurgeryMechanic.Instance.OnGameEnd += CompleteTask;
    }
    public override void CompleteTask()
    {
        SurgeryMechanic.Instance.OnGameEnd -= CompleteTask;
        PlayerHandsManager.Instance.ReleaseObject(_laparoscopeStraightBallController);
        PlayerHandsManager.Instance.ReleaseObject(_laparoscopeCurvedBallController);
        _vaina.SetActive(true);
        _cook.SetActive(true);
        _fetoscopeStraight.SetActive(true);
        _fetoscopeCurved.SetActive(true);
        _laparoscopeSurgeryStraight.SetActive(false);
        _laparoscopeSurgeryCurved.SetActive(false);
        _surgeryTimer.StopChronometer();
        _surgeryTimer.WriteTime();
        StartCoroutine(HideLaparoscopeSurgery());
        base.CompleteTask();
    }
}
