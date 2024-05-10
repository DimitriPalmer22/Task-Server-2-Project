namespace Task_Server_2.ServerTasks.ActivationConditions;

/// <summary>
/// The interface that will be used to determine if a task is ready to run.
/// </summary>
public interface IActivationCondition
{
    /// <summary>
    /// Determines if the task is ready to run.
    /// </summary>
    public bool ReadyToRun { get; }
    
    /// <summary>
    /// Determines if the task should be added back to the task manager after it has been run.
    /// </summary>
    public bool AddBackToTaskManager { get; }
    
    /// <summary>
    /// When the task is scheduled to run.
    /// </summary>
    public DateTime ScheduledTime { get; }
    
    /// <summary>
    /// Will this task be awaited by the task manager?
    /// </summary>
    protected internal bool Awaited { get; }
}