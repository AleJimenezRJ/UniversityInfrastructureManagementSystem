CREATE TABLE [Infrastructure].[Floor] (
    [FloorId] INT IDENTITY(1,1), -- PK / Internal Id
    [BuildingId] INT NOT NULL, -- FK
    [Number] INT NOT NULL,
    CONSTRAINT UQ_BuildingId_FloorNumber UNIQUE([BuildingId], [Number]),
    CONSTRAINT PK_Floor PRIMARY KEY([FloorId]),
    CONSTRAINT FK_Building_Floor 
        FOREIGN KEY([BuildingId]) 
        REFERENCES [Infrastructure].[Building](BuildingInternalId)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);
