namespace Task_Server_2.ServerTasks;

public abstract class OneTimeServerTask : ServerTask
{
    protected internal override bool ReadyToRun => true;

    public override DateTime ScheduledTime => DateTime.Now;

    protected OneTimeServerTask(string name, bool awaited)
        : base(name, awaited)
    {
    }
}