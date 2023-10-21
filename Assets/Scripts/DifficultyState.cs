using System;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DifficultyState : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    private DifficultyMode _sessionDifficulty;

    private void Start()
    {
        UpdateDifficultyState(DifficultyMode.Facil.ToString());
    }

    public void UpdateDifficultyState(string difficulty)
    {
        DifficultyMode difficultyMode =
            (DifficultyMode)Enum.Parse(typeof(DifficultyMode), difficulty);
        _sessionDifficulty = difficultyMode;
        if (_label != null) { _label.text = difficulty; }
            
    }

    public DifficultyMode SessionDifficulty { get => _sessionDifficulty; set => _sessionDifficulty = value; }
}

[Serializable]
public enum DifficultyMode
{
    Facil,
    Avanzado
}