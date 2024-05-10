namespace Task_Server_2.ServerTasks;

public sealed class ServerTaskManager
{
    #region Fields

    /// <summary>
    /// Singleton instance of the <see cref="ServerTaskManager"/>.
    /// </summary>
    private static ServerTaskManager _instance;

    /// <summary>
    /// The list of <see cref="ServerTaskProject"/>s that this class will manage.
    /// </summary>
    private readonly List<ServerTaskProject> _serverTaskProjects = new();

    /// <summary>
    /// Store the active tasks in a dictionary grouped by their project.
    /// </summary>
    private readonly Dictionary<ServerTaskProject, List<ServerTask>> _activeTasks = new();

    /// <summary>
    /// A lock object to prevent multiple threads from accessing the active tasks dictionary at the same time.
    /// </summary>
    private readonly object _activeTasksLock = new();

    /// <summary>
    /// The separate thread that the <see cref="ServerTaskManager"/> runs on.
    /// </summary>
    private Thread _thread;

    /// <summary>
    /// A flag used to determine if the <see cref="ServerTaskManager"/> is active.
    /// This controls if new tasks can be added to the task manager.
    /// </summary>
    private bool _active;

    /// <summary>
    /// How many times a second the Task Manager will check for new tasks.
    /// </summary>
    public int UpdatesPerSecond { get; set; } = 60;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Singleton instance of the <see cref="ServerTaskManager"/>.
    /// </summary>
    public static ServerTaskManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ServerTaskManager();

            return _instance;
        }
    }

    public int ActiveTaskCount
    {
        get
        {
            lock (_activeTasksLock)
                return _activeTasks.Sum(n => n.Value.Count);
        }
    }

    /// <summary>
    /// Used to control the loop in the Run method.
    /// </summary>
    public bool Running => _active;

    #endregion Properties

    private ServerTaskManager()
    {
        // Prevent the creation of multiple instances of the ServerTaskManager
        if (_instance != null)
            throw new InvalidOperationException("Cannot create multiple instances of the ServerTaskManager.");
    }

    public void AddProject(ServerTaskProject project)
    {
        // Check if the project is already in the list
        if (_serverTaskProjects.Contains(project))
            return;

        _serverTaskProjects.Add(project);
    }

    /// <summary>
    /// Start the <see cref="ServerTaskManager"/>.
    /// This generates a new thread.
    /// </summary>
    public void Start()
    {
        // Create a new thread
        _thread = new Thread(Run);

        // Set the active flag to true
        _active = true;

        // Start the thread
        _thread.Start();
    }

    private void Run()
    {
        while (Running)
        {
            // Initialize a queue of server tasks
            Queue<(ServerTask serverTask, ServerTaskProject taskProject)> taskQueue = new();

            // Go through each of the server task projects and add their ready tasks to the queue
            foreach (var project in _serverTaskProjects)
            {
                while (project.NextItem is { ReadyToRun: true })
                {
                    (ServerTask task, ServerTaskProject project) queueItem = (project.PopTask(), project);

                    taskQueue.Enqueue(queueItem);
                }
            }

            // While the task queue has tasks, pop and run them
            while (taskQueue.Count > 0)
            {
                // Get the next task
                var queueItem = taskQueue.Dequeue();
                var task = queueItem.serverTask;
                var project = queueItem.taskProject;

                // Create a new C# task that will run the server task
                var cTask = task.CreateTask(this, project);

                // Hook up the events to manage the active tasks
                task.OnStarted += ManageActiveTasks;
                task.OnCompleted += ManageActiveTasks;
                task.OnFailed += ManageActiveTasks;

                // Start the C# task
                cTask.Start();
            }

            // Wait for the next update
            Thread.Sleep(1000 / UpdatesPerSecond);
        }
    }

    /// <summary>
    /// Stop the <see cref="ServerTaskManager"/>.
    /// This stops the thread.
    /// </summary>
    public void Stop()
    {
        // Set the active flag to false
        _active = false;

        // Stop running the thread
        _thread.Join();
    }

    private void ManageActiveTasks(ServerTask sender, ServerTaskEventArgs e)
    {
        switch (e.ServerTaskEventType)
        {
            // If the task is started, add it to the active tasks dictionary
            case ServerTaskEventType.Started:
            {
                lock (_activeTasksLock)
                {
                    // If the project is not in the active tasks dictionary, add it
                    if (!_activeTasks.ContainsKey(e.ServerTaskProject))
                        _activeTasks.Add(e.ServerTaskProject, new List<ServerTask>());

                    // Add the task to the active tasks dictionary
                    _activeTasks[e.ServerTaskProject].Add(e.ServerTask);
                }

                // Log the event
                Console.WriteLine($"Task {e.ServerTask.TaskName} was added to the active tasks list.");

                break;
            }

            // If the task is completed or fails, remove it from the active tasks dictionary
            case ServerTaskEventType.Completed:
            case ServerTaskEventType.Failed:
            {
                lock (_activeTasksLock)
                {
                    // Remove the task from the active tasks dictionary
                    _activeTasks[e.ServerTaskProject].Remove(e.ServerTask);

                    // If the project has no more tasks, remove it from the active tasks dictionary
                    if (_activeTasks[e.ServerTaskProject].Count == 0)
                        _activeTasks.Remove(e.ServerTaskProject);
                }

                // Log the event
                Console.WriteLine($"Task {e.ServerTask.TaskName} was removed from the active tasks list.");

                break;
            }

            default:
                break;
        }

        Console.WriteLine($"Active Task Count: {ActiveTaskCount}");
    }
}