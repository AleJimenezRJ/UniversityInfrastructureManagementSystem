using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;


/// <summary>
/// Service class responsible for handling operations related to Staff entities.
/// </summary>
internal class StaffService : IStaffService
{
    private readonly IStaffRepository _staffRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="StaffService"/> class.
    /// </summary>
    /// <param name="staffRepository">The repository used for staff data persistence and retrieval.</param>
    public StaffService(IStaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
    }

    /// <summary>
    /// Asynchronously creates a new staff record in the data source.
    /// </summary>
    /// <param name="staff">The staff entity to be created.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean indicating success or failure.
    /// </returns>
    public Task<bool> CreateStaffAsync(Staff staff)
    {
        return _staffRepository.CreateStaffAsync(staff);
    }

    /// <summary>
    /// Asynchronously retrieves a list of students from the data source.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of students.
    /// </returns>
    public Task<List<Staff>?> ListStaffAsync()
    {
        return _staffRepository.ListStaffAsync();
    }
}
