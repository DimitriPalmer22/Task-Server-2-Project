using System.Diagnostics.CodeAnalysis;

namespace Task_Server_2.DebugLogger;

public record LogMessage
{
    public required LogType LogType { get; init; }

    public required string Message { get; init; }

    public required DateTime DateTime { get; init; }
    
    /// <summary>
    /// A list of sub-messages that are associated with this log message.
    /// The level of the sub-message determines the indentation level.
    /// The message is the actual string of the sub message.
    /// </summary>
    private readonly List<(int level, string message)> _subMessages = new();

    /// <summary>
    /// Get a read-only collection of the sub messages.
    /// </summary>
    public IReadOnlyCollection<(int level, string message)> SubMessages => _subMessages;
    
    [SetsRequiredMembers]
    public LogMessage(LogType logType, string message, params (int level, string message)[] subMessages)
    {
        LogType = logType;
        Message = message;

        DateTime = DateTime.Now;

        foreach (var (level, subMessage) in subMessages)
            AddSubMessage(level, subMessage);
    }
    
    /// <summary>
    /// Add a sub message to the log message.
    /// </summary>
    /// <param name="level">The level of indentation for the message. Start from 0.</param>
    /// <param name="message">The actual string of the sub message</param>
    public void AddSubMessage(int level, string message)
    {
        _subMessages.Add((level, message));
    }

    public override string ToString()
    {
        // Create the sub message string
        var subMessageString = "";
        foreach (var (level, subMessage) in _subMessages)
            subMessageString += $"\n\t{new string('\t', level)}{subMessage}";
        
        // Create the main string
        var mainString = $"({DateTime:MM/dd/yyyy hh:mm:ss tt}) [{LogType.ToString().ToUpper(), -12}]: {Message}";

        // Return the main string with the sub message string
        return mainString + subMessageString;
    }
}