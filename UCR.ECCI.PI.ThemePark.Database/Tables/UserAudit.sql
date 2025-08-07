CREATE TABLE UserAudit (
    AuditId INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(50),
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Email NVARCHAR(100),
    Phone VARCHAR(20),
    IdentityNumber VARCHAR(20),
    BirthDate DATE,
    ModifiedAt DATETIME NOT NULL,
    Action NVARCHAR(20) NOT NULL
);
