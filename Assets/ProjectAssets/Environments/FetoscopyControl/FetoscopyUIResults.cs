using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Linq;
public class FetoscopyUIResults : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] UIChronometer _chronometer;
    [SerializeField] RectTransform _returnHomeButton;

    [Header("Player results")]
    [SerializeField] TextMeshProUGUI _time;
    [SerializeField] TextMeshProUGUI _timeScore;
    [SerializeField] TextMeshProUGUI _hits;
    [SerializeField] TextMeshProUGUI _hitsScore;
    [SerializeField] TextMeshProUGUI _faults;
    [SerializeField] TextMeshProUGUI _faultsScore;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] GameObject[] _ranks;

    [Header("Leaderboard")]
    [SerializeField] RectTransform _content;
    [SerializeField] GameObject _resultElementPrefab;
    [SerializeField] GameObject _playerResultElementPrefab;

    [Header("Animation")]
    [SerializeField] Ease _animEase = Ease.InSine;

    private int[] timeSegments = new int[] { 180, 300, 480, 600 };

    private List<FetoscopyResult> storedResults = new List<FetoscopyResult>()
    {
        new FetoscopyResult(){Username = "Andrés Aboytes",Score = 700},
        new FetoscopyResult(){Username = "Imanol Darán",Score = 600},
        new FetoscopyResult(){Username = "Alberto Delgado",Score = 500},
        new FetoscopyResult(){Username = "Rafita",Score = 400},
        new FetoscopyResult(){Username = "Jorge",Score = 300},
        new FetoscopyResult(){Username = "Isaac Klarum",Score = 200},
        new FetoscopyResult(){Username = "Winni UWU",Score = 100},
    };
    public struct FetoscopyResult
    {
        public string Username;
        public int Score;
    }

    private void Start()
    {
        transform.localScale = Vector3.zero;
        _returnHomeButton.gameObject.SetActive(false);
    }

    private void AddToLeaderboardData(string username,int score)
    {
        FetoscopyResult newResult = new FetoscopyResult() { Username = username, Score = score };
        storedResults.Add(newResult);
        FetoscopyResult[] results = new FetoscopyResult[storedResults.Count];
        for (int i = 0; i < results.Length; i++)
        {
            results[i] = storedResults[i];
        }
        FetoscopyResult tmp;
        for (int i = 0; i < results.Length - 1; i++)
        {
            for (int j = i+1; j < results.Length; j++)
            {
                if (results[i].Score < results[j].Score)
                {
                    tmp = results[i];
                    results[i] = results[j];
                    results[j] = tmp;
                }
            }
        }
        storedResults = results.ToList();
    }

    private void FillLeaderboard(string username)
    {
        foreach (var result in storedResults)
        {
            FetoscopyUIResultElement element = null;
            if(result.Username != username)
                element = Instantiate(_resultElementPrefab, _content).GetComponent<FetoscopyUIResultElement>();
            else
                element = Instantiate(_playerResultElementPrefab, _content).GetComponent<FetoscopyUIResultElement>();
            element.Fill(result.Username, GetScoreFormat(result.Score));
        }
    }

    private float CalculateTimeScore(float time)
    {
        float score = 1/5f;
        for (int i = 0; i < timeSegments.Length; i++)
        {
            if(time<timeSegments[i])
            {
                score = 1-(i / 5);
                break;
            }
        }
        return score;
    }

    private float CalculateFaultsScore(int mistakes,int totalTargets)
    {
        float step = totalTargets / 4;
        float score = 1/6f;
        for(int i = 0; i < 5; i ++)
        {
            if (mistakes <= i * step)
            {
                score = 1 - (i / 6f);
                break;
            }
        }
        return score;
    }

    private string GetScoreFormat(int score)
    {
        return string.Format("{0:0000}", score);
    }
    public void CalculateScore()
    {
        float totalTime = _chronometer.ChronoTime;
        int hits = SurgeryMechanic.Instance.Hits;
        int faults = SurgeryMechanic.Instance.Faults;
        _time.text = _chronometer.GetTimeFormat(totalTime);
        _hits.text = $"x{hits}";
        _faults.text = $"x{faults}";

        int hitsScore = hits * SurgeryMechanic.Instance.IncreseScore;
        _hitsScore.text = GetScoreFormat(hitsScore);

        int faultsScore = faults * SurgeryMechanic.Instance.FaultScore;
        _faultsScore.text = "- " + GetScoreFormat(faultsScore);

        float timeScore = CalculateTimeScore(totalTime);
        _timeScore.text = GetScoreFormat((int)(timeScore * SurgeryMechanic.Instance.TimeMaxScore));

        float targetsScore = CalculateFaultsScore(faults, hits);
        float rankScore = (timeScore + targetsScore) / 2f;
        int rankIndex = (int)((1-rankScore) * _ranks.Length);
        _ranks[rankIndex].SetActive(true);
        int score = (int)((hitsScore - faultsScore) + (timeScore * SurgeryMechanic.Instance.TimeMaxScore));
        _score.text = GetScoreFormat(score);

        AddToLeaderboardData("Tú", score);
        FillLeaderboard("Tú");
    }

    public void Open()
    {
        transform.DOScale(Vector3.one, 1).SetEase(_animEase);
    }

    public void Close()
    {
        foreach (var rank in _ranks)
            rank.SetActive(false);
        transform.DOScale(Vector3.zero, 1).SetEase(_animEase);
    }

    public void ReturnHome()
    {
        _returnHomeButton.localScale = Vector3.zero;
        _returnHomeButton.gameObject.SetActive(true);
        _returnHomeButton.DOScale(Vector3.one, 1).SetEase(_animEase);
    }
}
