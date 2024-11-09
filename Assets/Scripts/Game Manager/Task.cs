using System;
public class Task
{
    public float Delay { get; set; }
    public string NameTask { get; set; }
    public Action OnStart { get; set; }
    public Action OnComplete { get; set; }
    public Func<bool> CompleteCondition { get; set; }
    public bool IsActive { get; set; }
    public bool IsComplete { get; set; }

    public Task(float delay, string nameTask, Action onStart, Action onComplete, Func<bool> completedCondition)
    {
        Delay = delay;
        NameTask = nameTask;
        OnStart = onStart;
        OnComplete = onComplete;
        CompleteCondition = completedCondition;
    }

    public Task(float delay, string nameTask, Func<bool> completedCondition)
    {
        Delay = delay;
        NameTask = nameTask;
        CompleteCondition = completedCondition;
    }

}
