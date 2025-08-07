namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

public class ConcurrencyConflictException : DomainException
{
    public ConcurrencyConflictException(string userFriendlyMessage) : base(userFriendlyMessage)
    {
    }
}
