namespace Task_Server_2.ServerTasks;

public abstract class ScheduledServerTask : OneTimeServerTask
{
    private readonly DateTime _scheduledTime;
    
    public override DateTime ScheduledTime => _scheduledTime;

    protected internal override bool ReadyToRun => DateTime.Now >= _scheduledTime;

    protected ScheduledServerTask(string name, DateTime scheduledTime, bool awaited)
        : base(name, awaited)
    {
        _scheduledTime = scheduledTime;
    }

}