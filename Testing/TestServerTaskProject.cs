using Task_Server_2.ServerTasks;

namespace Task_Server_2.Testing;

public class TestServerTaskProject : ServerTaskProject
{
    /// <summary>
    /// A flag used to determine if project is running.
    /// </summary>
    private bool _running;

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

            switch (key)
            {
                case ConsoleKey.Q:
                    Console.WriteLine($"\tEnqueueing a new task");
                    EnqueueTask(new TestOneTimeServerTask(5));
                    break;

                case ConsoleKey.E:
                    _running = false;
                    break;
            }
        }
    }
}