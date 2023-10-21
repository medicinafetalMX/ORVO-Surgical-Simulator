using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FetoscopyIndicatorScreen : MonoBehaviour
{
    [SerializeField] Image _arterieArterieIndicator;
    [SerializeField] Image _veinArterieIndicator;
    [SerializeField] Image _veinVeinIndicator;
    [SerializeField] Transform _indicatorsHandler;
    [SerializeField] FetoscopyTarget[] _targets;
    [SerializeField] FetoscopyUIScreen _uiScreen;
    [SerializeField] Camera _fetoscopyCam;

    List<Image> indicators = new List<Image>();

    public void InstantiateIndicators()
    {
        for (int i = 0; i < indicators.Count; i++)
        {
            Destroy(indicators[i]);
        }
        indicators.Clear();
        foreach(var target in _targets) 
        {
            Image indicator = null;
            switch (target.type)
            {
                case CirculationIntersectionType.ArterieArterie:
                    indicator = Instantiate(_arterieArterieIndicator, _indicatorsHandler);
                    break;
                case CirculationIntersectionType.ArterieVein:
                    indicator = Instantiate(_veinArterieIndicator, _indicatorsHandler);
                    break;
                case CirculationIntersectionType.VeinVein:
                    indicator = Instantiate(_veinVeinIndicator, _indicatorsHandler);
                    break;
                default:
                    break;
            }
            indicators.Add(indicator);
            indicator.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        for (int i = 0; i < indicators.Count; i++)
        {
            Vector3 screenPos = _fetoscopyCam.WorldToScreenPoint(_targets[i].transform.position);
            indicators[i].fillAmount = _targets[i].life;
            indicators[i].rectTransform.anchoredPosition= screenPos - new Vector3(1280f/2,720f/2);
        }
    }

    public void CleanTargets()
    {
        _targets = new FetoscopyTarget[0];
        for (int i = 0; i < indicators.Count; i++)
        {
            Destroy(indicators[i].gameObject);
        }
        indicators.Clear();
    }

    public void SetFetoscopyTargets(FetoscopyTarget[] fetoscopyTargets)
    {
        _targets = fetoscopyTargets;
        InstantiateIndicators();
    }

    public void ReportTargetKill()
    {
        SurgeryMechanic.Instance.IncreaseScore();
        _uiScreen.SetTargetAmounts(_targets);
        if(_uiScreen.RemainingTargets == 0)
        {
            SurgeryMechanic.Instance.EndGame();
        }
    }

    public void SetFetoscopyCamera(Camera cam) { _fetoscopyCam = cam; }
}
