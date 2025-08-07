using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.Spaces;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;

/// <summary>
/// Provides implementation for learning space log-related operations defined in <see cref="ILearningSpaceLogServices"/>.
/// </summary>
internal class LearningSpaceLogServices : ILearningSpaceLogServices
{
    private readonly ILearningSpaceLogRepository _learningSpaceLogRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningSpaceLogServices"/> class.
    /// </summary>
    /// <param name="learningSpaceLogRepository">The repository used to access learning space log data.</param>
    public LearningSpaceLogServices(ILearningSpaceLogRepository learningSpaceLogRepository)
    {
        _learningSpaceLogRepository = learningSpaceLogRepository;
    }

    /// <summary>
    /// Retrieves a list of learning space logs asynchronously.
    /// </summary>
    /// <returns>A list of <see cref="LearningSpaceLog"/> entries.</returns>
    public async Task<List<LearningSpaceLog>> ListLearningSpaceLogsAsync()
    {
        return await _learningSpaceLogRepository.ListLearningSpaceLogsAsync();
    }
}
