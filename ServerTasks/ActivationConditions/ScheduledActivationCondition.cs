namespace Task_Server_2.ServerTasks.ActivationConditions;

public class ScheduledActivationCondition : IActivationCondition
{
    public ServerTask ServerTask { get; set; }

    public bool ReadyToRun => ScheduledTime <= DateTime.Now;

    public bool AddBackToTaskManager => false;

    public bool HasRunBefore { get; set; }

    public DateTime ScheduledTime { get; init; }

    public bool Awaited { get; init; }

    public ScheduledActivationCondition(DateTime scheduledTime, bool awaited = false)
    {
        ScheduledTime = scheduledTime;
        Awaited = awaited;
    }

    public void HookEvents(ServerTask serverTask)
    {
    }
}