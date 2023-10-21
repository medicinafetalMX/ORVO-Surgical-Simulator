using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public Vector3 LastShotPos { get; set; }
    public int Score => _score;
    [SerializeField] int _score;
    [SerializeField] string _userkey;
    [SerializeField] ScoreUI _scoreUi;
    [SerializeField] FetoscopyUIScreen _uiScreen;
    public void ResetScore()
    {
        _score = 0;
        _scoreUi.SetScore(0);
        _uiScreen.CloseResults();
    }

    public void IncreaseScore(int amount)
    {
        _score += amount;
        _scoreUi.IncreaseScore(amount,_score);
    }
    public void DecreaseScore(int amount)
    {
        _score -= amount;
        _scoreUi.DecreaseScore(amount,_score);
    }

    public void ShowResults()
    {
        _uiScreen.ShowResults();
    }

    public void ShowHomeReturn()
    {
        _uiScreen.ShowHomeButton();
    }
}
