namespace PomodoroApi.Services;

public enum ResultType
{
    Success,
    Failure
}
public record StandardServiceResult
{
    public ResultType Result { get; set; }
}