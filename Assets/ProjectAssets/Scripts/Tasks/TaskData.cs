using UnityEngine;
[System.Serializable]
public class TaskData : MonoBehaviour
{
    public string task;
    public AudioClip audio;
    public TaskManager Listener;
    private bool isTaskRunning = false;
    protected void Update()
    {
        if (isTaskRunning)
        {
            ExecuteTask();
        }
    }
    public virtual void Init()
    {

    }
    public virtual void StartTask()
    {
        isTaskRunning = true;
    }
    public virtual void ExecuteTask()
    {

    }
    public virtual void CompleteTask()
    {
        isTaskRunning = false;
        Listener.CompleteTask(this);
    }
}
