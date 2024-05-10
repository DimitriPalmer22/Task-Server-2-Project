using System.Collections;
using System.Drawing;
using Task_Server_2.DebugLogger.LogOutput;

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
    /// Controls where the log messages are outputted to.
    /// </summary>
    private readonly List<LogOutput.LogOutput> _logOutputs = new();
    
    /// <summary>
    /// A lock object to prevent multiple threads from accessing the message log at the same time.
    /// </summary>
    private readonly object _messageLogLock = new();
    
    /// <summary>
    /// A lock object to prevent multiple threads from accessing the console at the same time.
    /// </summary>
    private readonly object _consoleLock = new();

    /// <summary>
    /// Controls where the log messages are outputted to.
    /// </summary>
    public IReadOnlyList<LogOutput.LogOutput> LogOutputs => _logOutputs;

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


    private DebugLog()
    {
        // Initialize the log outputs
        AddLogOutput(new ConsoleLogOutput());
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
    
    public void AddLogOutput(LogOutput.LogOutput logOutput)
    {
        _logOutputs.Add(logOutput);
        
        logOutput.HookToEvents();
    }
    
    public void RemoveLogOutput(LogOutput.LogOutput logOutput)
    {
        _logOutputs.Remove(logOutput);
     
        logOutput.UnhookFromEvents();
    }
    
    public void ClearLogOutputs()
    {
        foreach (var logOutput in _logOutputs)
            logOutput.UnhookFromEvents();
        
        _logOutputs.Clear();
    }

    /// <summary>
    /// A thread-safe method to write a line to the console.
    /// </summary>
    /// <param name="obj"></param>
    public void WriteLine(object obj)
    {
        // If the object is null, return
        if (obj == null)
            return;
        
        // Write the object to the console
        lock (_consoleLock)
        {
            Console.WriteLine(obj);
        }
    }

    public void Write(object obj)
    {
        // If the object is null, return
        if (obj == null)
            return;
        
        // Write the object to the console
        lock (_consoleLock)
        {
            Console.Write(obj);
        }
    }

    public void Write(params object[] objs)
    {
        // If the object is null, return
        if (objs == null || objs.Length == 0)
            return; 
        
        // Write the objects to the console
        lock (_consoleLock)
        {
            foreach (var obj in objs)
                Console.Write(obj);
        }
    }
}