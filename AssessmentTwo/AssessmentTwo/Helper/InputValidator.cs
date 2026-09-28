namespace AssessmentTwo.Helper;

public static class InputValidator
{
    public static bool IsValidInt(string input, out int result)
    {
        return int.TryParse(input, out result);
    }
    public static bool IsValidString(string input)
    {
        return !string.IsNullOrWhiteSpace(input);
    }

    public static bool IsIntWithinRange(int value, int min, int max)
    {
        return value >= min && value <= max;
    }
}