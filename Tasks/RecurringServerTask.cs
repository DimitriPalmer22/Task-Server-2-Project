namespace Task_Server_2.Tasks;

public abstract class RecurringServerTask : ServerTask
{
    protected RecurringServerTask(string name) : base(name)
    {
    }
}