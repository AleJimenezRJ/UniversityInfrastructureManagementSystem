namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;

public class DomainException : Exception
{
    /// <summary>
    /// Creates a new instance of the <see cref="DomainException"/> class with a specified error message.
    /// </summary>
    /// <param name="userFriendlyMessage"></param>
    public DomainException(string userFriendlyMessage) : base(userFriendlyMessage)
    {
    }
}
