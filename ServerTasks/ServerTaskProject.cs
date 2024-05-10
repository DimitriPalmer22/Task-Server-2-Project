namespace Task_Server_2.ServerTasks;

public abstract class ServerTaskProject
{
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
}