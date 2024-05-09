using Task_Server_2.Tasks;
using Task_Server_2.Testing;

namespace Task_Server_2;

public static class Program
{
    public static void Main(string[] args)
    {
        // Create a list of tasks to test
        var tasks = new ServerTask[]
        {
            new TestOneTimeServerTask(5),
            new TestOneTimeServerTask(6),
            new TestOneTimeServerTask(7),
        };

        // Run the tasks asynchronously
        foreach (var serverTask in tasks)
            Task.Run(() => serverTask.Run(null));
        
    }
}