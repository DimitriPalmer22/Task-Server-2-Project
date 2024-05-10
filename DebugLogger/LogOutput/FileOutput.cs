using System.Diagnostics.CodeAnalysis;
using System.Text;
using Task_Server_2.ServerTasks;

namespace Task_Server_2.DebugLogger.LogOutput;

public class FileOutput : LogOutput
{
    /// <summary>
    /// Keep track of if the file has been opened.
    /// This is used to determine if the file should be cleared or not.
    /// </summary>
    private bool _fileHasBeenOpened;

    public required string Path { get; init; }

    [SetsRequiredMembers]
    public FileOutput(string path, bool clearFile = true)
    {
        Path = path;

        // If the file should be cleared, set the fileHasBeenOpened to true
        // This way, the file will not be cleared when it is opened
        _fileHasBeenOpened = !clearFile;
    }

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
        
        // If the file does not exist, create it
        if (!File.Exists(Path))
            File.Create(Path).Close();

        // Clear the file if it has been opened
        if (!_fileHasBeenOpened)
        {
            using var _ = new FileStream(Path, FileMode.Truncate, FileAccess.Write);
            _fileHasBeenOpened = true;
        }

        // Open the file stream
        using var fileStream = new FileStream(Path, FileMode.Append, FileAccess.Write);

        // Write the message to the file
        using var streamWriter = new StreamWriter(fileStream);
        streamWriter.WriteLine(output);

        return output;
    }

    public override string Output(IEnumerable<LogMessage> messages)
    {
        var output = new StringBuilder();

        foreach (var message in messages)
            output.Append(Output(message));

        return output.ToString();
    }

    public override string OutputAll()
    {
        return Output(DebugLog.Instance.MessageLog);
    }
}