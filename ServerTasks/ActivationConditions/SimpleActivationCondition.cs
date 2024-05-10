namespace Task_Server_2.ServerTasks.ActivationConditions;

public class SimpleActivationCondition : IActivationCondition
{
    public ServerTask ServerTask { get; set; }

    public bool ReadyToRun => true;

    public bool AddBackToTaskManager => false;
    
    public bool HasRunBefore { get; set; }

    public DateTime ScheduledTime { get; } = DateTime.Now;

    public bool Awaited => false;

    public void HookEvents(ServerTask serverTask)
    {
    }
}