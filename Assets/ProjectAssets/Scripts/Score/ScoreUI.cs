using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class ScoreUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] TextMeshProUGUI _scoreIndicationPrefab;
    [SerializeField] RectTransform _scoreIndicatorsContainer;
    [SerializeField] Color _increaseColor;
    [SerializeField] Color _decreaseColor;

    private Queue<TextMeshProUGUI> scoreIndicators = new Queue<TextMeshProUGUI>();
    private int score;
    private void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            var scoreIndicator = Instantiate(_scoreIndicationPrefab,_scoreIndicatorsContainer);
            scoreIndicators.Enqueue(scoreIndicator);
            scoreIndicator.color = new Color(0, 0, 0,0);
        }
    }
    private TextMeshProUGUI GetIndicator()
    {
        var indicator = scoreIndicators.Dequeue();
        scoreIndicators.Enqueue(indicator);
        return indicator;
    }

    public void IncreaseScore(int amount,int totalScore)
    {
        int currentScore = score;
        score = totalScore;
        TextMeshProUGUI indicator = GetIndicator();
        indicator.rectTransform.localScale = Vector3.one;
        indicator.color = _increaseColor;
        DOTween.To(() => currentScore, x => currentScore = x, score, 2)
            .OnUpdate(()=> 
            {
                _score.text = string.Format("{0:0000}",currentScore);
            });
        indicator.text = amount.ToString();
        indicator.rectTransform.anchoredPosition = Vector2.zero;
        indicator.rectTransform.DOAnchorPosY(indicator.rectTransform.anchoredPosition.y + 200, 1);
        indicator.rectTransform.DOScale(2, 2f);
        Color increaseTransparent = _increaseColor;
        increaseTransparent.a = 0;
        indicator.DOBlendableColor(increaseTransparent, 3f);
    }

    public void DecreaseScore(int amount,int totalScore)
    {
        int currentScore = score;
        score = totalScore;
        TextMeshProUGUI indicator = GetIndicator();
        indicator.rectTransform.localScale = Vector3.one;
        indicator.color = _decreaseColor;
        DOTween.To(() => currentScore, x => currentScore = x, score, 2)
            .OnUpdate(() =>
            {
                _score.text = string.Format("{0:0000}", currentScore);
            });
        indicator.text = (-amount).ToString();
        indicator.rectTransform.anchoredPosition = Vector2.zero;
        indicator.rectTransform.DOAnchorPosY(indicator.rectTransform.anchoredPosition.y + 200,1);
        indicator.rectTransform.DOScale(2, 2f);
        Color decreaseTransparent = _decreaseColor;
        decreaseTransparent.a = 0;
        indicator.DOBlendableColor(decreaseTransparent,3f);
    }

    public void SetScore(int score)
    {
        this.score = score;
        _score.text = _score.text = string.Format("{0:0000}", score);
    }
}
