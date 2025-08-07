using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement
{
    /// <summary>
    /// Represents the response returned when retrieving all registered users.
    /// </summary>
    public class GetAllUsersResponse
    {
        /// <summary>
        /// Gets or sets the list of users retrieved from the system.
        /// </summary>
        public List<UserDto> Users { get; set; } = new List<UserDto>();
    }
}