using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FetoscopyUIScreen : MonoBehaviour
{
    public int RemainingTargets { get { return axa + axv + vxv; } }
    [SerializeField] TextMeshProUGUI _arteriesXArteriesAmount;
    [SerializeField] TextMeshProUGUI _arteriesXVeinsAmount;
    [SerializeField] TextMeshProUGUI _veinsXVeinsAmount;
    [SerializeField] UIChronometer _chronometer;
    [SerializeField] FetoscopyUIResults _results;

    private int axa, axv, vxv;

    private void Start()
    {
        _chronometer.StartChronometer();
    }
    public void ClearTargets()
    {
        axa = 0;
        axv = 0;
        vxv = 0;
        _chronometer.ChronoTime = 0;
        _results.Close();
    }

    public void SetTargetAmounts(FetoscopyTarget[] targets)
    {
        axa = 0;
        axv = 0;
        vxv = 0;
        foreach (var target in targets)
        {
            if (target.life > 0)
            {
                if (target.type == CirculationIntersectionType.ArterieArterie)
                    axa++;
                else if (target.type == CirculationIntersectionType.ArterieVein)
                    axv++;
                else
                    vxv++;
            }
        }
        _arteriesXArteriesAmount.text = axa.ToString();
        _arteriesXVeinsAmount.text = axv.ToString();
        _veinsXVeinsAmount.text = vxv.ToString();
    }

    public void ShowResults()
    {
        _results.CalculateScore();
        _results.Open();
    }

    public void CloseResults()
    {
        _results.Close();
    }

    public void ShowHomeButton()
    {
        _results.ReturnHome();
    }
}
