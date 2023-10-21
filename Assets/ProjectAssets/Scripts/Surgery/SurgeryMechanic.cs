using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(SurgeryMechanic))]
public class SurgeryMechanicEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("End game"))
            (target as SurgeryMechanic).EndGame();
    }
}
#endif

public class SurgeryMechanic : MonoBehaviour
{
    public UnityAction OnGameEnd;
    public int Faults => faults;
    public int Hits => hits;
    public int FaultScore => _faultScore;
    public int IncreseScore => _increaseScore;
    public int TimeMaxScore => _timeMaxScore;
    public static SurgeryMechanic Instance;
    [SerializeField] int _faultScore;
    [SerializeField] int _increaseScore;
    [SerializeField] int _timeMaxScore;
    [SerializeField] ScoreManager _scoreManager;

    private int faults, hits;

    private void Awake()
    {
        Instance = this;
    }

    public void DecreaseScore()
    {
        faults++;
        _scoreManager.DecreaseScore(_faultScore);
    }

    public void IncreaseScore()
    {
        hits++;
        _scoreManager.IncreaseScore(_increaseScore);
    }

    public void EndGame()
    {
        OnGameEnd?.Invoke();
        _scoreManager.ShowResults();
    }

    public void EndSimulation()
    {
        _scoreManager.ShowHomeReturn();
    }

    public void Reset()
    {
        faults = hits = 0;
    }
}
