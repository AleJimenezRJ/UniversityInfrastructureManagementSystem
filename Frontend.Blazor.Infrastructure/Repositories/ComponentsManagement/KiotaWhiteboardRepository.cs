using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.ComponentsManagement
{
    /// <summary>
    /// Repository implementation for whiteboards using Kiota-generated API client.
    /// Inherits from <see cref="KiotaLearningComponentRepository"/> to reuse shared logic.
    /// </summary>
    internal class KiotaWhiteboardRepository : KiotaLearningComponentRepository, IWhiteboardRepository
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="KiotaWhiteboardRepository"/> class.
        /// </summary>
        /// <param name="apiClient">The Kiota API client used for HTTP operations.</param>
        public KiotaWhiteboardRepository(ApiClient apiClient, IEnumerable<ILearningComponentRequestBuilder> professionalContentRequestBuilders) : base(apiClient,professionalContentRequestBuilders )
        {
        }
    }
}
