namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement
{
    /// <summary>
    /// Represents an audit record for changes made to a learning component.
    /// </summary>
    public class LearningComponentAudit
    {
        /// <summary>
        /// Gets or sets the unique identifier for the audit record.
        /// </summary>
        public int LearningComponentAuditId { get; set; }

        /// <summary>
        /// Gets or sets the component id.
        /// </summary>
        public int ComponentId { get; set; }

        /// <summary>
        /// Gets or sets the width of the component.
        /// </summary>
        public decimal? Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the component.
        /// </summary>
        public decimal? Height { get; set; }

        /// <summary>
        /// Gets or sets the depth of the component.
        /// </summary>
        public decimal? Depth { get; set; }

        /// <summary>
        /// Gets or sets the X position of the component.
        /// </summary>
        public decimal? X { get; set; }

        /// <summary>
        /// Gets or sets the Y position of the component.
        /// </summary>
        public decimal? Y { get; set; }

        /// <summary>
        /// Gets or sets the Z position of the component.
        /// </summary>
        public decimal? Z { get; set; }

        /// <summary>
        /// Gets or sets the orientation of the component.
        /// </summary>
        public string? Orientation { get; set; }

        /// <summary>
        /// Gets or sets whether the component was logically deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the type of the component (e.g., "Whiteboard", "Projector").
        /// </summary>
        public string ComponentType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the color of the whiteboard marker, if applicable.
        /// </summary>
        public string? MarkerColor { get; set; }

        /// <summary>
        /// Gets or sets the projected content of a projector, if applicable.
        /// </summary>
        public string? ProjectedContent { get; set; }

        /// <summary>
        /// Gets or sets the projected height of a projector, if applicable.
        /// </summary>
        public decimal? ProjectedHeight { get; set; }

        /// <summary>
        /// Gets or sets the projected width of a projector, if applicable.
        /// </summary>
        public decimal? ProjectedWidth { get; set; }

        /// <summary>
        /// Gets or sets the action performed (e.g., "Created", "Updated", "Deleted").
        /// </summary>
        public string Action { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date and time when the modification occurred.
        /// </summary>
        public DateTime ModifiedAt { get; set; }
    }
}
