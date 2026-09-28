namespace AssessmentTwo.Helper;

/// <summary>
/// Class for displaying console messages in different colors.
/// </summary>
internal class OutputColor
{
    /// <summary>
    /// Display error message in red color.
    /// </summary>
    public static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{message}");
        Console.ResetColor();
    }

    /// <summary>
    /// Display success message in green color.
    /// </summary>
    public static void Success(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{message}");
        Console.ResetColor();
    }

    /// <summary>
    /// Display warning message in yellow color.
    /// </summary>
    public static void Warn(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{message}");
        Console.ResetColor();
    }

    /// <summary>
    /// Display warning message in blue color.
    /// </summary>
    public static void Info(string message)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{message}");
        Console.ResetColor();
    }

    /// <summary>
    /// Display warning message in grey color.
    /// </summary>
    public static void Shade(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"{message}");
        Console.ResetColor();
    }
}
