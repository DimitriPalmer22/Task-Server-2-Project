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

    private void StopProject()
    {
        _running = false;
    }

    #endregion
}