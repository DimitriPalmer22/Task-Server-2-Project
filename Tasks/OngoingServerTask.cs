namespace Task_Server_2.Tasks;

public abstract class OngoingServerTask : ServerTask
{
    protected internal override bool ReadyToRun => true;

    protected OngoingServerTask(string name) : base(name)
    {
    }
}