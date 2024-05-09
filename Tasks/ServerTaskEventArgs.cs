namespace Task_Server_2.Tasks;


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
    public DateTime Time { get; }

    public ServerTaskEventArgs(ServerTask serverTask, ServerTaskManager taskManager, DateTime time)
    {
        ServerTask = serverTask;
        TaskManager = taskManager;
        Time = time;
    }
    
    public static ServerTaskEventArgs DefaultArgs(ServerTask serverTask, ServerTaskManager serverTaskManager)
    {
        return new ServerTaskEventArgs(serverTask, serverTaskManager, DateTime.Now);
    }
    
    
}