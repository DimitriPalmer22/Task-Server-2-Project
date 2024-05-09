using Task_Server_2.ServerTasks;

namespace Task_Server_2.Testing;

public class TestOneTimeServerTask : OneTimeServerTask
{
    // The number of iterations to run the task
    private readonly int _iterations;

    public TestOneTimeServerTask(int iterations)
        : base($"Test one time task with {iterations} iterations", false)
    {
        _iterations = iterations;

        OnCompleted += delegate { Console.WriteLine($""); };
    }

    protected override void TaskLogic(ServerTaskManager serverTaskManager)
    {
        Console.WriteLine($"{TaskName}");

        for (var i = 0; i < _iterations; i++)
        {
            Thread.Sleep(100);
            Console.WriteLine($"\tTask iteration {i + 1}");
        }
    }
}