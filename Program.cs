using System.Diagnostics;
using Task_Server_2.DebugLogger;
using Task_Server_2.DebugLogger.LogOutput;
using Task_Server_2.ServerTasks;
using Task_Server_2.Testing;

namespace Task_Server_2;

public static class Program
{
    public static void Main(string[] args)
    {
        // Change where the log messages are outputted
        DebugLog.Instance.AddLogOutput(new FileOutput("Test.txt"));
        DebugLog.Instance.AddLogOutput(new RealTimeConsoleLogOutput());

        // Create a new server task project
        var serverTaskProject = new TestServerTaskProject();

        // Start the server task manager
        ServerTaskManager.Instance.Start(serverTaskProject);

        // Run the project
        serverTaskProject.Start();

        // Stop the server task manager
        ServerTaskManager.Instance.Stop();
    }

    public static void SecondConsoleTest()
    {
        // Create a new process for the second console
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "gnome-terminal",
                Arguments = "--wait -- zsh -c \"echo bruh && sleep 10\"",

                RedirectStandardInput = false,
                RedirectStandardOutput = false,
                UseShellExecute = false,
                CreateNoWindow = false
            }
        };

        // Start the process
        process.Start();

        process.WaitForExit();
    }
}