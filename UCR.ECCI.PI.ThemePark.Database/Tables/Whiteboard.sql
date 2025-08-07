CREATE TABLE [Infrastructure].[Whiteboard]
(
	ComponentId INT,
    MarkerColor VARCHAR(20),
    PRIMARY KEY (ComponentId),
    FOREIGN KEY (ComponentId)
    REFERENCES Infrastructure.LearningComponent(ComponentId)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);