namespace Task_Server_2.ServerTasks;

/// <summary>
/// A delegate for handling events related to server tasks.
/// </summary>
public delegate void ServerTaskEventHandler(ServerTask sender, ServerTaskEventArgs e);

/// <summary>
/// An abstract class to represent a task.
/// </summary>
public abstract class ServerTask
{
    #region Fields

    public event ServerTaskEventHandler OnStarted = LogServerTaskEvent;
    public event ServerTaskEventHandler OnCompleted = LogServerTaskEvent;
    public event ServerTaskEventHandler OnFailed = LogServerTaskEvent;

    #endregion Fields

    #region Properties

    /// <summary>
    /// The display name of the task.
    /// </summary>    
    public string TaskName { get; init; }

    /// <summary>
    /// Will this task be awaited by the task manager?
    /// </summary>
    protected internal bool Awaited { get; init; }

    internal Task Task { get; private set; }

    #endregion Properties

    protected ServerTask(string name, bool awaited)
    {
        TaskName = name;
        Awaited = awaited;
    }

    #region Methods

    /// <summary>
    /// A flag to determine if the task is ready to run.
    /// The task manager will check this flag before running the task.
    /// </summary>
    protected internal abstract bool ReadyToRun { get; }

    /// <summary>
    /// The method outside classes will use to run the task.
    /// This method should only really be called by the <see cref="ServerTaskManager"/>.
    /// </summary>
    internal void Run(ServerTaskManager serverTaskManager, ServerTaskProject project)
    {
        // Invoke the OnStarted event.
        OnStarted?.Invoke(this,
            ServerTaskEventArgs.DefaultArgs(this, serverTaskManager, ServerTaskEventType.Started, project));

        var ranSuccessfully = true;

        // Run the task logic.
        try
        {
            TaskLogic(serverTaskManager, project);
        }
        catch (Exception e)
        {
            // Invoke the OnCompleted event
            OnFailed?.Invoke(this,
                ServerTaskEventArgs.DefaultArgs(this, serverTaskManager, ServerTaskEventType.Failed, project));

            ranSuccessfully = false;
        }

        // Invoke the OnCompleted event
        if (ranSuccessfully)
            OnCompleted?.Invoke(this,
                ServerTaskEventArgs.DefaultArgs(this, serverTaskManager, ServerTaskEventType.Completed, project));
    }

    /// <summary>
    /// The method that will be called when the task is started.
    /// This method handles the actual logic of the task.
    /// </summary>
    protected abstract void TaskLogic(ServerTaskManager serverTaskManager, ServerTaskProject serverTaskProject);

    /// <summary>
    /// Create a C# Task object for this server task.
    /// This method should only be run within the task manager.
    /// </summary>
    /// <param name="serverTaskManager"></param>
    /// <param name="project"></param>
    /// <returns></returns>
    internal Task CreateTask(ServerTaskManager serverTaskManager, ServerTaskProject project)
    {
        // If the task has already been created, return it.
        if (Task != null)
            return Task;
        
        // Create a new task
        Task = new Task(() => Run(serverTaskManager, project));
        
        return Task;
    }
    
    #endregion Methods

    #region Static Methods

    private static void LogServerTaskEvent(ServerTask sender, ServerTaskEventArgs e)
    {
        Console.WriteLine(
            $"({e.DateTime}) [{e.ServerTaskEventType.ToString().ToUpper()}] {{{e.TaskManager}}} {e.ServerTask.TaskName}.");
    }

    #endregion Static Methods
}