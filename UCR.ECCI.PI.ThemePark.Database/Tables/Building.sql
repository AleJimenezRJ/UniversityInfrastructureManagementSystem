CREATE TABLE [Infrastructure].[Building]
(
    [BuildingInternalId] INT IDENTITY(1,1) NOT NULL,
    [Name] VARCHAR(100) NOT NULL,
    [Color] VARCHAR(50) NOT NULL,
    [Length] DECIMAL(6, 2) NOT NULL,
    [Width] DECIMAL(6, 2) NOT NULL,
    [Height] DECIMAL(6, 2) NOT NULL,
    [X] DECIMAL(9, 6) NOT NULL,
    [Y] DECIMAL(9, 6) NOT NULL,
    [Z] DECIMAL(9, 6) NOT NULL,
    AreaName VARCHAR(100) NOT NULL,
    CONSTRAINT PK_Building PRIMARY KEY ([BuildingInternalId]),
    CONSTRAINT UQ_Building_Name UNIQUE ([Name]),
    CONSTRAINT FK_Area_Building
        FOREIGN KEY (AreaName)
        REFERENCES [Locations].[Area](Name)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);
