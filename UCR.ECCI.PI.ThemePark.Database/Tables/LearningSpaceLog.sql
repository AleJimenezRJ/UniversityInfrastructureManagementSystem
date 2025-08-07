CREATE TABLE [Infrastructure].[LearningSpaceLog] (
    [LearningSpaceLogInternalId] INT IDENTITY(1,1) NOT NULL,
    [Name] VARCHAR(50) NOT NULL,
    [Type] VARCHAR(50) NOT NULL,
    [MaxCapacity] INT NOT NULL,
    [Width] DECIMAL(5,2) NOT NULL,
    [Height] DECIMAL(5,2) NOT NULL,
    [Length] DECIMAL(5,2) NOT NULL,
    [ColorFloor] VARCHAR(50) NOT NULL,
    [ColorWalls] VARCHAR(50) NOT NULL,
    [ColorCeiling] VARCHAR(50) NOT NULL,
    [ModifiedAt] DATETIME NOT NULL,
    [Action] NVARCHAR(20) NOT NULL,
    CONSTRAINT PK_LearningSpaceLog PRIMARY KEY ([LearningSpaceLogInternalId])
);