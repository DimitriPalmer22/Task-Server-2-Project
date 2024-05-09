namespace Task_Server_2.Tasks;

/// <summary>
/// A delegate for handling events related to server tasks.
/// </summary>
public delegate void ServerTaskEventHandler(ServerTask sender, ServerTaskEventArgs e);

/// <summary>
/// An abstract class to represent a task.
/// </summary>
public abstract class ServerTask
{
    public event ServerTaskEventHandler OnStarted;
    public event ServerTaskEventHandler OnCompleted;
    public event ServerTaskEventHandler OnFailed;

    /// <summary>
    /// The display name of the task.
    /// </summary>
    protected readonly string taskName;

    protected ServerTask(string name)
    {
        taskName = name;
    }
    
    /// <summary>
    /// A flag to determine if the task is ready to run.
    /// The task manager will check this flag before running the task.
    /// </summary>
    protected internal abstract bool ReadyToRun { get; }
    
    /// <summary>
    /// The method outside classes will use to run the task.
    /// This method should only really be called by the <see cref="ServerTaskManager"/>.
    /// </summary>
    internal void Run(ServerTaskManager serverTaskManager)
    {
        // Invoke the OnStarted event.
        OnStarted?.Invoke(this, ServerTaskEventArgs.DefaultArgs(this, serverTaskManager));

        // Run the task logic.
        try
        {
            TaskLogic(serverTaskManager);
            OnCompleted?.Invoke(this, ServerTaskEventArgs.DefaultArgs(this, serverTaskManager));
        }
        catch (Exception e)
        {
            OnFailed?.Invoke(this, ServerTaskEventArgs.DefaultArgs(this, serverTaskManager));
        }
    }

    /// <summary>
    /// The method that will be called when the task is started.
    /// This method handles the actual logic of the task.
    /// </summary>
    protected abstract void TaskLogic(ServerTaskManager serverTaskManager);
}