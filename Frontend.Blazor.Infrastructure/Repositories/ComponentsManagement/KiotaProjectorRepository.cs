using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.ComponentsManagement;

/// <summary>
/// Implements <see cref="IProjectorRepository"/> using the Kiota-generated API client
/// to perform operations specific to projector components.
/// </summary>
internal class KiotaProjectorRepository : KiotaLearningComponentRepository, IProjectorRepository
{

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaProjectorRepository"/> class.
    /// </summary>
    /// <param name="apiClient">The Kiota API client used to communicate with the backend service.</param>
    public KiotaProjectorRepository(ApiClient apiClient, IEnumerable<ILearningComponentRequestBuilder> professionalContentRequestBuilders) : base(apiClient, professionalContentRequestBuilders)
    {
    }

}
