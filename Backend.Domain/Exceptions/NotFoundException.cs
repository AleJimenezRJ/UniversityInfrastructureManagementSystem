namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string userFriendlyMessage) : base(userFriendlyMessage)
    {
    }
}
