using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public List<TaskData> Tasks = new List<TaskData>();
    [SerializeField] TasksList _taskList;
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _completedTaskSound;

    private int taskIndex = 0;
    private TaskData currentTask;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5);
        StartTasks();
    }
    public void StartTasks()
    {
        taskIndex = -1;
        NextTask();
    }

    public void NextTask()
    {
        taskIndex++;
        if(taskIndex>= Tasks.Count)
        {
            EndTasks();
        }
        else
        {
            currentTask = Tasks[taskIndex];
            InitCurrentTask();
        }
    }

    public void EndTasks()
    {
        Debug.Log("All tasks done");
        SurgeryMechanic.Instance.EndSimulation();
    }

    public void CompleteTask(TaskData task)
    {
        if(currentTask == task)
        {
            Debug.Log(task.name + " completed");
            _audio.PlayOneShot(_completedTaskSound);
            _taskList.CompleteTask(task);
            NextTask();
        }
        else
        {
            Debug.LogError("Received unexpected task "+task.name);
        }
    }

    public void InitCurrentTask()
    {
        Debug.Log("Starting "+currentTask.name);
        _taskList.AddTask(currentTask);
        currentTask.Listener = this;
        currentTask.Init();
        currentTask.StartTask();
        if (currentTask.audio)
            _audio.PlayOneShot(currentTask.audio);
    }
}
