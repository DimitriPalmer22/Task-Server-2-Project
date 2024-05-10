namespace Task_Server_2.DebugLogger.LogOutput;

public class RealTimeConsoleLogOutput : RealTimeLogOutput
{
    protected override string CustomOutput(LogMessage message)
    {
        // Construct the output
        var output = ConstructOutput(message);

        // Output the message to the console
        DebugLog.Instance.WriteLine(output);

        return output;
    }
}