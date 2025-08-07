CREATE TABLE [Locations].[Area]
(
    [Name] VARCHAR(100) NOT NULL PRIMARY KEY,
    CampusName VARCHAR(100) NOT NULL,
    CONSTRAINT FK_Campus_Area
        FOREIGN KEY (CampusName)
        REFERENCES [Locations].[Campus](Name)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);