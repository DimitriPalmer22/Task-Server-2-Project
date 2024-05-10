namespace Task_Server_2.ServerTasks.ActivationConditions;

public class RecurringActivationCondition : IActivationCondition
{
    public int Iterations { get; private set; }

    public int MaxIterations { get; init; }

    public DateTime StartTime { get; init; }

    public TimeSpan Interval { get; init; }

    public ServerTask ServerTask { get; set; }

    public bool ReadyToRun =>
        (DateTime.Now >= StartTime) &&
        (MaxIterations == -1 || Iterations < MaxIterations) &&
        DateTime.Now >= ScheduledTime;

    public bool AddBackToTaskManager =>
        (MaxIterations == -1 || Iterations < MaxIterations);

    public bool HasRunBefore { get; set; }

    public DateTime ScheduledTime => StartTime + (Interval * Iterations);

    public bool Awaited { get; }

    public RecurringActivationCondition(DateTime startTime, TimeSpan interval, int maxIterations = -1,
        bool awaited = false)
    {
        StartTime = startTime;
        Interval = interval;
        MaxIterations = maxIterations;
        Awaited = awaited;
    }

    public void HookEvents(ServerTask serverTask)
    {
        serverTask.OnStarted += UpdateIterations;
    }

    private void UpdateIterations(ServerTask sender, ServerTaskEventArgs.ServerTaskEventArgs e)
    {
        Iterations++;
    }
}