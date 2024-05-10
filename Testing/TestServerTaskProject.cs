using Task_Server_2.ServerTasks;
using Task_Server_2.ServerTasks.ActivationConditions;

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
    }

    public void Start()
    {
        _running = true;
        Run();
    }

    private void Run()
    {
        while (_running)
        {
            Console.WriteLine("Press a key");
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
        void WrapperBody1()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Wrapper iteration {i + 1}");
                Thread.Sleep(100);
            }
        }

        void WrapperBody2(int iterations, string message, int waitTime)
        {
            for (int i = 0; i < iterations; i++)
            {
                Console.WriteLine($"{message}. Iteration {i + 1}");
                Thread.Sleep(waitTime);
            }
        }

        List<ServerTask> wrapperTasks =
        [
            new FunctionWrapperServerTask("Wrapper 1", new SimpleActivationCondition(), WrapperBody1),
            new FunctionWrapperServerTask(
                "Wrapper 2", new SimpleActivationCondition(), WrapperBody2,
                5, "Wrapper 2 Dynamic Invoke", 200
            )
        ];

        // Create a task group
        var taskGroup =
            new ServerTaskGroup("Wrapper Task Group", ServerTaskGroupType.Sequential, wrapperTasks.ToArray());

        // Enqueue the task group
        EnqueueTask(taskGroup);
    }

    private void ExceptionTaskTest()
    {
        EnqueueTask(new FunctionWrapperServerTask(
            "Exception Task", new SimpleActivationCondition(),
            () => { throw new Exception("This is an exception"); }
        ));
    }

    #endregion
}