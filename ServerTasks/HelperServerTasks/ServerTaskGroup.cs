using Task_Server_2.ServerTasks.ActivationConditions;

namespace Task_Server_2.ServerTasks.HelperServerTasks;

/// <summary>
/// Group a set of tasks together so that they can be run as a single task.
/// </summary>
public sealed class ServerTaskGroup : ServerTask
{
    /// <summary>
    /// The queue of tasks to run.
    /// </summary>
    private readonly Queue<ServerTask> _taskQueue = new();

    /// <summary>
    /// The type of task group this is.
    /// </summary>
    private ServerTaskGroupType GroupType { get; }

    public ServerTaskGroup(string name, ServerTaskGroupType groupType, params ServerTask[] tasks)
        : this(name, new SimpleActivationCondition(), groupType, tasks)
    {
    }

    public ServerTaskGroup(string name, IActivationCondition activationCondition, ServerTaskGroupType groupType,
        params ServerTask[] tasks)
        : base(name, activationCondition)
    {
        GroupType = groupType;

        // Add the tasks to the task queue
        foreach (var task in tasks)
            _taskQueue.Enqueue(task);
    }

    protected override void TaskLogic(ServerTaskManager serverTaskManager, ServerTaskProject serverTaskProject)
    {
        switch (GroupType)
        {
            case ServerTaskGroupType.Sequential:
                RunSequential(serverTaskManager, serverTaskProject);
                break;

            case ServerTaskGroupType.Asynchronous:
                RunAsynchronous(serverTaskManager, serverTaskProject);
                break;

            default:
                throw new NotImplementedException();
        }
    }

    private void RunSequential(ServerTaskManager serverTaskManager, ServerTaskProject serverTaskProject)
    {
        // Copy the queue so that we can modify it
        var currentQueue = new Queue<ServerTask>(_taskQueue);
        
        // Run each task in the queue
        // Make sure the task is ready to run
        while (currentQueue.Count > 0 && _taskQueue.Peek().ActivationCondition.ReadyToRun)
        {
            // Get the next task
            var task = currentQueue.Dequeue();

            // Create a C# task from the server task
            var cTask = new Task(() => task.Run(serverTaskManager, serverTaskProject));

            // Run the task synchronously
            cTask.RunSynchronously();
        }
    }

    private void RunAsynchronous(ServerTaskManager serverTaskManager, ServerTaskProject serverTaskProject)
    {
        // Create a list of the current C# tasks.
        List<Task> currentTasks = new();
        
        // Copy the queue so that we can modify it
        var currentQueue = new Queue<ServerTask>(_taskQueue);

        // Run each task asynchronously
        while (currentQueue.Count > 0)
        {
            // Get the next task
            var task = currentQueue.Dequeue();

            // Run the task asynchronously
            var cTask = Task.Run(() => task.Run(serverTaskManager, serverTaskProject));

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