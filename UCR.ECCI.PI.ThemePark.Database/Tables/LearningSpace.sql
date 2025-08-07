CREATE TABLE [Infrastructure].[LearningSpace] (
	[LearningSpaceId] INT IDENTITY(1,1), -- PK / Internal Id
	[FloorId] INT NOT NULL, -- FK
	[Name] VARCHAR(50) NOT NULL,
	[MaxCapacity] INT,
	[Type] VARCHAR(50) NOT NULL,
    [Width] DECIMAL(5,2),
    [Height] DECIMAL(5,2),
    [Length] DECIMAL(5,2),
	[ColorFloor] VARCHAR(50) NOT NULL,
	[ColorWalls] VARCHAR(50) NOT NULL,
	[ColorCeiling] VARCHAR(50) NOT NULL,
	CONSTRAINT UQ_FloorId_Name UNIQUE ([FloorId], [Name]),
	CONSTRAINT PK_LearningSpace PRIMARY KEY([LearningSpaceId]),
	CONSTRAINT FK_Floor_LearningSpace 
		FOREIGN KEY([FloorId]) 
		REFERENCES Infrastructure.Floor([FloorId])
		ON DELETE CASCADE
		ON UPDATE CASCADE
);
