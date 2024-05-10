using Task_Server_2.DebugLogger;
using Task_Server_2.ServerTasks;
using Task_Server_2.ServerTasks.ActivationConditions;

namespace Task_Server_2.Testing;

public class TestServerTask : ServerTask
{
    // The number of iterations to run the task
    private readonly int _iterations;

    public TestServerTask(IActivationCondition activationCondition, int iterations)
        : base($"Test one time task with {iterations} iterations", activationCondition)
    {
        _iterations = iterations;

        OnCompleted += delegate { DebugLog.Instance.WriteLine($""); };
    }

    protected override void TaskLogic(ServerTaskManager serverTaskManager, ServerTaskProject serverTaskProject)
    {
        DebugLog.Instance.WriteLine($"{TaskName}");

        for (var i = 0; i < _iterations; i++)
        {
            Thread.Sleep(100);
            DebugLog.Instance.WriteLine($"\tTask iteration {i + 1}");
        }
    }
}