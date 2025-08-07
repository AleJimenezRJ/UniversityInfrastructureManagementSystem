namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

public class ValidationError
{
    public string Parameter { get; }
    public string Message { get; }

    public ValidationError(string parameter, string message)
    {
        Parameter = parameter;
        Message = message;
    }
}