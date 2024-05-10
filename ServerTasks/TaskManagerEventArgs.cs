using System.Diagnostics.CodeAnalysis;

namespace Task_Server_2.ServerTasks;

public class TaskManagerEventArgs : EventArgs
{
    public new static TaskManagerEventArgs Empty => new TaskManagerEventArgs(null, null);

    public required ServerTask Task { get; init; }
    public required ServerTaskProject Project { get; init; }

    [SetsRequiredMembers]
    public TaskManagerEventArgs(ServerTask task, ServerTaskProject project)
    {
        Task = task;
        Project = project;
    }
}
