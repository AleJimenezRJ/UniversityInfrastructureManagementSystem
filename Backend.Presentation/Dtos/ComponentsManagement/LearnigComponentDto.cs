using System.Text.Json.Serialization;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

/// <summary>
/// Data Transfer Object (DTO) for a learning component, used to expose
/// minimal and structured information through the API.
/// </summary>
/// <param name="Id">The unique identifier of the learning component.</param>
/// <param name="Orientation">The orientation or facing direction of the component.</param>
/// <param name="Position">The position of the component in the learning space.</param>
/// <param name="Dimensions">The dimensions of the component.</param>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "@odata.type")]
[JsonDerivedType(typeof(ProjectorDto), "#namespace.ProjectorDto")]
[JsonDerivedType(typeof(WhiteboardDto), "#namespace.WhiteboardDto")]
public abstract record LearningComponentDto(
    int Id,
    string Orientation,
    PositionDto Position,
    DimensionsDto Dimensions
);