using Task_Server_2.ServerTasks;
using Task_Server_2.Testing;

namespace Task_Server_2;

public static class Program
{
    public static void Main(string[] args)
    {
        // // Create a list of tasks to test
        // var tasks = new ServerTask[]
        // {
        //     new TestOneTimeServerTask(5),
        //     new TestOneTimeServerTask(6),
        //     new TestOneTimeServerTask(7),
        //     new TestOneTimeServerTask(7),
        //     new TestOneTimeServerTask(200),
        //     new TestOneTimeServerTask(7),
        // };
        //
        // // Add the tasks to a task group
        // var taskGroup = new ServerTaskGroup("Test task group", ServerTaskGroupType.Sequential, tasks);
        //
        // // Run the task group
        // taskGroup.Run(null);

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
    }
}