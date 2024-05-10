using System.Drawing;

namespace Task_Server_2.DebugLogger;

public class LogColor(Color? foreground, Color? background = null, string effect = "")
{
    public const string RESET = "\u001b[0m";
    public const string BOLD = "\u001b[1m";
    public const string FAINT = "\u001b[2m";
    public const string ITALIC = "\u001b[3m";
    public const string UNDERLINE = "\u001b[4m";
    public const string BLINKING = "\u001b[5m";

    public const string INVERT = "\u001b[7m";
    public const string STRIKETHROUGH = "\u001b[9m";
    public const string DOUBLE_UNDERLINE = "\u001b[21m";
    public const string FRAMED = "\u001b[51m";
    public const string ENCIRCLED = "\u001b[52m";
    public const string OVERLINED = "\u001b[53m";


    /// <summary>
    /// The foreground color of the text.
    /// </summary>
    public Color? Foreground { get; set; } = foreground;

    /// <summary>
    /// The background color of the text.
    /// </summary>
    public Color? Background { get; set; } = background;

    /// <summary>
    /// ANSI escape code for text effects link underline, bold, etc.
    /// </summary>
    private string Effect { get; set; } = effect;

    public override string ToString()
    {
        return $"{ColorToAnsi(Foreground)}{ColorToAnsi(Background, false)}{Effect}";
    }

    private static string ColorToAnsi(Color? nullableColor, bool foreground = true)
    {
        if (nullableColor == null)
            return "";
        
        var color = nullableColor.Value;

        if (foreground)
            return $"\u001b[38;2;{color.R};{color.G};{color.B}m";

        return $"\u001b[48;2;{color.R};{color.G};{color.B}m";
    }
}