namespace Task_Server_2.Tasks;

public abstract class OneTimeServerTask : ServerTask
{
    protected internal override bool ReadyToRun => true;

    protected OneTimeServerTask(string name)
        : base(name)
    {
    }
}