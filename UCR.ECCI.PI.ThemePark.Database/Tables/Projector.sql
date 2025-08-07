CREATE TABLE [Infrastructure].[Projector]
(
	ComponentId INT,
    ProjectedContent VARCHAR(255),
    ProjectedHeight DECIMAL(5,2),
    ProjectedWidth DECIMAL(5,2),
    PRIMARY KEY (ComponentId),
    FOREIGN KEY (ComponentId)
    REFERENCES Infrastructure.LearningComponent(ComponentId)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);