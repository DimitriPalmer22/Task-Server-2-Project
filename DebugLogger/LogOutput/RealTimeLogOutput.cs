using System.Text;
using Task_Server_2.ServerTasks;

namespace Task_Server_2.DebugLogger.LogOutput;

public abstract class RealTimeLogOutput : LogOutput
{
    /// <summary>
    /// A dictionary to keep track of the log messages that have been outputted already.
    /// </summary>
    private readonly Dictionary<LogMessage, string> _hasBeenOutputted = new();

    public override void HookToEvents()
    {
        ServerTaskManager.Instance.OnUpdateComplete += OutputAllOnTaskManagerEvent;
    }

    public override void UnhookFromEvents()
    {
        ServerTaskManager.Instance.OnUpdateComplete -= OutputAllOnTaskManagerEvent;
    }

    public override string Output(LogMessage message)
    {
        // If the message has already been outputted, return an empty string
        if (_hasBeenOutputted.ContainsKey(message))
            return string.Empty;

        // Output the message
        var output = CustomOutput(message);
        
        // Add the message to the dictionary
        _hasBeenOutputted[message] = output;

        return output;
    }

    public override string Output(IEnumerable<LogMessage> messages)
    {
        var sb = new StringBuilder();

        foreach (var message in messages)
            sb.Append(Output(message));

        return sb.ToString();
    }

    protected abstract string CustomOutput(LogMessage message);

    public override string OutputAll()
    {
        return Output(DebugLog.Instance.MessageLog);
    }

}