namespace Task_Server_2.ServerTasks;

public abstract class ServerTaskProject
{
    /// <summary>
    /// The list of all projects that are currently running.
    /// </summary>
    private static readonly List<ServerTaskProject> ActiveProjects = new();

    /// <summary>
    /// The tasks that are a part of this project.
    /// This is a priority queue that will order the tasks by their scheduled time.
    /// </summary>
    private readonly PriorityQueue<ServerTask, DateTime> _serverTasks = new();

    /// <summary>
    /// A lock object to prevent multiple threads from accessing the queue at the same time.
    /// </summary>
    private readonly object _queueLock = new();

    /// <summary>
    /// When this project was created.
    /// Can be used to determine if some tasks should be run.
    /// </summary>
    public DateTime CreationTime { get; } = DateTime.Now;

    /// <summary>
    /// Peek at the next item in the queue.
    /// </summary>
    public ServerTask NextItem
    {
        get
        {
            lock (_queueLock)
            {
                if (_serverTasks.Count == 0)
                    return null;

                return _serverTasks.Peek();
            }
        }
    }

    public ServerTaskProject()
    {
        AddProject(this);
    }

    public void EnqueueTask(ServerTask task)
    {
        lock (_queueLock)
            _serverTasks.Enqueue(task, task.ActivationCondition.ScheduledTime);
    }

    public ServerTask PopTask()
    {
        lock (_queueLock)
        {
            if (_serverTasks.Count == 0)
                return null;

            return _serverTasks.Dequeue();
        }
    }

    public static IReadOnlyList<ServerTaskProject> GetActiveProjects()
    {
        lock (ActiveProjects)
            return ActiveProjects.ToList();
    }

    private static void AddProject(ServerTaskProject project)
    {
        lock (ActiveProjects)
        {
            // If the project is already in the list, don't add it again
            if (ActiveProjects.Contains(project))
                return;

            ActiveProjects.Add(project);
        }
    }

    public static void RemoveProject(ServerTaskProject project)
    {
        lock (ActiveProjects)
            ActiveProjects.Remove(project);
    }
}