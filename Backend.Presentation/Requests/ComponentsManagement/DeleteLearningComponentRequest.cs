namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;

/// <summary>
/// Represents a request to delete a learning component identified by its unique ID.
/// </summary>
/// <param name="Id">The unique identifier of the learning component to be deleted.</param>
public record class DeleteLearningComponentRequest(int Id);