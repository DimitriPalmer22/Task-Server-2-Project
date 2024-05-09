namespace Task_Server_2.ServerTasks;

public abstract class OngoingServerTask : ServerTask
{
    protected internal override bool ReadyToRun => true;

    protected OngoingServerTask(string name, bool awaited) : base(name, awaited)
    {
    }
}