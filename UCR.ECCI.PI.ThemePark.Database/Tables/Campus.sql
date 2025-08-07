CREATE TABLE [Locations].[Campus]
(
    [Name] VARCHAR(100) NOT NULL,
    [Location] VARCHAR(100) NOT NULL,
    UniversityName VARCHAR(100) NOT NULL,
    CONSTRAINT PK_Name PRIMARY KEY ([Name]),
    CONSTRAINT FK_University_Name
        FOREIGN KEY (UniversityName)
        REFERENCES [Locations].[University](Name)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

