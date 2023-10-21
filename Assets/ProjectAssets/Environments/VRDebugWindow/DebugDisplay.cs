using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DebugDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _display;
    private Dictionary<string, string> debugLogs = new Dictionary<string, string>();

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }
    private void HandleLog(string condition, string stackTrace, LogType type)
    {
        if(type == LogType.Log || type == LogType.Error)
        {
            string[] splitString = condition.Split(char.Parse(":"));
            string debugKey = splitString[0];
            string debugValue = splitString.Length > 1 ? splitString[1] : "";
            debugLogs[debugKey] = debugValue;
        }

        string displayText = "";
        foreach(var log in debugLogs)
        {
            if (log.Value == "")
                displayText += log.Key + "\n";
            else
                displayText += log.Key + ": " + log.Value + "\n";
        }
        _display.text = displayText;
    }
}
