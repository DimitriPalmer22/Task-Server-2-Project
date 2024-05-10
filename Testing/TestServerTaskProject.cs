using Task_Server_2.DebugLogger;
using Task_Server_2.ServerTasks;
using Task_Server_2.ServerTasks.ActivationConditions;
using Task_Server_2.ServerTasks.HelperServerTasks;

namespace Task_Server_2.Testing;

public class TestServerTaskProject : ServerTaskProject
{
    /// <summary>
    /// A flag used to determine if project is running.
    /// </summary>
    private bool _running;

    /// <summary>
    /// A dictionary of keys and their corresponding functions.
    /// </summary>
    private readonly Dictionary<ConsoleKey, Action> _keyActions = new();

    public TestServerTaskProject()
    {
        InitializeKeyActions();
    }

    private void InitializeKeyActions()
    {
        _keyActions.Add(ConsoleKey.Q, StopProject);

        _keyActions.Add(ConsoleKey.D1, TestOneTimeTask);
        _keyActions.Add(ConsoleKey.D2, TestTaskGroup);
        _keyActions.Add(ConsoleKey.D3, TestScheduledTask);
        _keyActions.Add(ConsoleKey.D4, ActionWrapperTest);
        _keyActions.Add(ConsoleKey.D5, ExceptionTaskTest);
        _keyActions.Add(ConsoleKey.D6, () =>
        {
            var task = new FunctionWrapperServerTask(
                "Test Wrapper Task 2",
                new SimpleActivationCondition(),
                Program.SecondConsoleTest
            );
            EnqueueTask(task);
        });
        _keyActions.Add(ConsoleKey.D7, RecurringTaskTest);
    }

    public void Start()
    {
        _running = true;

        EnqueueTask(
            new FunctionWrapperServerTask(
                "Recurring Test",
                new RecurringActivationCondition(DateTime.Now, new TimeSpan(0, 1, 0 )),
                () =>
                {
                    DebugLog.Instance.WriteLine($" - Please Bro {DateTime.Now}");
                }
            )
        );

        Run();
    }

    private void Run()
    {
        while (_running)
        {
            DebugLog.Instance.WriteLine("Press a key");

            var key = Console.ReadKey(true).Key;

            if (_keyActions.TryGetValue(key, out var action))
                action();
        }
    }

    #region Key Actions

    private void StopProject()
    {
        _running = false;
    }

    private void TestOneTimeTask()
    {
        EnqueueTask(new TestServerTask(new SimpleActivationCondition(), 5));
    }

    private void TestTaskGroup()
    {
        List<ServerTask> tasks = new();

        for (int i = 0; i < 10; i++)
            tasks.Add(new TestServerTask(new SimpleActivationCondition(), i + 1));

        EnqueueTask(new ServerTaskGroup("Test Task Group", ServerTaskGroupType.Sequential, tasks.ToArray()));
    }

    private void TestScheduledTask()
    {
        var activationCondition = new ScheduledActivationCondition(DateTime.Now.AddSeconds(5));

        EnqueueTask(new TestServerTask(activationCondition, 5));
    }

    private void ActionWrapperTest()
    {
        const int constIterations = 10;
        const int constWaitTime = 100;

        void WrapperBody1()
        {
            // Get the start time
            var startTime = DateTime.Now;

            for (int i = 0; i < constIterations; i++)
                DebugLog.Instance.WriteLine($"Wrapper iteration {i + 1}");

            // Get the end time
            var endTime = DateTime.Now;

            // Calculate the elapsed time
            var elapsedTime = endTime - startTime;

            DebugLog.Instance.WriteLine($"Wrapper 1 took {elapsedTime.TotalMilliseconds} milliseconds to complete");
        }

        void WrapperBody2(int iterations, string message, int waitTime)
        {
            _ = waitTime;

            // Get the start time
            var startTime = DateTime.Now;

            for (int i = 0; i < iterations; i++)
                DebugLog.Instance.WriteLine($"{message}. Iteration {i + 1}");

            // Get the end time
            var endTime = DateTime.Now;

            // Calculate the elapsed time
            var elapsedTime = endTime - startTime;

            DebugLog.Instance.WriteLine($"Wrapper 2 took {elapsedTime.TotalMilliseconds} milliseconds to complete");
        }

        List<ServerTask> wrapperTasks =
        [
            new FunctionWrapperServerTask("Wrapper 1", new SimpleActivationCondition(), WrapperBody1),
            new FunctionWrapperServerTask(
                "Wrapper 2", new SimpleActivationCondition(), WrapperBody2,
                constIterations, "Wrapper 2 Dynamic Invoke", constWaitTime
            )
        ];

        // Create a task group
        var taskGroup =
            new ServerTaskGroup("Wrapper Task Group", ServerTaskGroupType.Asynchronous, wrapperTasks.ToArray());

        // Enqueue the task group
        EnqueueTask(taskGroup);
    }

    private void ExceptionTaskTest()
    {
        EnqueueTask(new FunctionWrapperServerTask(
            "Exception Task", new SimpleActivationCondition(),
            () => throw new TypeAccessException("Forced Exception Call")));
    }

    private void RecurringTaskTest()
    {
        var activationCondition = new RecurringActivationCondition(
            DateTime.Now, TimeSpan.FromSeconds(2), 5);

        EnqueueTask(new TestServerTask(activationCondition, 3));
    }

    #endregion
}