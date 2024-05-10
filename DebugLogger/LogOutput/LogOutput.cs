using System.Drawing;
using Task_Server_2.ServerTasks;

namespace Task_Server_2.DebugLogger.LogOutput;

/// <summary>
/// Determines where the log messages are outputted to.
/// </summary>
public abstract class LogOutput
{
    /// <summary>
    /// A dictionary of log levels and their corresponding colors.
    /// </summary>
    protected static readonly Dictionary<LogType, LogColor> LogLevelColors = new()
    {
        { LogType.Normal, new LogColor(Color.White) },
        { LogType.Warning, new LogColor(Color.Black, Color.Yellow, LogColor.BOLD) },
        { LogType.Error, new LogColor(Color.Black, Color.Red, LogColor.BOLD) },
        { LogType.Event, new LogColor(Color.CornflowerBlue, null, LogColor.ITALIC) },
        { LogType.TaskManager, new LogColor(Color.Gray, null, LogColor.ITALIC) },
    };


    /// <summary>
    /// Add functions to the events of the Server Task Manager and Debug Logger.
    /// </summary>
    public abstract void HookToEvents();

    /// <summary>
    /// Remove functions from the events of the Server Task Manager and Debug Logger.
    /// </summary>
    public abstract void UnhookFromEvents();

    /// <summary>
    /// Generate the output for a single log message.
    /// </summary>
    protected virtual string ConstructOutput(LogMessage message)
    {
        // Get the color for the log type
        var logColor = LogLevelColors[message.LogType];

        // Create the sub message string
        var subMessageString = "";
        foreach (var (level, subMessage) in message.SubMessages)
            subMessageString += $"\n\t{new string('\t', level)}{logColor}{subMessage}{LogColor.RESET}";
        
        // Create the main string
        var mainString = $"{logColor}({message.DateTime:MM/dd/yyyy hh:mm:ss tt}) [{message.LogType.ToString().ToUpper(), -12}]: {message.Message}{LogColor.RESET}";

        // Return the main string with the sub message string
        return mainString + subMessageString;
    }    
    
    /// <summary>
    /// Display the output for a single log message.
    /// </summary>
    public abstract string Output(LogMessage message);

    public abstract string Output(IEnumerable<LogMessage> messages);

    public abstract string OutputAll();

    protected void OutputAllOnTaskManagerEvent(ServerTaskManager sender, TaskManagerEventArgs e)
    {
        OutputAll();
    }
}