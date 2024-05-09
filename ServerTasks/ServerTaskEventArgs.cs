namespace Task_Server_2.ServerTasks;


public class ServerTaskEventArgs : EventArgs
{
    /// <summary>
    /// A reference to the <see cref="ServerTask"/> that raised the event.
    /// </summary>
    public ServerTask ServerTask { get; }
    
    /// <summary>
    /// A reference to the <see cref="ServerTaskManager"/> that raised the event.
    /// </summary>
    public ServerTaskManager TaskManager { get; }
    
    /// <summary>
    /// The time the event was triggered.
    /// </summary>
    public DateTime DateTime { get; }

    /// <summary>
    /// The type of event that is being raised.
    /// </summary>
    public ServerTaskEventType ServerTaskEventType { get; }

    public ServerTaskEventArgs(ServerTask serverTask, ServerTaskManager taskManager, DateTime dateTime, ServerTaskEventType serverTaskEventType)
    {
        ServerTask = serverTask;
        TaskManager = taskManager;
        DateTime = dateTime;
        ServerTaskEventType = serverTaskEventType;
    }
    
    public static ServerTaskEventArgs DefaultArgs(ServerTask serverTask, ServerTaskManager serverTaskManager, ServerTaskEventType serverTaskEventType)
    {
        return new ServerTaskEventArgs(serverTask, serverTaskManager, DateTime.Now, serverTaskEventType);
    }
}