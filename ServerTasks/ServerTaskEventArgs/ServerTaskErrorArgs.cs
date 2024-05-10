using System.Diagnostics.CodeAnalysis;

namespace Task_Server_2.ServerTasks;

public class ServerTaskErrorArgs : ServerTaskEventArgs
{
    public required Exception Exception { get; init; }

    [SetsRequiredMembers]
    public ServerTaskErrorArgs(ServerTask serverTask, ServerTaskManager taskManager, DateTime dateTime,
        ServerTaskEventType serverTaskEventType, ServerTaskProject serverTaskProject, Exception exception)
        : base(serverTask, taskManager, dateTime, serverTaskEventType, serverTaskProject)
    {
        Exception = exception;
    }

    public static ServerTaskErrorArgs ErrorArgs(ServerTask serverTask, ServerTaskManager serverTaskManager,
        ServerTaskEventType serverTaskEventType, ServerTaskProject serverTaskProject, Exception exception)
    {
        return new ServerTaskErrorArgs(serverTask, serverTaskManager, DateTime.Now, serverTaskEventType,
            serverTaskProject, exception);
    }
}