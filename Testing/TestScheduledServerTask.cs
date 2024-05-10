using Task_Server_2.ServerTasks;

namespace Task_Server_2.Testing;

public class TestScheduledServerTask : ScheduledServerTask
{
    // The number of iterations to run the task
    private readonly int _iterations;
    
    public TestScheduledServerTask(DateTime scheduledTime, int iterations)
        : base($"Scheduled Test Task with {iterations} iterations @ {scheduledTime}", scheduledTime, false)
    {
        _iterations = iterations;

        OnCompleted += delegate { Console.WriteLine($""); };
    }

    protected override void TaskLogic(ServerTaskManager serverTaskManager, ServerTaskProject serverTaskProject)
    {
        Console.WriteLine($"{TaskName}");

        for (var i = 0; i < _iterations; i++)
        {
            Thread.Sleep(100);
            Console.WriteLine($"\tTask iteration {i + 1}");
        }
    }
}