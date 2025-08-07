namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

public class ValidationException : DomainException
{
    public IReadOnlyList<ValidationError> Errors { get; }

    public ValidationException()
        : base("Validation failed.")
    {
        Errors = new List<ValidationError>();
    }

    public ValidationException(string message)
        : base(message)
    {
        Errors = new List<ValidationError>
        {
            new ValidationError(string.Empty, message)
        };
    }

    public ValidationException(IEnumerable<ValidationError> errors)
        : base("Validation failed.")
    {
        Errors = new List<ValidationError>(errors);
    }

    public ValidationException(string message, IEnumerable<ValidationError> errors)
        : base(message)
    {
        var list = new List<ValidationError> { new ValidationError(string.Empty, message) };
        list.AddRange(errors);
        Errors = list;
    }
}
