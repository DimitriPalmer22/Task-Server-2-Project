namespace Task_Server_2.ServerTasks.ActivationConditions;

public class SimpleActivationCondition : IActivationCondition
{
    public bool ReadyToRun => true;

    public bool AddBackToTaskManager => false;
    
    public DateTime ScheduledTime { get; } = DateTime.Now;

    public bool Awaited => false;
}