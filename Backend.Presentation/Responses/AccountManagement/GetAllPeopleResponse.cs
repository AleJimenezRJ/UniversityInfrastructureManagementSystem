using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement
{
    /// <summary>
    /// Represents the response returned when retrieving all registered people.
    /// </summary>
    public class GetAllPeopleResponse
    {
        public List<PersonDto> People { get; set; } = new List<PersonDto>();
    }
}