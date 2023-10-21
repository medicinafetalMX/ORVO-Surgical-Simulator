using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskListElement : MonoBehaviour
{
    [SerializeField] GameObject _checkmark;
    [SerializeField] TextMeshProUGUI _task;
    public void MarkComplete()
    {
        _checkmark.SetActive(true);
    }
    public void Fill(string task)
    {
        _task.text = task;
        _checkmark.SetActive(false);
    }
}
