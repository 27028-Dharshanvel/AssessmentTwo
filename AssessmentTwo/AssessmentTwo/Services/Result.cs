namespace AssessmentTwo.Services;

/// <summary>
/// Result pattern to handle errors gracefully.
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }

    public Result(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Returns a result instance with true 
    /// </summary>
    /// <returns>Result</returns>
    public static Result Success()
    {
        return new Result(true, string.Empty);
    }

    /// <summary>
    /// Returns a result instance with false along with the errormessage.
    /// </summary>
    /// <param name="errorMessage">errormessage</param>
    /// <returns>Result</returns>
    public static Result Failure(string errorMessage)
    {
        return new Result(false, errorMessage);
    }
}