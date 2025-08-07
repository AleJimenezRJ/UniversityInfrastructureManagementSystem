namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;
public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entityType)
        : base(userFriendlyMessage: $"Failed to find requested entity of type {entityType}.")
    {
    }
}