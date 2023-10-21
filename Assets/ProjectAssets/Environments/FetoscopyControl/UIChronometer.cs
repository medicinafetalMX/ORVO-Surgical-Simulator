using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIChronometer : MonoBehaviour
{
    public float ChronoTime { get; set; }
    [SerializeField] TextMeshProUGUI _time;
    private bool isMeasuringTime = false;
    private void Update()
    {
        if (isMeasuringTime)
        {
            ChronoTime += Time.deltaTime;
            WriteTime();
        }
    }
    private void WriteTime()
    {
        _time.text = GetTimeFormat(ChronoTime);
    }

    public string GetTimeFormat(float time)
    {
        int minutes = Mathf.RoundToInt(time / 60f);
        int seconds = Mathf.RoundToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void PauseChronometer()
    {
        isMeasuringTime = false;
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
