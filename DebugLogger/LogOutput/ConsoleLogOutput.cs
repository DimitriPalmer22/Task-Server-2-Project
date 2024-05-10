using System.Text;
using Task_Server_2.ServerTasks;

namespace Task_Server_2.DebugLogger.LogOutput;

/// <summary>
/// Output the contents of the debug log to the console.
/// </summary>
public class ConsoleLogOutput : LogOutput
{
    public override void HookToEvents()
    {
        ServerTaskManager.Instance.OnStopped += OutputAllOnTaskManagerEvent;
    }

    public override void UnhookFromEvents()
    {
        ServerTaskManager.Instance.OnStopped -= OutputAllOnTaskManagerEvent;
    }


    public override string Output(LogMessage message)
    {
        // Construct the output
        var output = ConstructOutput(message);

        // Output the message to the console
        DebugLog.Instance.WriteLine(output);

        return output;
    }

    public override string Output(IEnumerable<LogMessage> messages)
    {
        var sb = new StringBuilder();

        foreach (var message in messages)
            sb.Append(Output(message) + "\n");

        return sb.ToString();
    }

    public override string OutputAll()
    {
        var output = Output(DebugLog.Instance.MessageLog);
        return output;
    }
}