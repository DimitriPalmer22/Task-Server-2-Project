using System.Diagnostics.CodeAnalysis;

namespace Task_Server_2.ServerTasks.ServerTaskEventArgs;

public class ServerTaskErrorEventArgs : ServerTaskEventArgs
{
    public required Exception Exception { get; init; }

    [SetsRequiredMembers]
    public ServerTaskErrorEventArgs(ServerTask serverTask, ServerTaskManager taskManager, DateTime dateTime,
        ServerTaskEventType serverTaskEventType, ServerTaskProject serverTaskProject, Exception exception)
        : base(serverTask, taskManager, dateTime, serverTaskEventType, serverTaskProject)
    {
        Exception = exception;
    }

    public static ServerTaskErrorEventArgs ErrorArgs(ServerTask serverTask, ServerTaskManager serverTaskManager,
        ServerTaskEventType serverTaskEventType, ServerTaskProject serverTaskProject, Exception exception)
    {
        return new ServerTaskErrorEventArgs(serverTask, serverTaskManager, DateTime.Now, serverTaskEventType,
            serverTaskProject, exception);
    }
}