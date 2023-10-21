using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateSurgery : TaskData
{
    [SerializeField] GameObject _vaina;
    [SerializeField] GameObject _cook;
    [SerializeField] GameObject _fetoscopeStraight;
    [SerializeField] FetoscopeActivator _fetoscopeStraightActivator;
    [SerializeField] GameObject _fetoscopeCurved;
    [SerializeField] FetoscopeActivator _fetoscopeCurvedActivator;
    [SerializeField] Image _mainScreen;
    [SerializeField] Image _sideScreenTv;
    

    public override void Init()
    {
        base.Init();

        _fetoscopeStraightActivator.OnFetoscopeActivated += CompleteTask;
        _fetoscopeCurvedActivator.OnFetoscopeActivated += CompleteTask;
    }

    public override void CompleteTask()
    {
        _fetoscopeStraightActivator.OnFetoscopeActivated -= CompleteTask;
        _fetoscopeCurvedActivator.OnFetoscopeActivated -= CompleteTask;
        _vaina.SetActive(false);
        _cook.SetActive(false);
        _fetoscopeStraight.SetActive(false);
        _fetoscopeCurved.SetActive(false);
        TurnOnScreens();
        base.CompleteTask();
    }

    private void TurnOnScreens()
    {
        _mainScreen.color = new Color(_mainScreen.color.r,
            _mainScreen.color.g, _mainScreen.color.b, 1f);
        _sideScreenTv.color = new Color(_sideScreenTv.color.r,
            _sideScreenTv.color.g, _sideScreenTv.color.b, 1f);
    }
}
