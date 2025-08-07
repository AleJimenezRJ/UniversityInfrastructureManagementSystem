using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;

/// <summary>
/// Represents a request to modify an existing projector's information.
/// </summary>
/// <param name="Projector">The data transfer object containing the updated details of the projector.</param>
public record class PutProjectorRequest(ProjectorNoIdDto Projector);
