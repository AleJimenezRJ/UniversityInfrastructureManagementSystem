namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string userFriendlyMessage) : base(userFriendlyMessage)
    {
    }
}
