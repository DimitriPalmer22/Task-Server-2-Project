namespace Task_Server_2.ServerTasks.ActivationConditions;

public class ScheduledActivationCondition : IActivationCondition
{
    public bool ReadyToRun => ScheduledTime <= DateTime.Now;
    
    public bool AddBackToTaskManager => false;
    public DateTime ScheduledTime { get; init; }
    
    public bool Awaited { get; init; }

    public ScheduledActivationCondition(DateTime scheduledTime, bool awaited = false)
    {
        ScheduledTime = scheduledTime;
        Awaited = awaited;
    }
    
}