namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;

public class DuplicatedEntityException : DomainException
{
    /// <summary>
    /// Creates a new instance of the <see cref="DuplicatedEntityException"/> class with a specified error message.
    /// </summary>
    /// <param name="userFriendlyMessage"></param>
    public DuplicatedEntityException(string userFriendlyMessage) : base(userFriendlyMessage)
    {
    }
}
