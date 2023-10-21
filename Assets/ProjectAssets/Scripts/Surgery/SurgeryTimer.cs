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
        string time = string.Format("{0:00}:{1:00}", minutes, seconds);
        _timeText.text = time;
    }

    public void StartChronometer()
    {
        ChronoTime = 0;
        isMeasuringTime = true;
    }

    public void StopChronometer()
    {
        isMeasuringTime = false;
    }
}
