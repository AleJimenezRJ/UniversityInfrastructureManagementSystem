CREATE TABLE [Infrastructure].[LearningComponentAudit] (
    LearningComponentAuditId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,

    -- Base (LearningComponent)
    ComponentId INT NOT NULL,
    Width DECIMAL(5,2) NOT NULL,
    Height DECIMAL(5,2) NOT NULL,
    Depth DECIMAL(5,2) NOT NULL,
    X DECIMAL(9,6) NOT NULL,
    Y DECIMAL(9,6) NOT NULL,
    Z DECIMAL(9,6) NOT NULL,
    Orientation VARCHAR(20),
    IsDeleted BIT NOT NULL,

    -- Discriminator
    ComponentType VARCHAR(20) NOT NULL, -- 'Whiteboard' o 'Projector'

    -- Whiteboard
    MarkerColor VARCHAR(20) NULL,

    -- Projector
    ProjectedContent VARCHAR(255) NULL,
    ProjectedHeight DECIMAL(5,2) NULL,
    ProjectedWidth DECIMAL(5,2) NULL,

    -- Log
    Action NVARCHAR(20) NOT NULL, -- 'Created', 'Updated', 'Deleted'
    ModifiedAt DATETIME NOT NULL
);