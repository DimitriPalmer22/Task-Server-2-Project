using Task_Server_2.Tasks;

namespace Task_Server_2.Testing;

public class TestOneTimeServerTask : OneTimeServerTask
{
    // The number of iterations to run the task
    private readonly int _iterations;

    public TestOneTimeServerTask(int iterations)
        : base($"Test one time task with {iterations} iterations")
    {
        _iterations = iterations;
    }

    protected override void TaskLogic(ServerTaskManager serverTaskManager)
    {
        Console.WriteLine($"{taskName}");
        
        for (var i = 0; i < _iterations; i++)
            Console.WriteLine($"Task iteration {i + 1}");

        Console.WriteLine();
    }
}