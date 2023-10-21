using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksList : MonoBehaviour
{
    [SerializeField] TaskListElement _taskListPrefab;
    [SerializeField] Transform _content;

    private List<TaskListElement> tasks = new List<TaskListElement>();
    private Dictionary<TaskData, TaskListElement> taskReferences = new Dictionary<TaskData, TaskListElement>();
    public void AddTask(TaskData taskData)
    {
        var task = Instantiate(_taskListPrefab, _content);
        task.Fill(taskData.task);
        task.transform.SetAsFirstSibling();
        taskReferences[taskData] = task;
        tasks.Add(task);
    }

    public void CompleteTask(TaskData taskData)
    {
        if (taskReferences.ContainsKey(taskData))
        {
            taskReferences[taskData].MarkComplete();
        }
        else
            Debug.LogError("Any task list element have a reference to "+taskData);
    }
}