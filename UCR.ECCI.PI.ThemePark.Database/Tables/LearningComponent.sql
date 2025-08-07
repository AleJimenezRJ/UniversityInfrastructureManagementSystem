CREATE TABLE [Infrastructure].[LearningComponent] (
	[ComponentId] INT IDENTITY(1,1) NOT NULL, -- PK
	[LearningSpaceId] INT NOT NULL, -- FK
	[Width] DECIMAL(5,2),
	[Height] DECIMAL(5,2),
	[Depth] DECIMAL(5,2),
	[X] DECIMAL(9,6),
	[Y] DECIMAL(9,6),
	[Z] DECIMAL(9,6),
	[Orientation] VARCHAR(20),
	[IsDeleted] BIT NOT NULL,
	CONSTRAINT PK_LearningComponent PRIMARY KEY([ComponentId]),
	CONSTRAINT FK_LearningSpace_LearningComponent 
		FOREIGN KEY([LearningSpaceId]) 
		REFERENCES Infrastructure.LearningSpace([LearningSpaceId])
		ON DELETE CASCADE
		ON UPDATE CASCADE
);
