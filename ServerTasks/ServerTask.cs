using System.Diagnostics;
using Task_Server_2.DebugLogger;
using Task_Server_2.ServerTasks.ActivationConditions;
using Task_Server_2.ServerTasks.ServerTaskEventArgs;

namespace Task_Server_2.ServerTasks;

/// <summary>
/// A delegate for handling events related to server tasks.
/// </summary>
public delegate void ServerTaskEventHandler(ServerTask sender, ServerTaskEventArgs.ServerTaskEventArgs e);

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
    /// The C# Task object that will run this server task.
    /// </summary>
    private Task Task { get; set; }

    /// <summary>
    /// The activation condition for the task.
    /// </summary>
    protected internal IActivationCondition ActivationCondition { get; init; }

    #endregion Properties

    protected ServerTask(string name, IActivationCondition activationCondition)
    {
        TaskName = name;

        ActivationCondition = activationCondition;
        ActivationCondition.ServerTask = this;
        ActivationCondition.HookEvents(this);
    }

    #region Methods

    /// <summary>
    /// The method outside classes will use to run the task.
    /// This method should only really be called by the <see cref="ServerTaskManager"/>.
    /// </summary>
    internal void Run(ServerTaskManager serverTaskManager, ServerTaskProject project)
    {
        // Invoke the OnStarted event.
        OnStarted?.Invoke(this,
            ServerTaskEventArgs.ServerTaskEventArgs.DefaultArgs(this, serverTaskManager, ServerTaskEventType.Started,
                project));

        var ranSuccessfully = true;

        // Update the Has Run Before flag
        ActivationCondition.HasRunBefore = true;

        // Run the task logic.
        try
        {
            TaskLogic(serverTaskManager, project);
        }
        catch (Exception exception)
        {
            // Invoke the OnFailed event
            OnFailed?.Invoke(this,
                ServerTaskErrorEventArgs.ErrorArgs(this, serverTaskManager, ServerTaskEventType.Failed, project,
                    exception));

            ranSuccessfully = false;
        }

        // Invoke the OnCompleted event
        if (ranSuccessfully)
            OnCompleted?.Invoke(this,
                ServerTaskEventArgs.ServerTaskEventArgs.DefaultArgs(this, serverTaskManager,
                    ServerTaskEventType.Completed, project));
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
        // If the task has not already been created, or
        // the task is recurring, and it is completed,
        // create a new task.
        if (
            Task == null ||
            (ActivationCondition.AddBackToTaskManager && Task is { IsCompleted: true })
        )
            Task = new Task(() => Run(serverTaskManager, project));

        return Task;
    }

    #endregion Methods

    #region Static Methods

    private static void LogServerTaskEvent(ServerTask sender, ServerTaskEventArgs.ServerTaskEventArgs e)
    {
        var logLevel = e.ServerTaskEventType switch
        {
            ServerTaskEventType.Started => LogType.Event,
            ServerTaskEventType.Completed => LogType.Event,
            ServerTaskEventType.Failed => LogType.Error,
            _ => LogType.Normal
        };

        var logMessage = DebugLog.Instance.Log(logLevel,
            $"[{e.ServerTaskEventType.ToString().ToUpper(),-10}] \"{e.ServerTask.TaskName}\".");

        // If the event is a failure, add a sub message containing the error message
        if (e.ServerTaskEventType == ServerTaskEventType.Failed)
        {
            if (e is not ServerTaskErrorEventArgs errorArgs)
                return;

            if (errorArgs.Exception == null)
                return;

            logMessage.AddSubMessage(0, errorArgs.Exception.Source);

            logMessage.AddSubMessage(0, errorArgs.Exception.Message);

            var splitStackTrace = errorArgs.Exception.StackTrace?.Replace("   ", "").Split('\n');

            // Split the stack trace into lines
            if (splitStackTrace != null)
                foreach (var stackTrace in splitStackTrace)
                    logMessage.AddSubMessage(1, stackTrace);
        }
    }

    #endregion Static Methods
}