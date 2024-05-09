namespace Task_Server_2.ServerTasks;

public abstract class RecurringServerTask : ServerTask
{
    protected RecurringServerTask(string name, bool awaited) : base(name, awaited)
    {
    }
}