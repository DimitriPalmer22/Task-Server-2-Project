namespace Task_Server_2.ServerTasks.ActivationConditions;

/// <summary>
/// The interface that will be used to determine if a task is ready to run.
/// </summary>
public interface IActivationCondition
{
    /// <summary>
    /// A reference to the task that this activation condition is for.
    /// Should only be set by the task.
    /// </summary>
    public ServerTask ServerTask { get; set; }

    /// <summary>
    /// Determines if the task is ready to run.
    /// </summary>
    public bool ReadyToRun { get; }

    /// <summary>
    /// Determines if the task should be added back to the task manager after it has been run.
    /// </summary>
    public bool AddBackToTaskManager { get; }
    
    /// <summary>
    /// A flag to determine if the task has run before.
    /// </summary>
    public bool HasRunBefore { get; set; }

    /// <summary>
    /// When the task is scheduled to run.
    /// </summary>
    public DateTime ScheduledTime { get; }

    /// <summary>
    /// Will this task be awaited by the task manager?
    /// </summary>
    public bool Awaited { get; }

    /// <summary>
    /// Hooks the events for the activation condition to the corresponding events in the server task.
    /// </summary>
    public void HookEvents(ServerTask serverTask);
}