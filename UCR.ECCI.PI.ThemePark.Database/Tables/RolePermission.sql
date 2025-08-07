CREATE TABLE RolePermission (
  RoleId INT,
  PermissionId INT,
  PRIMARY KEY (RoleId, PermissionId),
  FOREIGN KEY (RoleId) REFERENCES Role(Id) ON DELETE CASCADE,
  FOREIGN KEY (PermissionId) REFERENCES Permission(Id) ON DELETE CASCADE
);