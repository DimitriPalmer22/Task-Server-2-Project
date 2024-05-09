namespace Task_Server_2.ServerTasks;

/// <summary>
/// Group a set of tasks together so that they can be run as a single task.
/// </summary>
public sealed class ServerTaskGroup : OneTimeServerTask
{
    /// <summary>
    /// The queue of tasks to run.
    /// </summary>
    private readonly Queue<ServerTask> _taskQueue = new();

    /// <summary>
    /// The type of task group this is.
    /// </summary>
    public ServerTaskGroupType GroupType { get; }

    public ServerTaskGroup(string name, ServerTaskGroupType groupType, params ServerTask[] tasks)
        : base(name, false)
    {
        GroupType = groupType;

        // Add the tasks to the task queue
        foreach (var task in tasks)
            _taskQueue.Enqueue(task);
    }

    protected override void TaskLogic(ServerTaskManager serverTaskManager)
    {
        switch (GroupType)
        {
            case ServerTaskGroupType.Sequential:
                RunSequential(serverTaskManager);
                break;

            case ServerTaskGroupType.Asynchronous:
                RunAsynchronous(serverTaskManager);
                break;

            default:
                throw new NotImplementedException();
        }
    }

    private void RunSequential(ServerTaskManager serverTaskManager)
    {
        // Run each task in the queue
        while (_taskQueue.Count > 0)
        {
            // Get the next task
            var task = _taskQueue.Dequeue();
            
            // Create a C# task from the server task
            var cTask = new Task(() => task.Run(serverTaskManager));
            
            // Run the task synchronously
            cTask.RunSynchronously();
        }
    }

    private void RunAsynchronous(ServerTaskManager serverTaskManager)
    {
        // Create a list of the current C# tasks.
        List<Task> currentTasks = new();

        // Run each task asynchronously
        while (_taskQueue.Count > 0)
        {
            // Get the next task
            var task = _taskQueue.Dequeue();

            // Run the task asynchronously
            var cTask = Task.Run(() => task.Run(serverTaskManager));

            // Add the C# task to the list
            currentTasks.Add(cTask);
        }

        Task.WaitAll(currentTasks.ToArray());
    }
}

public enum ServerTaskGroupType
{
    /// <summary>
    /// Run the tasks one after another.
    /// </summary>
    Sequential,

    /// <summary>
    /// Run each task independently.
    /// </summary>
    Asynchronous
}