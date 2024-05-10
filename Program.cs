using Task_Server_2.DebugLogger;
using Task_Server_2.ServerTasks;
using Task_Server_2.Testing;

namespace Task_Server_2;

public static class Program
{
    public static void Main(string[] args)
    {
        // Get the instance of the server task manager
        var serverTaskManager = ServerTaskManager.Instance;

        // Start the server task manager
        serverTaskManager.Start();

        // Create a new server task project
        var serverTaskProject = new TestServerTaskProject();

        // Add the project to the server task manager
        serverTaskManager.AddProject(serverTaskProject);
        
        // Run the project
        serverTaskProject.Start();
        
        // Stop the server task manager
        serverTaskManager.Stop();
        
        // Print the log messages
        PrintLogMessages();
    }
    
    private static void PrintLogMessages()
    {
        Console.WriteLine("Log Messages:");
        foreach (var message in DebugLog.Instance.MessageLog)
            Console.WriteLine($"{message}");
    }
}