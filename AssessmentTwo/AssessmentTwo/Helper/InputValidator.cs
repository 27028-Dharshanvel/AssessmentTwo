namespace AssessmentTwo.Helper;

public static class InputValidator
{
    /// <summary>
    /// Validates whether input string is integer.
    /// </summary>
    /// <param name="input">input</param>
    /// <param name="result">result</param>
    /// <returns>True if valid, False otherwise</returns>
    public static bool IsValidInt(string input, out int result)
    {
        return int.TryParse(input, out result);
    }

    /// <summary>
    /// Validates input string is within range
    /// </summary>
    /// <param name="value">value</param>
    /// <param name="min">minimum value</param>
    /// <param name="max">maximum value</param>
    /// <returns>True if valid,False otherwise.</returns>
    public static bool IsIntWithinRange(int value, int min, int max)
    {
        return value >= min && value <= max;
    }
}