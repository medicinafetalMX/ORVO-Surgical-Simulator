using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SurgeryTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _timeText;
    private float ChronoTime;
    private bool isMeasuringTime = false;


    void Update()
    {
        if (isMeasuringTime)
        {
            ChronoTime += Time.deltaTime;
        }
    }

    public void WriteTime()
    {
        int minutes = Mathf.RoundToInt(ChronoTime / 60f);
        int seconds = Mathf.RoundToInt(ChronoTime % 60);
        _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartChronometer()
    {
        isMeasuringTime = true;
    }

    public void StopChronometer()
    {
        isMeasuringTime = false;
        ChronoTime = 0;
    }
}
