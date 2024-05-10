using System.Collections;
using System.Drawing;

namespace Task_Server_2.DebugLogger;

public sealed class DebugLog
{
    /// <summary>
    /// The singleton instance of the debug log.
    /// </summary>
    private static DebugLog _instance;

    /// <summary>
    /// The list of log messages.
    /// </summary>
    private readonly List<LogMessage> _messageLog = new();

    /// <summary>
    /// A lock object to prevent multiple threads from accessing the message log at the same time.
    /// </summary>
    private readonly object _messageLogLock = new();

    /// <summary>
    /// A dictionary of log levels and their corresponding colors.
    /// </summary>
    private readonly Dictionary<LogType, LogColor> _logLevelColors = new()
    {
        { LogType.Normal, new LogColor(Color.White) },
        { LogType.Warning, new LogColor(Color.Black, Color.Yellow, LogColor.BOLD) },
        { LogType.Error, new LogColor(Color.Black, Color.Red, LogColor.BOLD) },
        { LogType.Event, new LogColor(Color.CornflowerBlue, null, LogColor.ITALIC) },
        { LogType.TaskManager, new LogColor(Color.Gray, null, LogColor.ITALIC) },
    };

    /// <summary>
    /// The singleton instance of the debug log.
    /// </summary>
    public static DebugLog Instance
    {
        get
        {
            if (_instance == null)
                _instance = new DebugLog();

            return _instance;
        }
    }

    /// <summary>
    /// A read-only list of the log messages.
    /// </summary>
    public IReadOnlyList<LogMessage> MessageLog
    {
        get
        {
            IReadOnlyList<LogMessage> messageLog;

            // Clone the message log to prevent modification
            lock (_messageLogLock)
                messageLog = _messageLog.ToList();

            return messageLog;
        }
    }

    /// <summary>
    /// A read-only dictionary of log level colors.
    /// </summary>
    public IReadOnlyDictionary<LogType, LogColor> LogLevelColors => _logLevelColors;

    private DebugLog()
    {
    }

    /// <summary>
    /// Add a log message to the message log.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="message"></param>
    /// <returns>The log message that was added to the debug log.</returns>
    public LogMessage Log(LogType type, string message)
    {
        // Create a new log message
        var logMessage = new LogMessage(type, message);

        // Add the log message to the message log
        lock (_messageLogLock)
            _messageLog.Add(logMessage);

        return logMessage;
    }
}