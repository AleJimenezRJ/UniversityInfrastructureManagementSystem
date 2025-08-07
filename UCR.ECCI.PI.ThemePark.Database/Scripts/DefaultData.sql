-- Insert basic person records into Person table
INSERT INTO Person (Email, IdentityNumber, FirstName, LastName, BirthDate, Phone)
VALUES
('ana.morales@ucr.ac.cr',      '2-0001-1001', 'Ana',       'Morales',   '1993-04-01', '8888-0201'),
('bryan.castro@ucr.ac.cr',     '2-0002-1002', 'Bryan',     'Castro',    '1990-02-02', '8888-0202'),
('carla.mendez@ucr.ac.cr',     '2-0003-1003', 'Carla',     'Méndez',    '1995-03-03', '8888-0203'),
('david.rodriguez@ucr.ac.cr',  '2-0004-1004', 'David',     'Rodríguez', '1989-04-04', '8888-0204'),
('elena.solis@ucr.ac.cr',      '2-0005-1005', 'Elena',     'Solís',     '1994-05-05', '8888-0205'),
('fabian.quesada@ucr.ac.cr',   '2-0006-1006', 'Fabián',    'Quesada',   '1992-06-06', '8888-0206'),
('gabriela.brenes@ucr.ac.cr',  '2-0007-1007', 'Gabriela',  'Brenes',    '1997-07-07', '8888-0207'),
('hector.martinez@ucr.ac.cr',  '2-0008-1008', 'Héctor',    'Martínez',  '1991-08-08', '8888-0208'),
('ivanna.murillo@ucr.ac.cr',   '2-0009-1009', 'Ivanna',    'Murillo',   '1990-09-09', '8888-0209'),
('jose.sanchez@ucr.ac.cr',     '2-0010-1010', 'José',      'Sánchez',   '1996-10-10', '8888-0210'),
('karla.chacon@ucr.ac.cr',     '2-0011-1011', 'Karla',     'Chacón',    '1998-11-11', '8888-0211'),
('luis.ramirez@ucr.ac.cr',     '2-0012-1012', 'Luis',      'Ramírez',   '1993-12-12', '8888-0212'),
('mariela.acuna@ucr.ac.cr',    '2-0013-1013', 'Mariela',   'Acuña',     '1995-01-13', '8888-0213'),
('nicolas.robles@ucr.ac.cr',   '2-0014-1014', 'Nicolás',   'Robles',    '1988-02-14', '8888-0214'),
('olga.valverde@ucr.ac.cr',    '2-0015-1015', 'Olga',      'Valverde',  '1991-03-15', '8888-0215'),
('pablo.cespedes@ucr.ac.cr',   '2-0016-1016', 'Pablo',     'Céspedes',  '1990-04-16', '8888-0216'),
('quiroz.maria@ucr.ac.cr',     '2-0017-1017', 'María',     'Quiróz',    '1989-05-17', '8888-0217'),
('ronald.venegas@ucr.ac.cr',   '2-0018-1018', 'Ronald',    'Venegas',   '1994-06-18', '8888-0218'),
('silvia.murillo@ucr.ac.cr',   '2-0019-1019', 'Silvia',    'Murillo',   '1992-07-19', '8888-0219'),
('tomas.reyes@ucr.ac.cr',      '2-0020-1020', 'Tomás',     'Reyes',     '1993-08-20', '8888-0220'),
('ursula.monge@ucr.ac.cr',     '2-0021-1021', 'Úrsula',    'Monge',     '1995-09-21', '8888-0221'),
('victor.castro@ucr.ac.cr',    '2-0022-1022', 'Víctor',    'Castro',    '1987-10-22', '8888-0222'),
('wendy.ramirez@ucr.ac.cr',    '2-0023-1023', 'Wendy',     'Ramírez',   '1990-11-23', '8888-0223'),
('ximena.quesada@ucr.ac.cr',   '2-0024-1024', 'Ximena',    'Quesada',   '1996-12-24', '8888-0224'),
('yolanda.corrales@ucr.ac.cr', '2-0025-1025', 'Yolanda',   'Corrales',  '1991-01-25', '8888-0225'),
('zandro.solis@ucr.ac.cr',     '2-0026-1026', 'Zandro',    'Solís',     '1988-02-26', '8888-0226'),
('adriana.lopez@ucr.ac.cr',    '2-0027-1027', 'Adriana',   'López',     '1992-03-27', '8888-0227'),
('beto.gomez@ucr.ac.cr',       '2-0028-1028', 'Beto',      'Gómez',     '1990-04-28', '8888-0228'),
('cynthia.arias@ucr.ac.cr',    '2-0029-1029', 'Cynthia',   'Arias',     '1989-05-29', '8888-0229'),
('diego.venegas@ucr.ac.cr',    '2-0030-1030', 'Diego',     'Venegas',   '1993-06-30', '8888-0230'),
('userthemepark+admin@gmail.com',  '2-0031-1031', 'Andrea', 'Villalobos', '1990-01-31', '8888-0231'),
('userthemepark+editor@gmail.com', '2-0032-1032', 'Esteban', 'Jiménez',   '1991-02-28', '8888-0232'),
('userthemepark+viewer@gmail.com', '2-0033-1033', 'Valeria', 'Soto',      '1982-03-27', '8888-0233');

-- Link some persons to UserAccounts
INSERT INTO UserAccount (UserName, PersonId)
VALUES
('ana.morales',     (SELECT Id FROM Person WHERE Email = 'ana.morales@ucr.ac.cr')),
('bryan.castro',    (SELECT Id FROM Person WHERE Email = 'bryan.castro@ucr.ac.cr')),
('carla.mendez',    (SELECT Id FROM Person WHERE Email = 'carla.mendez@ucr.ac.cr')),
('david.rodriguez', (SELECT Id FROM Person WHERE Email = 'david.rodriguez@ucr.ac.cr')),
('elena.solis',     (SELECT Id FROM Person WHERE Email = 'elena.solis@ucr.ac.cr')),
('fabian.quesada',  (SELECT Id FROM Person WHERE Email = 'fabian.quesada@ucr.ac.cr')),
('gabriela.brenes', (SELECT Id FROM Person WHERE Email = 'gabriela.brenes@ucr.ac.cr')),
('hector.martinez', (SELECT Id FROM Person WHERE Email = 'hector.martinez@ucr.ac.cr')),
('ivanna.murillo',  (SELECT Id FROM Person WHERE Email = 'ivanna.murillo@ucr.ac.cr')),
('jose.sanchez',    (SELECT Id FROM Person WHERE Email = 'jose.sanchez@ucr.ac.cr')),
('karla.chacon',    (SELECT Id FROM Person WHERE Email = 'karla.chacon@ucr.ac.cr')),
('luis.ramirez',    (SELECT Id FROM Person WHERE Email = 'luis.ramirez@ucr.ac.cr')),
('mariela.acuna',   (SELECT Id FROM Person WHERE Email = 'mariela.acuna@ucr.ac.cr')),
('nicolas.robles',  (SELECT Id FROM Person WHERE Email = 'nicolas.robles@ucr.ac.cr')),
('olga.valverde',   (SELECT Id FROM Person WHERE Email = 'olga.valverde@ucr.ac.cr')),
('pablo.cespedes',  (SELECT Id FROM Person WHERE Email = 'pablo.cespedes@ucr.ac.cr')),
('quiroz.maria',    (SELECT Id FROM Person WHERE Email = 'quiroz.maria@ucr.ac.cr')),
('ronald.venegas',  (SELECT Id FROM Person WHERE Email = 'ronald.venegas@ucr.ac.cr')),
('silvia.murillo',  (SELECT Id FROM Person WHERE Email = 'silvia.murillo@ucr.ac.cr')),
('tomas.reyes',     (SELECT Id FROM Person WHERE Email = 'tomas.reyes@ucr.ac.cr')),
('ursula.monge',    (SELECT Id FROM Person WHERE Email = 'ursula.monge@ucr.ac.cr')),
('victor.castro',   (SELECT Id FROM Person WHERE Email = 'victor.castro@ucr.ac.cr')),
('wendy.ramirez',   (SELECT Id FROM Person WHERE Email = 'wendy.ramirez@ucr.ac.cr')),
('ximena.quesada',  (SELECT Id FROM Person WHERE Email = 'ximena.quesada@ucr.ac.cr')),
('yolanda.corrales',(SELECT Id FROM Person WHERE Email = 'yolanda.corrales@ucr.ac.cr')),
('zandro.solis',    (SELECT Id FROM Person WHERE Email = 'zandro.solis@ucr.ac.cr')),
('adriana.lopez',   (SELECT Id FROM Person WHERE Email = 'adriana.lopez@ucr.ac.cr')),
('beto.gomez',      (SELECT Id FROM Person WHERE Email = 'beto.gomez@ucr.ac.cr')),
('cynthia.arias',   (SELECT Id FROM Person WHERE Email = 'cynthia.arias@ucr.ac.cr')),
('diego.venegas',   (SELECT Id FROM Person WHERE Email = 'diego.venegas@ucr.ac.cr')),
('andrea.villalobos', (SELECT Id FROM Person WHERE Email = 'userthemepark+admin@gmail.com')),
('esteban.jimenez',   (SELECT Id FROM Person WHERE Email = 'userthemepark+editor@gmail.com')),
('valeria.soto',      (SELECT Id FROM Person WHERE Email = 'userthemepark+viewer@gmail.com'));

-- Create roles in the system
INSERT INTO Role (Name) VALUES 
('Admin'),
('Editor'),
('Viewer');

-- Define system permissions
INSERT INTO Permission (Description)
VALUES
  ('View Users'),                    -- Can list and view user details 1
  ('Create Users'),                  -- Can create new users 2
  ('Delete Users'),                  -- Can delete users 3
  ('List Buildings'),                -- Can list all buildings 4
  ('View Specific Building'),        -- Can view building details 5 
  ('Edit Buildings'),                -- Can edit existing buildings 6
  ('List Universities'),             -- Can list all universities 7
  ('View Specific University'),      -- Can view university details 8
  ('Edit Users'),                    -- Can edit users 9
  ('Create Buildings'),              -- Can create a building 10
  ('View Specific Component'),       -- Can view component details 11
  ('List Components'),               -- Can list all components 12
  ('Edit Components'),               -- Can edit existing components 13
  ('Create Components'),             -- Can create a component 14
  ('Delete Components'),             -- Can delete a component 15
  ('Delete Buildings'),              -- Can delete a building 16
  ('Delete Areas'),                  -- Can delete an area 17
  ('Delete Campus'),                 -- Can delete a campus 18
  ('Delete Universities'),           -- Can delete a university 19
  ('Create Universities'),           -- Can create a university 20
  ('Create Campus'),                 -- Can create a campus 21
  ('Create Area'),                   -- Can create an area 22
  ('List Areas'),                    -- Can list all areas 23
  ('List Campus'),                   -- Can list all campus 24
  ('View Specific Area'),            -- Can view area details 25
  ('View Specific Campus'),          -- Can view campus details 26                  -- Can view audit 27

  ('List Floors'),                  -- Can list Floors 27                    
  ('Delete Floors'),                 -- Can delete Floors 28              
  ('List Learning Space'),           -- Can list Learning Spaces 29      
  ('View Learning Space'),           -- Can view  Learning Spaces details 30
  ('Create Learning Space'),         -- Can create new Learning Spaces 31  
  ('Edit Learning Space'),           -- Can edit existing  Learning Spaces 32
  ('Delete Learning Space'),         -- Can delete Learning Spaces 33         
  ('Create Floors'),                 -- Can create Learning Spaces 34         
  ('View Audit');                    -- Can Audit 35

-- Assign permissions to Admin (full access)
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 1);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 2);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 3);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 4);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 5);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 6);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 7);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 8);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 9);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 10);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 11);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 12);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 13);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 14);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 15);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 16);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 17);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 18);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 19);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 20);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 21);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 22);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 23);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 24);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 25);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 26);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 27);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 28);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 29);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 30);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 31);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 32);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 33);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 34);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (1, 35);

-- Assign permissions to Editor (not including create or delete access)
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 1);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 4);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 5);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 6); -- Edit buildings
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 7);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 8);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 9);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 11);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 12);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 13);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 23); -- List Areas
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 24); -- List Campus
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 25); -- View Specific Area
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 26); -- View Specific Campus
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 27);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 29);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 30);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (2, 32);


-- Assign permissions to Viewer (read access only)
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (3, 1);  -- View Users
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (3, 4);  -- List Buildings
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (3, 5);  -- View Specific Building
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (3, 7);  -- List Universities
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (3, 8);  -- View Specific University
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (3, 11); -- View Specific Components
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (3, 12); -- List Components
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (3, 23); -- List Areas
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (3, 24); -- List Campus
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (3, 25); -- View Specific Area
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (3, 26); -- View Specific Campus
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (3, 27);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (3, 29);
INSERT INTO RolePermission (RoleId, PermissionId) VALUES (3, 30);

-- Admins
INSERT INTO UserRole (RoleId, UserId)
VALUES
((SELECT Id FROM Role WHERE Name = 'Admin'), (SELECT Id FROM UserAccount WHERE UserName = 'ana.morales')),
((SELECT Id FROM Role WHERE Name = 'Admin'), (SELECT Id FROM UserAccount WHERE UserName = 'hector.martinez')),
((SELECT Id FROM Role WHERE Name = 'Admin'), (SELECT Id FROM UserAccount WHERE UserName = 'karla.chacon')),
((SELECT Id FROM Role WHERE Name = 'Admin'), (SELECT Id FROM UserAccount WHERE UserName = 'ronald.venegas')),
((SELECT Id FROM Role WHERE Name = 'Admin'), (SELECT Id FROM UserAccount WHERE UserName = 'cynthia.arias')),
((SELECT Id FROM Role WHERE Name = 'Admin'), (SELECT Id FROM UserAccount WHERE UserName = 'andrea.villalobos'));

-- Editors
INSERT INTO UserRole (RoleId, UserId)
VALUES
((SELECT Id FROM Role WHERE Name = 'Editor'), (SELECT Id FROM UserAccount WHERE UserName = 'bryan.castro')),
((SELECT Id FROM Role WHERE Name = 'Editor'), (SELECT Id FROM UserAccount WHERE UserName = 'david.rodriguez')),
((SELECT Id FROM Role WHERE Name = 'Editor'), (SELECT Id FROM UserAccount WHERE UserName = 'fabian.quesada')),
((SELECT Id FROM Role WHERE Name = 'Editor'), (SELECT Id FROM UserAccount WHERE UserName = 'luis.ramirez')),
((SELECT Id FROM Role WHERE Name = 'Editor'), (SELECT Id FROM UserAccount WHERE UserName = 'adriana.lopez')),
((SELECT Id FROM Role WHERE Name = 'Editor'), (SELECT Id FROM UserAccount WHERE UserName = 'esteban.jimenez'));

-- Viewers
INSERT INTO UserRole (RoleId, UserId)
VALUES
((SELECT Id FROM Role WHERE Name = 'Viewer'), (SELECT Id FROM UserAccount WHERE UserName = 'carla.mendez')),
((SELECT Id FROM Role WHERE Name = 'Viewer'), (SELECT Id FROM UserAccount WHERE UserName = 'elena.solis')),
((SELECT Id FROM Role WHERE Name = 'Viewer'), (SELECT Id FROM UserAccount WHERE UserName = 'gabriela.brenes')),
((SELECT Id FROM Role WHERE Name = 'Viewer'), (SELECT Id FROM UserAccount WHERE UserName = 'ivanna.murillo')),
((SELECT Id FROM Role WHERE Name = 'Viewer'), (SELECT Id FROM UserAccount WHERE UserName = 'jose.sanchez')),
((SELECT Id FROM Role WHERE Name = 'Viewer'), (SELECT Id FROM UserAccount WHERE UserName = 'mariela.acuna')),
((SELECT Id FROM Role WHERE Name = 'Viewer'), (SELECT Id FROM UserAccount WHERE UserName = 'nicolas.robles')),
((SELECT Id FROM Role WHERE Name = 'Viewer'), (SELECT Id FROM UserAccount WHERE UserName = 'olga.valverde')),
((SELECT Id FROM Role WHERE Name = 'Viewer'), (SELECT Id FROM UserAccount WHERE UserName = 'pablo.cespedes')),
((SELECT Id FROM Role WHERE Name = 'Viewer'), (SELECT Id FROM UserAccount WHERE UserName = 'quiroz.maria')),
((SELECT Id FROM Role WHERE Name = 'Viewer'), (SELECT Id FROM UserAccount WHERE UserName = 'valeria.soto'));


-- Asuming schemas already exist

-- Insert new universities
INSERT INTO [Locations].[University] ([Name], [Country])
VALUES 
('Technological Institute of Costa Rica', 'Costa Rica'),
('University of Costa Rica', 'Costa Rica'),
('National University of Costa Rica', 'Costa Rica');

-- Insert new campuses
INSERT INTO [Locations].[Campus] ([Name], [Location], [UniversityName])
VALUES 
('Central Cartago', 'Cartago', 'Technological Institute of Costa Rica'),
('San Carlos', 'Alajuela', 'Technological Institute of Costa Rica'),
('Rodrigo Facio', 'San Jose', 'University of Costa Rica'),
('Heredia Main', 'Heredia', 'National University of Costa Rica');

-- Insert areas
INSERT INTO [Locations].[Area] ([Name], [CampusName])
VALUES 
('Computer Science', 'Central Cartago'),
('Industrial Design', 'Central Cartago'),
('Agronomy', 'San Carlos'),
('Veterinary', 'San Carlos'),
('Social Sciences', 'Heredia Main'),
('Environmental Studies', 'Heredia Main'),
('Maths', 'Heredia Main'),
('Engineering', 'Rodrigo Facio');

-- Insert 22 new buildings (adding to existing 3)
INSERT INTO [Infrastructure].[Building] (
    [Name], [Color], [Length], [Width], [Height], [X], [Y], [Z], [AreaName]
)
VALUES 
('AI Research Center', 'White', 45.0, 22.0, 11.0, 11.1, 20.2, 0.0, 'Computer Science'),
('Electrical Engineering', 'Silver', 35.0, 18.0, 9.0, 12.2, 21.3, 0.0, 'Engineering'),
('Design Studio A', 'Orange', 25.0, 20.0, 10.0, 13.3, 22.4, 0.0, 'Industrial Design'),
('Marine Life Exhibit', 'Yellow', 28.0, 22.0, 10.0, 14.4, 23.5, 0.0, 'Veterinary'),
('Agronomy Lab 1', 'Green', 30.0, 19.0, 8.0, 15.5, 24.6, 0.0, 'Agronomy'),
('Agronomy Lab 2', 'Green', 32.0, 18.0, 8.5, 16.6, 25.7, 0.0, 'Agronomy'),
('Vet Hospital', 'Red', 40.0, 25.0, 12.0, 17.7, 26.8, 0.0, 'Veterinary'),
('Vet Classrooms', 'Red', 35.0, 21.0, 10.0, 18.8, 27.9, 0.0, 'Veterinary'),
('Psychology Building', 'Purple', 38.0, 20.0, 10.5, 19.9, 28.0, 0.0, 'Social Sciences'),
('Sociology Building', 'Pink', 36.0, 19.5, 9.8, 20.0, 28.1, 0.0, 'Social Sciences'),
('Eco Studies Lab', 'Cyan', 29.0, 18.0, 9.0, 21.1, 29.2, 0.0, 'Environmental Studies'),
('Climate Research Center', 'Navy', 33.0, 20.0, 9.2, 22.2, 30.3, 0.0, 'Environmental Studies'),
('Tech Workshop', 'Gray', 42.0, 23.0, 11.0, 23.3, 31.4, 0.0, 'Computer Science'),
('Industrial Prototyping', 'Black', 40.0, 25.0, 11.5, 24.4, 32.5, 0.0, 'Industrial Design'),
('Physics Lab', 'Lime', 31.0, 20.0, 8.7, 25.5, 33.6, 0.0, 'Maths'),
('Animal Research Unit', 'Brown', 39.0, 24.0, 10.8, 26.6, 34.7, 0.0, 'Veterinary'),
('Behavioral Studies', 'Purple', 30.0, 18.0, 9.4, 27.7, 35.8, 0.0, 'Social Sciences'),
('Environmental Tech', 'Green', 34.0, 21.0, 10.0, 28.8, 36.9, 0.0, 'Environmental Studies'),
('Robotics Lab', 'Blue', 33.5, 20.0, 10.0, 29.9, 38.0, 0.0, 'Computer Science'),
('Machine Shop', 'Gray', 38.5, 22.0, 10.5, 30.0, 39.1, 0.0, 'Industrial Design'),
('Greenhouse 1', 'Green', 28.0, 17.0, 6.5, 31.1, 40.2, 0.0, 'Agronomy'),
('Clinical Training', 'Blue', 36.0, 20.0, 9.0, 32.2, 41.3, 0.0, 'Veterinary');


-- 1. Get IDs of key buildings for expansion
DECLARE @BuildingAI       INT = (SELECT BuildingInternalId FROM [Infrastructure].[Building] WHERE Name = 'AI Research Center');
DECLARE @BuildingEco      INT = (SELECT BuildingInternalId FROM [Infrastructure].[Building] WHERE Name = 'Eco Studies Lab');
DECLARE @BuildingVet      INT = (SELECT BuildingInternalId FROM [Infrastructure].[Building] WHERE Name = 'Vet Hospital');
DECLARE @BuildingEng      INT = (SELECT BuildingInternalId FROM [Infrastructure].[Building] WHERE Name = 'Electrical Engineering');
DECLARE @BuildingProto    INT = (SELECT BuildingInternalId FROM [Infrastructure].[Building] WHERE Name = 'Industrial Prototyping');
DECLARE @BuildingRob      INT = (SELECT BuildingInternalId FROM [Infrastructure].[Building] WHERE Name = 'Robotics Lab');

-- 2. Insert 15 new floors distributed among buildings
INSERT INTO [Infrastructure].[Floor] (BuildingId, Number)
VALUES
  (@BuildingAI,    1), (@BuildingAI,    2), (@BuildingAI,    3),
  (@BuildingEco,   1), (@BuildingEco,   2),
  (@BuildingVet,   1), (@BuildingVet,   2),
  (@BuildingEng,   1), (@BuildingEng,   2),
  (@BuildingProto, 1), (@BuildingProto, 2), (@BuildingProto, 3),
  (@BuildingRob,   1), (@BuildingRob,   2), (@BuildingRob,   3);

-- 3. Retrieve FloorIds of the newly created floors
DECLARE @FloorAI1 INT = (SELECT FloorId FROM [Infrastructure].[Floor] WHERE BuildingId = @BuildingAI AND Number = 1);
DECLARE @FloorAI2 INT = (SELECT FloorId FROM [Infrastructure].[Floor] WHERE BuildingId = @BuildingAI AND Number = 2);
DECLARE @FloorAI3 INT = (SELECT FloorId FROM [Infrastructure].[Floor] WHERE BuildingId = @BuildingAI AND Number = 3);

DECLARE @FloorEco1 INT = (SELECT FloorId FROM [Infrastructure].[Floor] WHERE BuildingId = @BuildingEco AND Number = 1);
DECLARE @FloorEco2 INT = (SELECT FloorId FROM [Infrastructure].[Floor] WHERE BuildingId = @BuildingEco AND Number = 2);

DECLARE @FloorVet1 INT = (SELECT FloorId FROM [Infrastructure].[Floor] WHERE BuildingId = @BuildingVet AND Number = 1);
DECLARE @FloorVet2 INT = (SELECT FloorId FROM [Infrastructure].[Floor] WHERE BuildingId = @BuildingVet AND Number = 2);

DECLARE @FloorEng1 INT = (SELECT FloorId FROM [Infrastructure].[Floor] WHERE BuildingId = @BuildingEng AND Number = 1);
DECLARE @FloorEng2 INT = (SELECT FloorId FROM [Infrastructure].[Floor] WHERE BuildingId = @BuildingEng AND Number = 2);

DECLARE @FloorProto1 INT = (SELECT FloorId FROM [Infrastructure].[Floor] WHERE BuildingId = @BuildingProto AND Number = 1);
DECLARE @FloorProto2 INT = (SELECT FloorId FROM [Infrastructure].[Floor] WHERE BuildingId = @BuildingProto AND Number = 2);
DECLARE @FloorProto3 INT = (SELECT FloorId FROM [Infrastructure].[Floor] WHERE BuildingId = @BuildingProto AND Number = 3);

DECLARE @FloorRob1 INT = (SELECT FloorId FROM [Infrastructure].[Floor] WHERE BuildingId = @BuildingRob AND Number = 1);
DECLARE @FloorRob2 INT = (SELECT FloorId FROM [Infrastructure].[Floor] WHERE BuildingId = @BuildingRob AND Number = 2);
DECLARE @FloorRob3 INT = (SELECT FloorId FROM [Infrastructure].[Floor] WHERE BuildingId = @BuildingRob AND Number = 3);

-- 4. Insert approximately 30 new LearningSpaces distributed in these floors
INSERT INTO [Infrastructure].[LearningSpace] (
  FloorId, Name, MaxCapacity, Type, Width, Height, Length, ColorFloor, ColorWalls, ColorCeiling
)
VALUES
  -- AI Research Center
  (@FloorAI1,  'Data Science Room',          25, 'Laboratory',  6.5, 3.0, 8.0, 'White','White','White'),
  (@FloorAI1,  'Deep Learning Classroom',    40, 'Classroom',   8.0, 3.2,10.0, 'White','White','White'),

  (@FloorAI2,  'Computer Vision Lab',        20, 'Laboratory',  6.0, 3.0, 7.5, 'Gray','White','White'),
  (@FloorAI2,  'AI Seminar Room',             50, 'Auditorium',  9.0, 3.5,10.0, 'Gray','Blue','White'),

  (@FloorAI3,  'AI Robotics Classroom',      30, 'Classroom',   7.0, 3.0, 9.0, 'White','White','White'),
  (@FloorAI3,  'Bot Lab',                    25, 'Laboratory',  6.0, 3.1, 7.5, 'White','White','White'),

  -- Eco Studies Lab
  (@FloorEco1, 'Environmental Management',  35, 'Classroom',   8.0, 3.0, 9.0, 'Green','White','White'),
  (@FloorEco1, 'Botany Lab',                20, 'Laboratory',  6.0, 3.0, 7.0, 'White','White','White'),

  (@FloorEco2, 'Eco Conference Room',       40, 'Auditorium',  9.0, 3.5,10.0, 'Gray','White','White'),
  (@FloorEco2, 'Clean Energy Classroom',    30, 'Classroom',   7.0, 3.0, 9.0, 'White','White','White'),

  -- Vet Hospital
  (@FloorVet1, 'Vet Pathology Lab',          15, 'Laboratory',  6.0, 3.5, 7.0, 'Red','White','White'),
  (@FloorVet1, 'Vet Recovery Room',          20, 'Auditorium',  8.0, 3.3, 9.0, 'Blue','White','White'),

  (@FloorVet2, 'Vet Classroom 202',          30, 'Classroom',   8.0, 3.0, 9.0, 'Blue','White','White'),
  (@FloorVet2, 'Diagnostic Lab',             20, 'Laboratory',  6.0, 3.5, 7.0, 'White','White','White'),
  (@FloorVet2, 'Vet Simulation Room',        25, 'Auditorium',  9.0, 3.5,10.0, 'White','White','White'),

  -- Electrical Engineering
  (@FloorEng1, 'Power Lab',                  20, 'Laboratory',  6.0, 3.0, 7.5, 'Gray','White','White'),
  (@FloorEng1, 'Advanced Electronics Class',40, 'Classroom',   8.0, 3.2,10.0, 'White','Blue','White'),

  (@FloorEng2, 'EE Project Room',            30, 'Auditorium',  9.0, 3.5,10.0, 'Gray','White','White'),

  -- Industrial Prototyping
  (@FloorProto1,'Basic CNC Workshop',        15, 'Laboratory',  6.0, 3.5, 7.0, 'Black','White','White'),
  (@FloorProto1,'Prototyping Classroom 101',30, 'Classroom',   8.0, 3.0, 9.0, 'White','Gray','White'),

  (@FloorProto2,'3D Face Lab',               15, 'Laboratory',  6.0, 3.0, 7.0, 'Black','White','White'),
  (@FloorProto2,'Industrial Design Room',   40, 'Auditorium',  9.0, 3.5,10.0, 'Orange','White','White'),

  (@FloorProto3,'Mechanical Classroom 201', 30, 'Classroom',   8.0, 3.0, 9.0, 'White','Gray','White'),
  (@FloorProto3,'Robotics Lab',              20, 'Laboratory',  6.0, 3.2, 7.5, 'Gray','White','White'),

  -- Robotics Lab
  (@FloorRob1,  'Robotics Simulation Room', 30, 'Auditorium',  9.0, 3.5,10.0, 'Blue','White','White'),

  (@FloorRob2,  'Drone Lab',                 20, 'Laboratory',  6.0, 3.0, 7.5, 'Gray','White','White'),

  (@FloorRob3,  'Advanced Robotics Classroom',30,'Classroom',   8.0, 3.0, 9.0, 'White','Blue','White');

-- Retrieve LearningSpaceIds of the newly created LearningSpaces
DECLARE @LS_DataScience INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Data Science Room');
DECLARE @LS_DeepLearning INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Deep Learning Classroom');
DECLARE @LS_ComputerVision INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Computer Vision Lab');
DECLARE @LS_AISeminar INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'AI Seminar Room');
DECLARE @LS_AIRobotics INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'AI Robotics Classroom');
DECLARE @LS_BotLab INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Bot Lab');
DECLARE @LS_EnvManagement INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Environmental Management');
DECLARE @LS_BotanyLab INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Botany Lab');
DECLARE @LS_EcoConference INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Eco Conference Room');
DECLARE @LS_CleanEnergy INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Clean Energy Classroom');
DECLARE @LS_VetPathology INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Vet Pathology Lab');
DECLARE @LS_VetRecovery INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Vet Recovery Room');
DECLARE @LS_VetClass202 INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Vet Classroom 202');
DECLARE @LS_DiagnosticLab INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Diagnostic Lab');
DECLARE @LS_VetSimulation INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Vet Simulation Room');
DECLARE @LS_PowerLab INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Power Lab');
DECLARE @LS_Electronics INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Advanced Electronics Class');
DECLARE @LS_EEProject INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'EE Project Room');
DECLARE @LS_CNCWorkshop INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Basic CNC Workshop');
DECLARE @LS_ProtoClass INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Prototyping Classroom 101');
DECLARE @LS_3DFaceLab INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = '3D Face Lab');
DECLARE @LS_IndustrialDesign INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Industrial Design Room');
DECLARE @LS_MechClass INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Mechanical Classroom 201');
DECLARE @LS_RoboticsLab INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Robotics Lab');
DECLARE @LS_RoboticsSimulation INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Robotics Simulation Room');
DECLARE @LS_DroneLab INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Drone Lab');
DECLARE @LS_AdvancedRobotics INT = (SELECT LearningSpaceId FROM [Infrastructure].[LearningSpace] WHERE Name = 'Advanced Robotics Classroom');

-- Data Science Room (6 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_DataScience, 0.55, 0.25, 0.45, 1.00, 2.50, 0.00, 'North',0),
(@LS_DataScience, 0.50, 0.22, 0.42, 5.50, 2.30, 0.00, 'South',0),
(@LS_DataScience, 2.00, 1.20, 0.05, 0.50, 1.00, 0.00, 'East',0),
(@LS_DataScience, 1.50, 1.00, 0.05, 6.00, 1.50, 0.00, 'West',0),
(@LS_DataScience, 1.80, 1.10, 0.05, 3.00, 0.10, 0.00, 'North',0),
(@LS_DataScience, 1.40, 0.90, 0.05, 3.50, 3.90, 0.00, 'South',0);

-- Deep Learning Classroom (4 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_DeepLearning, 0.60, 0.28, 0.48, 1.50, 3.00, 0.00, 'North',0),
(@LS_DeepLearning, 2.20, 1.30, 0.05, 1.00, 1.20, 0.00, 'East',0),
(@LS_DeepLearning, 1.60, 1.00, 0.05, 7.50, 1.80, 0.00, 'West',0),
(@LS_DeepLearning, 1.80, 1.10, 0.05, 4.00, 0.10, 0.00, 'North',0);

-- Computer Vision Lab (5 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_ComputerVision, 0.52, 0.24, 0.44, 1.20, 2.80, 0.00, 'North',0),
(@LS_ComputerVision, 0.48, 0.20, 0.40, 4.80, 2.60, 0.00, 'South',0),
(@LS_ComputerVision, 1.90, 1.15, 0.05, 0.30, 1.10, 0.00, 'East',0),
(@LS_ComputerVision, 1.30, 0.95, 0.05, 5.50, 1.40, 0.00, 'West',0),
(@LS_ComputerVision, 1.50, 1.00, 0.05, 3.00, 0.10, 0.00, 'North',0);

-- AI Seminar Room (6 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_AISeminar, 0.65, 0.30, 0.50, 1.80, 3.20, 0.00, 'North',0),
(@LS_AISeminar, 0.58, 0.26, 0.46, 7.20, 3.00, 0.00, 'North',0),
(@LS_AISeminar, 2.50, 1.40, 0.05, 1.00, 1.30, 0.00, 'East',0),
(@LS_AISeminar, 2.30, 1.35, 0.05, 8.50, 1.50, 0.00, 'West',0),
(@LS_AISeminar, 2.00, 1.20, 0.05, 4.50, 0.10, 0.00, 'North',0),
(@LS_AISeminar, 2.00, 1.20, 0.05, 6.50, 0.10, 0.00, 'North',0);

-- AI Robotics Classroom (4 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_AIRobotics, 0.58, 0.26, 0.46, 1.40, 2.70, 0.00, 'North',0),
(@LS_AIRobotics, 2.00, 1.25, 0.05, 0.80, 1.20, 0.00, 'East',0),
(@LS_AIRobotics, 1.70, 1.05, 0.05, 6.80, 1.60, 0.00, 'West',0),
(@LS_AIRobotics, 1.80, 1.10, 0.05, 3.50, 0.10, 0.00, 'North',0);

-- Bot Lab (3 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_BotLab, 0.54, 0.23, 0.43, 1.30, 2.60, 0.00, 'North',0),
(@LS_BotLab, 1.80, 1.15, 0.05, 0.60, 1.10, 0.00, 'East',0),
(@LS_BotLab, 1.60, 1.00, 0.05, 5.60, 1.50, 0.00, 'West',0);

-- Environmental Management (4 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_EnvManagement, 0.56, 0.25, 0.45, 1.60, 2.80, 0.00, 'North',0),
(@LS_EnvManagement, 2.10, 1.30, 0.05, 1.00, 1.25, 0.00, 'East',0),
(@LS_EnvManagement, 1.80, 1.10, 0.05, 7.50, 1.70, 0.00, 'West',0),
(@LS_EnvManagement, 1.90, 1.15, 0.05, 4.00, 0.10, 0.00, 'North',0);

-- Botany Lab (3 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_BotanyLab, 0.52, 0.22, 0.42, 1.20, 2.50, 0.00, 'North',0),
(@LS_BotanyLab, 1.70, 1.05, 0.05, 0.50, 1.00, 0.00, 'East',0),
(@LS_BotanyLab, 1.50, 0.95, 0.05, 5.80, 1.40, 0.00, 'West',0);

-- Eco Conference Room (5 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_EcoConference, 0.62, 0.28, 0.48, 1.80, 3.10, 0.00, 'North',0),
(@LS_EcoConference, 0.55, 0.24, 0.44, 7.20, 2.90, 0.00, 'North',0),
(@LS_EcoConference, 2.20, 1.35, 0.05, 1.20, 1.40, 0.00, 'East',0),
(@LS_EcoConference, 2.00, 1.25, 0.05, 8.50, 1.60, 0.00, 'West',0),
(@LS_EcoConference, 2.40, 1.40, 0.05, 4.50, 0.10, 0.00, 'North',0);

-- Clean Energy Classroom (4 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_CleanEnergy, 0.57, 0.25, 0.45, 1.50, 2.70, 0.00, 'North',0),
(@LS_CleanEnergy, 2.00, 1.25, 0.05, 0.90, 1.20, 0.00, 'East',0),
(@LS_CleanEnergy, 1.80, 1.10, 0.05, 6.60, 1.60, 0.00, 'West',0),
(@LS_CleanEnergy, 1.90, 1.15, 0.05, 3.50, 0.10, 0.00, 'North',0);

-- Vet Pathology Lab (3 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_VetPathology, 0.53, 0.23, 0.43, 1.30, 2.60, 0.00, 'North',0),
(@LS_VetPathology, 1.70, 1.05, 0.05, 0.60, 1.10, 0.00, 'East',0),
(@LS_VetPathology, 1.60, 1.00, 0.05, 5.70, 1.50, 0.00, 'West',0);

-- Vet Recovery Room (4 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_VetRecovery, 0.58, 0.26, 0.46, 1.60, 2.80, 0.00, 'North',0),
(@LS_VetRecovery, 1.90, 1.20, 0.05, 1.00, 1.30, 0.00, 'East',0),
(@LS_VetRecovery, 1.70, 1.05, 0.05, 7.50, 1.70, 0.00, 'West',0),
(@LS_VetRecovery, 1.80, 1.10, 0.05, 4.00, 0.10, 0.00, 'North',0);

-- Vet Classroom 202 (4 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_VetClass202, 0.59, 0.27, 0.47, 1.70, 2.90, 0.00, 'North',0),
(@LS_VetClass202, 2.10, 1.30, 0.05, 1.10, 1.35, 0.00, 'East',0),
(@LS_VetClass202, 1.80, 1.10, 0.05, 7.40, 1.75, 0.00, 'West',0),
(@LS_VetClass202, 2.00, 1.20, 0.05, 4.00, 0.10, 0.00, 'North',0);

-- Diagnostic Lab (3 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_DiagnosticLab, 0.54, 0.24, 0.44, 1.40, 2.70, 0.00, 'North',0),
(@LS_DiagnosticLab, 1.80, 1.10, 0.05, 0.70, 1.20, 0.00, 'East',0),
(@LS_DiagnosticLab, 1.60, 1.00, 0.05, 5.80, 1.60, 0.00, 'West',0);

-- Vet Simulation Room (5 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_VetSimulation, 0.61, 0.28, 0.48, 1.80, 3.00, 0.00, 'North',0),
(@LS_VetSimulation, 0.56, 0.25, 0.45, 7.20, 2.80, 0.00, 'North',0),
(@LS_VetSimulation, 2.10, 1.30, 0.05, 1.20, 1.40, 0.00, 'East',0),
(@LS_VetSimulation, 1.90, 1.20, 0.05, 8.60, 1.70, 0.00, 'West',0),
(@LS_VetSimulation, 2.20, 1.35, 0.05, 4.50, 0.10, 0.00, 'North',0);

-- Power Lab (3 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_PowerLab, 0.55, 0.24, 0.44, 1.30, 2.60, 0.00, 'North',0),
(@LS_PowerLab, 1.70, 1.05, 0.05, 0.60, 1.10, 0.00, 'East',0),
(@LS_PowerLab, 1.60, 1.00, 0.05, 5.70, 1.50, 0.00, 'West',0);

-- Advanced Electronics Class (4 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_Electronics, 0.60, 0.27, 0.47, 1.70, 2.90, 0.00, 'North',0),
(@LS_Electronics, 2.20, 1.35, 0.05, 1.20, 1.40, 0.00, 'East',0),
(@LS_Electronics, 1.90, 1.20, 0.05, 7.60, 1.80, 0.00, 'West',0),
(@LS_Electronics, 2.00, 1.25, 0.05, 4.00, 0.10, 0.00, 'North',0);

-- EE Project Room (5 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_EEProject, 0.62, 0.28, 0.48, 1.80, 3.10, 0.00, 'North',0),
(@LS_EEProject, 0.57, 0.25, 0.45, 7.20, 2.90, 0.00, 'North',0),
(@LS_EEProject, 2.30, 1.40, 0.05, 1.30, 1.50, 0.00, 'East',0),
(@LS_EEProject, 2.00, 1.25, 0.05, 8.70, 1.80, 0.00, 'West',0),
(@LS_EEProject, 2.40, 1.45, 0.05, 4.50, 0.10, 0.00, 'North',0);

-- Basic CNC Workshop (3 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_CNCWorkshop, 0.56, 0.25, 0.45, 1.40, 2.70, 0.00, 'North',0),
(@LS_CNCWorkshop, 1.80, 1.10, 0.05, 0.70, 1.20, 0.00, 'East',0),
(@LS_CNCWorkshop, 1.70, 1.05, 0.05, 5.80, 1.60, 0.00, 'West',0);

-- Prototyping Classroom 101 (4 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_ProtoClass, 0.58, 0.26, 0.46, 1.60, 2.80, 0.00, 'North',0),
(@LS_ProtoClass, 2.10, 1.30, 0.05, 1.10, 1.35, 0.00, 'East',0),
(@LS_ProtoClass, 1.80, 1.10, 0.05, 7.40, 1.75, 0.00, 'West',0),
(@LS_ProtoClass, 1.90, 1.15, 0.05, 4.00, 0.10, 0.00, 'North',0);

-- 3D Face Lab (3 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_3DFaceLab, 0.54, 0.24, 0.44, 1.30, 2.60, 0.00, 'North',0),
(@LS_3DFaceLab, 1.70, 1.05, 0.05, 0.60, 1.10, 0.00, 'East',0),
(@LS_3DFaceLab, 1.60, 1.00, 0.05, 5.70, 1.50, 0.00, 'West',0);

-- Industrial Design Room (6 componentes - sala grande)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_IndustrialDesign, 0.63, 0.29, 0.49, 2.00, 3.20, 0.00, 'North',0),
(@LS_IndustrialDesign, 0.58, 0.26, 0.46, 7.00, 3.00, 0.00, 'North',0),
(@LS_IndustrialDesign, 2.50, 1.45, 0.05, 1.50, 1.60, 0.00, 'East',0),
(@LS_IndustrialDesign, 2.30, 1.40, 0.05, 8.50, 1.80, 0.00, 'West',0),
(@LS_IndustrialDesign, 2.20, 1.35, 0.05, 4.50, 0.10, 0.00, 'North',0),
(@LS_IndustrialDesign, 2.20, 1.35, 0.05, 6.80, 0.10, 0.00, 'North',0);

-- Mechanical Classroom 201 (4 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_MechClass, 0.59, 0.27, 0.47, 1.70, 2.90, 0.00, 'North',0),
(@LS_MechClass, 2.10, 1.30, 0.05, 1.10, 1.35, 0.00, 'East',0),
(@LS_MechClass, 1.80, 1.10, 0.05, 7.40, 1.75, 0.00, 'West',0),
(@LS_MechClass, 2.00, 1.20, 0.05, 4.00, 0.10, 0.00, 'North',0);

-- Robotics Lab (Prototyping) (3 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_RoboticsLab, 0.55, 0.24, 0.44, 1.40, 2.70, 0.00, 'North',0),
(@LS_RoboticsLab, 1.80, 1.10, 0.05, 0.70, 1.20, 0.00, 'East',0),
(@LS_RoboticsLab, 1.70, 1.05, 0.05, 5.80, 1.60, 0.00, 'West',0);

-- Robotics Simulation Room (2 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_RoboticsSimulation, 0.71, 0.22, 0.78, 2.80, 3.50, 0.00, 'North',0),
(@LS_RoboticsSimulation, 0.60, 0.3, 1.46, 5.20, 2.30, 0.00, 'North',0);

-- Drone Lab (3 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_DroneLab, 0.62, 0.30, 0.68, 1.90, 3.50, 0.00, 'North',0),
(@LS_DroneLab, 0.59, 0.30, 0.60, 6.20, 2.70, 0.00, 'North',0),
(@LS_DroneLab, 2.20, 1.45, 0.05, 1.30, 1.50, 0.00, 'East',0);

-- Advanced Robotics Classroom (2 components)
INSERT INTO [Infrastructure].[LearningComponent] (LearningSpaceId, Width, Height, Depth, X, Y, Z, Orientation, IsDeleted)
VALUES 
(@LS_AdvancedRobotics, 0.61, 0.28, 0.48, 1.80, 3.00, 0.00, 'North',0),
(@LS_AdvancedRobotics, 0.56, 0.25, 0.45, 7.20, 2.80, 0.00, 'North',0);

-- Declare variables for component IDs
-- Data Science Classroom Components
DECLARE @LC_DataScience_Projector1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_DataScience AND Width = 0.55 AND Height = 0.25 AND Depth = 0.45 
          AND X = 1.00 AND Y = 2.50 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0 AND IsDeleted = 0
);
DECLARE @LC_DataScience_Projector2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_DataScience AND Width = 0.50 AND Height = 0.22 AND Depth = 0.42 
          AND X = 5.50 AND Y = 2.30 AND Z = 0.00 AND Orientation = 'South' AND IsDeleted = 0
);
DECLARE @LC_DataScience_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_DataScience AND Width = 2.00 AND Height = 1.20 AND Depth = 0.05 
          AND X = 0.50 AND Y = 1.00 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_DataScience_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_DataScience AND Width = 1.50 AND Height = 1.00 AND Depth = 0.05 
          AND X = 6.00 AND Y = 1.50 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);
DECLARE @LC_DataScience_Board3 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_DataScience AND Width = 1.80 AND Height = 1.10 AND Depth = 0.05 
          AND X = 3.00 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0 AND IsDeleted = 0
);
DECLARE @LC_DataScience_Board4 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_DataScience AND Width = 1.40 AND Height = 0.90 AND Depth = 0.05 
          AND X = 3.50 AND Y = 3.90 AND Z = 0.00 AND Orientation = 'South' AND IsDeleted = 0
);

-- Deep Learning Classroom Components
DECLARE @LC_DeepLearning_Projector INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_DeepLearning AND Width = 0.60 AND Height = 0.28 AND Depth = 0.48 
          AND X = 1.50 AND Y = 3.00 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_DeepLearning_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_DeepLearning AND Width = 2.20 AND Height = 1.30 AND Depth = 0.05 
          AND X = 1.00 AND Y = 1.20 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_DeepLearning_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_DeepLearning AND Width = 1.60 AND Height = 1.00 AND Depth = 0.05 
          AND X = 7.50 AND Y = 1.80 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);
DECLARE @LC_DeepLearning_Board3 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_DeepLearning AND Width = 1.80 AND Height = 1.10 AND Depth = 0.05 
          AND X = 4.00 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);

-- Computer Vision Lab Components
DECLARE @LC_ComputerVision_Projector1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_ComputerVision AND Width = 0.52 AND Height = 0.24 AND Depth = 0.44 
          AND X = 1.20 AND Y = 2.80 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_ComputerVision_Projector2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_ComputerVision AND Width = 0.48 AND Height = 0.20 AND Depth = 0.40 
          AND X = 4.80 AND Y = 2.60 AND Z = 0.00 AND Orientation = 'South' AND IsDeleted = 0
);
DECLARE @LC_ComputerVision_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_ComputerVision AND Width = 1.90 AND Height = 1.15 AND Depth = 0.05 
          AND X = 0.30 AND Y = 1.10 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_ComputerVision_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_ComputerVision AND Width = 1.30 AND Height = 0.95 AND Depth = 0.05 
          AND X = 5.50 AND Y = 1.40 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);
DECLARE @LC_ComputerVision_Board3 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_ComputerVision AND Width = 1.50 AND Height = 1.00 AND Depth = 0.05 
          AND X = 3.00 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);

-- AI Seminar Room Components
DECLARE @LC_AISeminar_Projector1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_AISeminar AND Width = 0.65 AND Height = 0.30 AND Depth = 0.50 
          AND X = 1.80 AND Y = 3.20 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_AISeminar_Projector2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_AISeminar AND Width = 0.58 AND Height = 0.26 AND Depth = 0.46 
          AND X = 7.20 AND Y = 3.00 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_AISeminar_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_AISeminar AND Width = 2.50 AND Height = 1.40 AND Depth = 0.05 
          AND X = 1.00 AND Y = 1.30 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_AISeminar_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_AISeminar AND Width = 2.30 AND Height = 1.35 AND Depth = 0.05 
          AND X = 8.50 AND Y = 1.50 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);
DECLARE @LC_AISeminar_Board3 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_AISeminar AND Width = 2.00 AND Height = 1.20 AND Depth = 0.05 
          AND X = 4.50 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_AISeminar_Board4 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_AISeminar AND Width = 2.00 AND Height = 1.20 AND Depth = 0.05 
          AND X = 6.50 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);

-- AI Robotics Classroom Components
DECLARE @LC_AIRobotics_Projector INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_AIRobotics AND Width = 0.58 AND Height = 0.26 AND Depth = 0.46 
          AND X = 1.40 AND Y = 2.70 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_AIRobotics_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_AIRobotics AND Width = 2.00 AND Height = 1.25 AND Depth = 0.05 
          AND X = 0.80 AND Y = 1.20 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_AIRobotics_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_AIRobotics AND Width = 1.70 AND Height = 1.05 AND Depth = 0.05 
          AND X = 6.80 AND Y = 1.60 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);
DECLARE @LC_AIRobotics_Board3 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_AIRobotics AND Width = 1.80 AND Height = 1.10 AND Depth = 0.05 
          AND X = 3.50 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);

-- Bot Lab Components
DECLARE @LC_BotLab_Projector INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_BotLab AND Width = 0.54 AND Height = 0.23 AND Depth = 0.43 
          AND X = 1.30 AND Y = 2.60 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_BotLab_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_BotLab AND Width = 1.80 AND Height = 1.15 AND Depth = 0.05 
          AND X = 0.60 AND Y = 1.10 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_BotLab_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_BotLab AND Width = 1.60 AND Height = 1.00 AND Depth = 0.05 
          AND X = 5.60 AND Y = 1.50 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);

-- Environmental Management Components
DECLARE @LC_EnvManagement_Projector INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_EnvManagement AND Width = 0.56 AND Height = 0.25 AND Depth = 0.45 
          AND X = 1.60 AND Y = 2.80 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_EnvManagement_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_EnvManagement AND Width = 2.10 AND Height = 1.30 AND Depth = 0.05 
          AND X = 1.00 AND Y = 1.25 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_EnvManagement_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_EnvManagement AND Width = 1.80 AND Height = 1.10 AND Depth = 0.05 
          AND X = 7.50 AND Y = 1.70 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);
DECLARE @LC_EnvManagement_Board3 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_EnvManagement AND Width = 1.90 AND Height = 1.15 AND Depth = 0.05 
          AND X = 4.00 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);

-- Botany Lab Components
DECLARE @LC_BotanyLab_Projector INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_BotanyLab AND Width = 0.52 AND Height = 0.22 AND Depth = 0.42 
          AND X = 1.20 AND Y = 2.50 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_BotanyLab_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_BotanyLab AND Width = 1.70 AND Height = 1.05 AND Depth = 0.05 
          AND X = 0.50 AND Y = 1.00 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_BotanyLab_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_BotanyLab AND Width = 1.50 AND Height = 0.95 AND Depth = 0.05 
          AND X = 5.80 AND Y = 1.40 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);

-- Eco Conference Room Components
DECLARE @LC_EcoConference_Projector1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_EcoConference AND Width = 0.62 AND Height = 0.28 AND Depth = 0.48 
          AND X = 1.80 AND Y = 3.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_EcoConference_Projector2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_EcoConference AND Width = 0.55 AND Height = 0.24 AND Depth = 0.44 
          AND X = 7.20 AND Y = 2.90 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_EcoConference_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_EcoConference AND Width = 2.20 AND Height = 1.35 AND Depth = 0.05 
          AND X = 1.20 AND Y = 1.40 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_EcoConference_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_EcoConference AND Width = 2.00 AND Height = 1.25 AND Depth = 0.05 
          AND X = 8.50 AND Y = 1.60 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);
DECLARE @LC_EcoConference_Board3 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_EcoConference AND Width = 2.40 AND Height = 1.40 AND Depth = 0.05 
          AND X = 4.50 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);

-- Clean Energy Classroom Components
DECLARE @LC_CleanEnergy_Projector INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_CleanEnergy AND Width = 0.57 AND Height = 0.25 AND Depth = 0.45 
          AND X = 1.50 AND Y = 2.70 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_CleanEnergy_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_CleanEnergy AND Width = 2.00 AND Height = 1.25 AND Depth = 0.05 
          AND X = 0.90 AND Y = 1.20 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_CleanEnergy_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_CleanEnergy AND Width = 1.80 AND Height = 1.10 AND Depth = 0.05 
          AND X = 6.60 AND Y = 1.60 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);
DECLARE @LC_CleanEnergy_Board3 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_CleanEnergy AND Width = 1.90 AND Height = 1.15 AND Depth = 0.05 
          AND X = 3.50 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);

-- Vet Pathology Lab Components
DECLARE @LC_VetPathology_Projector INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_VetPathology AND Width = 0.53 AND Height = 0.23 AND Depth = 0.43 
          AND X = 1.30 AND Y = 2.60 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_VetPathology_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_VetPathology AND Width = 1.70 AND Height = 1.05 AND Depth = 0.05 
          AND X = 0.60 AND Y = 1.10 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_VetPathology_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_VetPathology AND Width = 1.60 AND Height = 1.00 AND Depth = 0.05 
          AND X = 5.70 AND Y = 1.50 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);

-- Vet Recovery Room Components
DECLARE @LC_VetRecovery_Projector INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_VetRecovery AND Width = 0.58 AND Height = 0.26 AND Depth = 0.46 
          AND X = 1.60 AND Y = 2.80 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_VetRecovery_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_VetRecovery AND Width = 1.90 AND Height = 1.20 AND Depth = 0.05 
          AND X = 1.00 AND Y = 1.30 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_VetRecovery_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_VetRecovery AND Width = 1.70 AND Height = 1.05 AND Depth = 0.05 
          AND X = 7.50 AND Y = 1.70 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);
DECLARE @LC_VetRecovery_Board3 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_VetRecovery AND Width = 1.80 AND Height = 1.10 AND Depth = 0.05 
          AND X = 4.00 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);

-- Vet Classroom 202 Components
DECLARE @LC_VetClass202_Projector INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_VetClass202 AND Width = 0.59 AND Height = 0.27 AND Depth = 0.47 
          AND X = 1.70 AND Y = 2.90 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_VetClass202_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_VetClass202 AND Width = 2.10 AND Height = 1.30 AND Depth = 0.05 
          AND X = 1.10 AND Y = 1.35 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_VetClass202_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_VetClass202 AND Width = 1.80 AND Height = 1.10 AND Depth = 0.05 
          AND X = 7.40 AND Y = 1.75 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);
DECLARE @LC_VetClass202_Board3 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_VetClass202 AND Width = 2.00 AND Height = 1.20 AND Depth = 0.05 
          AND X = 4.00 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);

-- Diagnostic Lab Components
DECLARE @LC_DiagnosticLab_Projector INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_DiagnosticLab AND Width = 0.54 AND Height = 0.24 AND Depth = 0.44 
          AND X = 1.40 AND Y = 2.70 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_DiagnosticLab_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_DiagnosticLab AND Width = 1.80 AND Height = 1.10 AND Depth = 0.05 
          AND X = 0.70 AND Y = 1.20 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_DiagnosticLab_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_DiagnosticLab AND Width = 1.60 AND Height = 1.00 AND Depth = 0.05 
          AND X = 5.80 AND Y = 1.60 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);

-- Vet Simulation Room Components
DECLARE @LC_VetSimulation_Projector1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_VetSimulation AND Width = 0.61 AND Height = 0.28 AND Depth = 0.48 
          AND X = 1.80 AND Y = 3.00 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_VetSimulation_Projector2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_VetSimulation AND Width = 0.56 AND Height = 0.25 AND Depth = 0.45 
          AND X = 7.20 AND Y = 2.80 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_VetSimulation_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_VetSimulation AND Width = 2.10 AND Height = 1.30 AND Depth = 0.05 
          AND X = 1.20 AND Y = 1.40 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_VetSimulation_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_VetSimulation AND Width = 1.90 AND Height = 1.20 AND Depth = 0.05 
          AND X = 8.60 AND Y = 1.70 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);
DECLARE @LC_VetSimulation_Board3 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_VetSimulation AND Width = 2.20 AND Height = 1.35 AND Depth = 0.05 
          AND X = 4.50 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);

-- Power Lab Components
DECLARE @LC_PowerLab_Projector INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_PowerLab AND Width = 0.55 AND Height = 0.24 AND Depth = 0.44 
          AND X = 1.30 AND Y = 2.60 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_PowerLab_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_PowerLab AND Width = 1.70 AND Height = 1.05 AND Depth = 0.05 
          AND X = 0.60 AND Y = 1.10 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_PowerLab_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_PowerLab AND Width = 1.60 AND Height = 1.00 AND Depth = 0.05 
          AND X = 5.70 AND Y = 1.50 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);

-- Advanced Electronics Class Components
DECLARE @LC_Electronics_Projector INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_Electronics AND Width = 0.60 AND Height = 0.27 AND Depth = 0.47 
          AND X = 1.70 AND Y = 2.90 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_Electronics_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_Electronics AND Width = 2.20 AND Height = 1.35 AND Depth = 0.05 
          AND X = 1.20 AND Y = 1.40 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_Electronics_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_Electronics AND Width = 1.90 AND Height = 1.20 AND Depth = 0.05 
          AND X = 7.60 AND Y = 1.80 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);
DECLARE @LC_Electronics_Board3 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_Electronics AND Width = 2.00 AND Height = 1.25 AND Depth = 0.05 
          AND X = 4.00 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
-- EE Project Room Components
DECLARE @LC_EEProject_Projector1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_EEProject AND Width = 0.62 AND Height = 0.28 AND Depth = 0.48 
          AND X = 1.80 AND Y = 3.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_EEProject_Projector2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_EEProject AND Width = 0.57 AND Height = 0.25 AND Depth = 0.45 
          AND X = 7.20 AND Y = 2.90 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_EEProject_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_EEProject AND Width = 2.30 AND Height = 1.40 AND Depth = 0.05 
          AND X = 1.30 AND Y = 1.50 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_EEProject_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_EEProject AND Width = 2.00 AND Height = 1.25 AND Depth = 0.05 
          AND X = 8.70 AND Y = 1.80 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);
DECLARE @LC_EEProject_Board3 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_EEProject AND Width = 2.40 AND Height = 1.45 AND Depth = 0.05 
          AND X = 4.50 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);

-- Basic CNC Workshop Components
DECLARE @LC_CNCWorkshop_Projector INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_CNCWorkshop AND Width = 0.56 AND Height = 0.25 AND Depth = 0.45 
          AND X = 1.40 AND Y = 2.70 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_CNCWorkshop_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_CNCWorkshop AND Width = 1.80 AND Height = 1.10 AND Depth = 0.05 
          AND X = 0.70 AND Y = 1.20 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_CNCWorkshop_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_CNCWorkshop AND Width = 1.70 AND Height = 1.05 AND Depth = 0.05 
          AND X = 5.80 AND Y = 1.60 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);

-- Prototyping Classroom 101 Components
DECLARE @LC_ProtoClass_Projector INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_ProtoClass AND Width = 0.58 AND Height = 0.26 AND Depth = 0.46 
          AND X = 1.60 AND Y = 2.80 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_ProtoClass_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_ProtoClass AND Width = 2.10 AND Height = 1.30 AND Depth = 0.05 
          AND X = 1.10 AND Y = 1.35 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_ProtoClass_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_ProtoClass AND Width = 1.80 AND Height = 1.10 AND Depth = 0.05 
          AND X = 7.40 AND Y = 1.75 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);
DECLARE @LC_ProtoClass_Board3 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_ProtoClass AND Width = 1.90 AND Height = 1.15 AND Depth = 0.05 
          AND X = 4.00 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);

-- 3D Face Lab Components
DECLARE @LC_3DFaceLab_Projector INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_3DFaceLab AND Width = 0.54 AND Height = 0.24 AND Depth = 0.44 
          AND X = 1.30 AND Y = 2.60 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_3DFaceLab_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_3DFaceLab AND Width = 1.70 AND Height = 1.05 AND Depth = 0.05 
          AND X = 0.60 AND Y = 1.10 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_3DFaceLab_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_3DFaceLab AND Width = 1.60 AND Height = 1.00 AND Depth = 0.05 
          AND X = 5.70 AND Y = 1.50 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);

-- Industrial Design Room Components
DECLARE @LC_IndustrialDesign_Projector1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_IndustrialDesign AND Width = 0.63 AND Height = 0.29 AND Depth = 0.49 
          AND X = 2.00 AND Y = 3.20 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_IndustrialDesign_Projector2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_IndustrialDesign AND Width = 0.58 AND Height = 0.26 AND Depth = 0.46 
          AND X = 7.00 AND Y = 3.00 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_IndustrialDesign_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_IndustrialDesign AND Width = 2.50 AND Height = 1.45 AND Depth = 0.05 
          AND X = 1.50 AND Y = 1.60 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_IndustrialDesign_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_IndustrialDesign AND Width = 2.30 AND Height = 1.40 AND Depth = 0.05 
          AND X = 8.50 AND Y = 1.80 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);
DECLARE @LC_IndustrialDesign_Board3 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_IndustrialDesign AND Width = 2.20 AND Height = 1.35 AND Depth = 0.05 
          AND X = 4.50 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_IndustrialDesign_Board4 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_IndustrialDesign AND Width = 2.20 AND Height = 1.35 AND Depth = 0.05 
          AND X = 6.80 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);

-- Mechanical Classroom 201 Components
DECLARE @LC_MechClass_Projector INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_MechClass AND Width = 0.59 AND Height = 0.27 AND Depth = 0.47 
          AND X = 1.70 AND Y = 2.90 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_MechClass_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_MechClass AND Width = 2.10 AND Height = 1.30 AND Depth = 0.05 
          AND X = 1.10 AND Y = 1.35 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_MechClass_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_MechClass AND Width = 1.80 AND Height = 1.10 AND Depth = 0.05 
          AND X = 7.40 AND Y = 1.75 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);
DECLARE @LC_MechClass_Board3 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_MechClass AND Width = 2.00 AND Height = 1.20 AND Depth = 0.05 
          AND X = 4.00 AND Y = 0.10 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);

-- Robotics Lab (Prototyping) Components
DECLARE @LC_RoboticsLab_Projector INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_RoboticsLab AND Width = 0.55 AND Height = 0.24 AND Depth = 0.44 
          AND X = 1.40 AND Y = 2.70 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_RoboticsLab_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_RoboticsLab AND Width = 1.80 AND Height = 1.10 AND Depth = 0.05 
          AND X = 0.70 AND Y = 1.20 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);
DECLARE @LC_RoboticsLab_Board2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_RoboticsLab AND Width = 1.70 AND Height = 1.05 AND Depth = 0.05 
          AND X = 5.80 AND Y = 1.60 AND Z = 0.00 AND Orientation = 'West' AND IsDeleted = 0
);
-- Robotics Simulation Room Components
DECLARE @LC_RoboticsSimulation_Projector1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_RoboticsSimulation AND Width = 0.71 AND Height = 0.22 AND Depth = 0.78 
          AND X = 2.80 AND Y = 3.50 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_RoboticsSimulation_Projector2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_RoboticsSimulation AND Width = 0.60 AND Height = 0.30 AND Depth = 1.46 
          AND X = 5.20 AND Y = 2.30 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);

-- Drone Lab Components
DECLARE @LC_DroneLab_Projector1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_DroneLab AND Width = 0.62 AND Height = 0.30 AND Depth = 0.68 
          AND X = 1.90 AND Y = 3.50 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_DroneLab_Projector2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_DroneLab AND Width = 0.59 AND Height = 0.30 AND Depth = 0.60 
          AND X = 6.20 AND Y = 2.70 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_DroneLab_Board1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_DroneLab AND Width = 2.20 AND Height = 1.45 AND Depth = 0.05 
          AND X = 1.30 AND Y = 1.50 AND Z = 0.00 AND Orientation = 'East' AND IsDeleted = 0
);

-- Advanced Robotics Classroom Components
DECLARE @LC_AdvancedRobotics_Projector1 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_AdvancedRobotics AND Width = 0.61 AND Height = 0.28 AND Depth = 0.48 
          AND X = 1.80 AND Y = 3.00 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);
DECLARE @LC_AdvancedRobotics_Projector2 INT = (
    SELECT ComponentId FROM [Infrastructure].[LearningComponent]
    WHERE LearningSpaceId = @LS_AdvancedRobotics AND Width = 0.56 AND Height = 0.25 AND Depth = 0.45 
          AND X = 7.20 AND Y = 2.80 AND Z = 0.00 AND Orientation = 'North' AND IsDeleted = 0
);

-- Insert Projectors for Data Science Room
INSERT INTO [Infrastructure].[Projector] (ComponentId, ProjectedContent, ProjectedHeight, ProjectedWidth)
VALUES 
(@LC_DataScience_Projector1, 'Data Science Fundamentals', 2.20, 3.40),
(@LC_DataScience_Projector2, 'Machine Learning Algorithms', 2.10, 3.30),
(@LC_DeepLearning_Projector, 'Neural Networks Introduction', 2.30, 3.50),
(@LC_ComputerVision_Projector1, 'Image Recognition Systems', 2.15, 3.25),
(@LC_ComputerVision_Projector2, 'Computer Vision Algorithms', 2.00, 3.20),
(@LC_AISeminar_Projector1, 'AI Ethics and Applications', 2.40, 3.60),
(@LC_AISeminar_Projector2, 'Future of Artificial Intelligence', 2.35, 3.55),
(@LC_AIRobotics_Projector, 'Robotics AI Integration', 2.25, 3.45),
(@LC_BotLab_Projector, 'Chatbot Development', 2.10, 3.30),
(@LC_EnvManagement_Projector, 'Environmental Management Systems', 2.20, 3.40),
(@LC_BotanyLab_Projector, 'Plant Classification and Biology', 2.05, 3.25),
(@LC_EcoConference_Projector1, 'Sustainable Development Goals', 2.30, 3.50),
(@LC_EcoConference_Projector2, 'Climate Change Analysis', 2.25, 3.45),
(@LC_CleanEnergy_Projector, 'Renewable Energy Technologies', 2.15, 3.35),
(@LC_VetPathology_Projector, 'Animal Pathology Case Studies', 2.10, 3.30),
(@LC_VetRecovery_Projector, 'Veterinary Recovery Protocols', 2.20, 3.40),
(@LC_VetClass202_Projector, 'Animal Anatomy and Physiology', 2.25, 3.45),
(@LC_DiagnosticLab_Projector, 'Diagnostic Imaging Techniques', 2.15, 3.35),
(@LC_VetSimulation_Projector1, 'Surgery Simulation Practices', 2.30, 3.50),
(@LC_VetSimulation_Projector2, 'Emergency Veterinary Procedures', 2.25, 3.45),
(@LC_PowerLab_Projector, 'Electrical Power Systems', 2.10, 3.30),
(@LC_Electronics_Projector, 'Advanced Electronic Circuits', 2.20, 3.40),
(@LC_EEProject_Projector1, 'Electrical Engineering Projects', 2.30, 3.50),
(@LC_EEProject_Projector2, 'Embedded Systems Design', 2.25, 3.45),
(@LC_CNCWorkshop_Projector, 'CNC Machine Programming', 2.15, 3.35),
(@LC_ProtoClass_Projector, 'Prototyping Methodologies', 2.20, 3.40),
(@LC_3DFaceLab_Projector, '3D Modeling Techniques', 2.10, 3.30),
(@LC_IndustrialDesign_Projector1, 'Industrial Design Principles', 2.35, 3.55),
(@LC_IndustrialDesign_Projector2, 'User-centered Design', 2.30, 3.50),
(@LC_MechClass_Projector, 'Mechanical Engineering Basics', 2.20, 3.40),
(@LC_RoboticsLab_Projector, 'Robotics Programming', 2.15, 3.35),
(@LC_RoboticsSimulation_Projector1, 'Robot Motion Simulation', 2.40, 3.60),
(@LC_RoboticsSimulation_Projector2, 'Autonomous Systems', 2.35, 3.55),
(@LC_DroneLab_Projector1, 'Drone Flight Mechanics', 2.30, 3.50),
(@LC_DroneLab_Projector2, 'Aerial Imagery Analysis', 2.25, 3.45),
(@LC_AdvancedRobotics_Projector1, 'Advanced Robotics Control Systems', 2.30, 3.50),
(@LC_AdvancedRobotics_Projector2, 'Human-Robot Interaction', 2.25, 3.45);

-- Insert Whiteboards
INSERT INTO [Infrastructure].[Whiteboard] (ComponentId, MarkerColor)
VALUES 
(@LC_DataScience_Board1, 'Blue'),
(@LC_DataScience_Board2, 'Black'),
(@LC_DataScience_Board3, 'Green'),
(@LC_DataScience_Board4, 'Red'),
(@LC_DeepLearning_Board1, 'Black'),
(@LC_DeepLearning_Board2, 'Blue'),
(@LC_DeepLearning_Board3, 'Green'),
(@LC_ComputerVision_Board1, 'Black'),
(@LC_ComputerVision_Board2, 'Red'),
(@LC_ComputerVision_Board3, 'Blue'),
(@LC_AISeminar_Board1, 'Green'),
(@LC_AISeminar_Board2, 'Black'),
(@LC_AISeminar_Board3, 'Blue'),
(@LC_AISeminar_Board4, 'Red'),
(@LC_AIRobotics_Board1, 'Black'),
(@LC_AIRobotics_Board2, 'Blue'),
(@LC_AIRobotics_Board3, 'Green'),
(@LC_BotLab_Board1, 'Black'),
(@LC_BotLab_Board2, 'Red'),
(@LC_EnvManagement_Board1, 'Green'),
(@LC_EnvManagement_Board2, 'Black'),
(@LC_EnvManagement_Board3, 'Blue'),
(@LC_BotanyLab_Board1, 'Green'),
(@LC_BotanyLab_Board2, 'Black'),
(@LC_EcoConference_Board1, 'Blue'),
(@LC_EcoConference_Board2, 'Black'),
(@LC_EcoConference_Board3, 'Green'),
(@LC_CleanEnergy_Board1, 'Blue'),
(@LC_CleanEnergy_Board2, 'Black'),
(@LC_CleanEnergy_Board3, 'Green'),
(@LC_VetPathology_Board1, 'Red'),
(@LC_VetPathology_Board2, 'Black'),
(@LC_VetRecovery_Board1, 'Blue'),
(@LC_VetRecovery_Board2, 'Black'),
(@LC_VetRecovery_Board3, 'Red'),
(@LC_VetClass202_Board1, 'Green'),
(@LC_VetClass202_Board2, 'Black'),
(@LC_VetClass202_Board3, 'Blue'),
(@LC_DiagnosticLab_Board1, 'Red'),
(@LC_DiagnosticLab_Board2, 'Black'),
(@LC_VetSimulation_Board1, 'Blue'),
(@LC_VetSimulation_Board2, 'Black'),
(@LC_VetSimulation_Board3, 'Green'),
(@LC_PowerLab_Board1, 'Blue'),
(@LC_PowerLab_Board2, 'Black'),
(@LC_Electronics_Board1, 'Red'),
(@LC_Electronics_Board2, 'Black'),
(@LC_Electronics_Board3, 'Blue'),
(@LC_EEProject_Board1, 'Green'),
(@LC_EEProject_Board2, 'Black'),
(@LC_EEProject_Board3, 'Blue'),
(@LC_CNCWorkshop_Board1, 'Red'),
(@LC_CNCWorkshop_Board2, 'Black'),
(@LC_ProtoClass_Board1, 'Blue'),
(@LC_ProtoClass_Board2, 'Black'),
(@LC_ProtoClass_Board3, 'Green'),
(@LC_3DFaceLab_Board1, 'Blue'),
(@LC_3DFaceLab_Board2, 'Black'),
(@LC_IndustrialDesign_Board1, 'Red'),
(@LC_IndustrialDesign_Board2, 'Black'),
(@LC_IndustrialDesign_Board3, 'Blue'),
(@LC_IndustrialDesign_Board4, 'Green'),
(@LC_MechClass_Board1, 'Black'),
(@LC_MechClass_Board2, 'Blue'),
(@LC_MechClass_Board3, 'Green'),
(@LC_RoboticsLab_Board1, 'Red'),
(@LC_RoboticsLab_Board2, 'Black'),
(@LC_DroneLab_Board1, 'Blue');

-- UserAudit procedure
GO
CREATE PROCEDURE spInsertUserAudit
    @UserName NVARCHAR(50),
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @Phone VARCHAR(20),
    @IdentityNumber VARCHAR(20),
    @BirthDate DATE,
    @Action NVARCHAR(20)
AS
BEGIN
    INSERT INTO UserAudit (UserName, FirstName, LastName, Email, Phone, IdentityNumber, BirthDate, ModifiedAt, Action)
    VALUES (@UserName, @FirstName, @LastName, @Email, @Phone, @IdentityNumber, @BirthDate, GETUTCDATE(), @Action);
END;
GO

-- Trigger: trg_Building_Log, Trigger to log changes in the Building table
-- Logs all INSERT, UPDATE, DELETE actions on [Infrastructure].[Building] to BuildingsLog
CREATE OR ALTER TRIGGER [Infrastructure].[trg_Building_Log]
ON [Infrastructure].[Building]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Log CREATED: rows in inserted but not in deleted (true inserts)
    INSERT INTO BuildingsLog (
        [Name], [Color], [Length], [Width], [Height], [X], [Y], [Z], AreaName, [ModifiedAt], [Action]
    )
    SELECT
        i.[Name], i.[Color], i.[Length], i.[Width], i.[Height], i.[X], i.[Y], i.[Z], i.AreaName,
        GETUTCDATE(), 'CREATED'
    FROM inserted i
    LEFT JOIN deleted d ON d.BuildingInternalId = i.BuildingInternalId
    WHERE d.BuildingInternalId IS NULL;

    -- Log UPDATED: rows in both inserted and deleted (true updates)
    INSERT INTO BuildingsLog (
        [Name], [Color], [Length], [Width], [Height], [X], [Y], [Z], AreaName, [ModifiedAt], [Action]
    )
    SELECT
        i.[Name], i.[Color], i.[Length], i.[Width], i.[Height], i.[X], i.[Y], i.[Z], i.AreaName,
        GETUTCDATE(), 'UPDATED'
    FROM inserted i
    INNER JOIN deleted d ON d.BuildingInternalId = i.BuildingInternalId;

    -- Log DELETED: rows in deleted but not in inserted (true deletes)
    INSERT INTO BuildingsLog (
        [Name], [Color], [Length], [Width], [Height], [X], [Y], [Z], AreaName, [ModifiedAt], [Action]
    )
    SELECT
        d.[Name], d.[Color], d.[Length], d.[Width], d.[Height], d.[X], d.[Y], d.[Z], d.AreaName,
        GETUTCDATE(), 'DELETED'
    FROM deleted d
    LEFT JOIN inserted i ON i.BuildingInternalId = d.BuildingInternalId
    WHERE i.BuildingInternalId IS NULL;
END
GO


CREATE OR ALTER TRIGGER [Infrastructure].[trg_LearningSpace_Log]
ON [Infrastructure].[LearningSpace]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- CREATES
    INSERT INTO [Infrastructure].[LearningSpaceLog] (
        [Name], [Type], [MaxCapacity], [Width], [Height], [Length],
        [ColorFloor], [ColorWalls], [ColorCeiling], [ModifiedAt], [Action]
    )
    SELECT
        i.[Name], i.[Type], i.[MaxCapacity], i.[Width], i.[Height], i.[Length],
        i.[ColorFloor], i.[ColorWalls], i.[ColorCeiling], GETUTCDATE(), 'CREATED'
    FROM inserted i
    LEFT JOIN deleted d ON d.LearningSpaceId = i.LearningSpaceId
    WHERE d.LearningSpaceId IS NULL;

    -- UPDATES
    INSERT INTO [Infrastructure].[LearningSpaceLog] (
        [Name], [Type], [MaxCapacity], [Width], [Height], [Length],
        [ColorFloor], [ColorWalls], [ColorCeiling], [ModifiedAt], [Action]
    )
    SELECT
        i.[Name], i.[Type], i.[MaxCapacity], i.[Width], i.[Height], i.[Length],
        i.[ColorFloor], i.[ColorWalls], i.[ColorCeiling], GETUTCDATE(), 'UPDATED'
    FROM inserted i
    INNER JOIN deleted d ON d.LearningSpaceId = i.LearningSpaceId;

    -- DELETES
    INSERT INTO [Infrastructure].[LearningSpaceLog] (
        [Name], [Type], [MaxCapacity], [Width], [Height], [Length],
        [ColorFloor], [ColorWalls], [ColorCeiling], [ModifiedAt], [Action]
    )
    SELECT
        d.[Name], d.[Type], d.[MaxCapacity], d.[Width], d.[Height], d.[Length],
        d.[ColorFloor], d.[ColorWalls], d.[ColorCeiling], GETUTCDATE(), 'DELETED'
    FROM deleted d
    LEFT JOIN inserted i ON i.LearningSpaceId = d.LearningSpaceId
    WHERE i.LearningSpaceId IS NULL;
END
GO




-- Create user logs for mock data
EXEC spInsertUserAudit 'ana.morales', 'Ana', 'Morales', 'ana.morales@ucr.ac.cr', '8888-0201', '2-0001-1001', '1993-04-01', 'CREADO';
EXEC spInsertUserAudit 'bryan.castro', 'Bryan', 'Castro', 'bryan.castro@ucr.ac.cr', '8888-0202', '2-0002-1002', '1990-02-02', 'CREADO';
EXEC spInsertUserAudit 'carla.mendez', 'Carla', 'Méndez', 'carla.mendez@ucr.ac.cr', '8888-0203', '2-0003-1003', '1995-03-03', 'CREADO';
EXEC spInsertUserAudit 'david.rodriguez', 'David', 'Rodríguez', 'david.rodriguez@ucr.ac.cr', '8888-0204', '2-0004-1004', '1989-04-04', 'CREADO';
EXEC spInsertUserAudit 'elena.solis', 'Elena', 'Solís', 'elena.solis@ucr.ac.cr', '8888-0205', '2-0005-1005', '1994-05-05', 'CREADO';
EXEC spInsertUserAudit 'fabian.quesada', 'Fabián', 'Quesada', 'fabian.quesada@ucr.ac.cr', '8888-0206', '2-0006-1006', '1992-06-06', 'CREADO';
EXEC spInsertUserAudit 'gabriela.brenes', 'Gabriela', 'Brenes', 'gabriela.brenes@ucr.ac.cr', '8888-0207', '2-0007-1007', '1997-07-07', 'CREADO';
EXEC spInsertUserAudit 'hector.martinez', 'Héctor', 'Martínez', 'hector.martinez@ucr.ac.cr', '8888-0208', '2-0008-1008', '1991-08-08', 'CREADO';
EXEC spInsertUserAudit 'ivanna.murillo', 'Ivanna', 'Murillo', 'ivanna.murillo@ucr.ac.cr', '8888-0209', '2-0009-1009', '1990-09-09', 'CREADO';
EXEC spInsertUserAudit 'jose.sanchez', 'José', 'Sánchez', 'jose.sanchez@ucr.ac.cr', '8888-0210', '2-0010-1010', '1996-10-10', 'CREADO';
EXEC spInsertUserAudit 'karla.chacon', 'Karla', 'Chacón', 'karla.chacon@ucr.ac.cr', '8888-0211', '2-0011-1011', '1998-11-11', 'CREADO';
EXEC spInsertUserAudit 'luis.ramirez', 'Luis', 'Ramírez', 'luis.ramirez@ucr.ac.cr', '8888-0212', '2-0012-1012', '1993-12-12', 'CREADO';
EXEC spInsertUserAudit 'mariela.acuna', 'Mariela', 'Acuña', 'mariela.acuna@ucr.ac.cr', '8888-0213', '2-0013-1013', '1995-01-13', 'CREADO';
EXEC spInsertUserAudit 'nicolas.robles', 'Nicolás', 'Robles', 'nicolas.robles@ucr.ac.cr', '8888-0214', '2-0014-1014', '1988-02-14', 'CREADO';
EXEC spInsertUserAudit 'olga.valverde', 'Olga', 'Valverde', 'olga.valverde@ucr.ac.cr', '8888-0215', '2-0015-1015', '1991-03-15', 'CREADO';
EXEC spInsertUserAudit 'pablo.cespedes', 'Pablo', 'Céspedes', 'pablo.cespedes@ucr.ac.cr', '8888-0216', '2-0016-1016', '1990-04-16', 'CREADO';
EXEC spInsertUserAudit 'quiroz.maria', 'María', 'Quiróz', 'quiroz.maria@ucr.ac.cr', '8888-0217', '2-0017-1017', '1989-05-17', 'CREADO';
EXEC spInsertUserAudit 'ronald.venegas', 'Ronald', 'Venegas', 'ronald.venegas@ucr.ac.cr', '8888-0218', '2-0018-1018', '1994-06-18', 'CREADO';
EXEC spInsertUserAudit 'silvia.murillo', 'Silvia', 'Murillo', 'silvia.murillo@ucr.ac.cr', '8888-0219', '2-0019-1019', '1992-07-19', 'CREADO';
EXEC spInsertUserAudit 'tomas.reyes', 'Tomás', 'Reyes', 'tomas.reyes@ucr.ac.cr', '8888-0220', '2-0020-1020', '1993-08-20', 'CREADO';
EXEC spInsertUserAudit 'ursula.monge', 'Úrsula', 'Monge', 'ursula.monge@ucr.ac.cr', '8888-0221', '2-0021-1021', '1995-09-21', 'CREADO';
EXEC spInsertUserAudit 'victor.castro', 'Víctor', 'Castro', 'victor.castro@ucr.ac.cr', '8888-0222', '2-0022-1022', '1987-10-22', 'CREADO';
EXEC spInsertUserAudit 'wendy.ramirez', 'Wendy', 'Ramírez', 'wendy.ramirez@ucr.ac.cr', '8888-0223', '2-0023-1023', '1990-11-23', 'CREADO';
EXEC spInsertUserAudit 'ximena.quesada', 'Ximena', 'Quesada', 'ximena.quesada@ucr.ac.cr', '8888-0224', '2-0024-1024', '1996-12-24', 'CREADO';
EXEC spInsertUserAudit 'yolanda.corrales', 'Yolanda', 'Corrales', 'yolanda.corrales@ucr.ac.cr', '8888-0225', '2-0025-1025', '1991-01-25', 'CREADO';
EXEC spInsertUserAudit 'zandro.solis', 'Zandro', 'Solís', 'zandro.solis@ucr.ac.cr', '8888-0226', '2-0026-1026', '1988-02-26', 'CREADO';
EXEC spInsertUserAudit 'adriana.lopez', 'Adriana', 'López', 'adriana.lopez@ucr.ac.cr', '8888-0227', '2-0027-1027', '1992-03-27', 'CREADO';
EXEC spInsertUserAudit 'beto.gomez', 'Beto', 'Gómez', 'beto.gomez@ucr.ac.cr', '8888-0228', '2-0028-1028', '1990-04-28', 'CREADO';
EXEC spInsertUserAudit 'cynthia.arias', 'Cynthia', 'Arias', 'cynthia.arias@ucr.ac.cr', '8888-0229', '2-0029-1029', '1989-05-29', 'CREADO';
EXEC spInsertUserAudit 'diego.venegas', 'Diego', 'Venegas', 'diego.venegas@ucr.ac.cr', '8888-0230', '2-0030-1030', '1993-06-30', 'CREADO';
GO

CREATE OR ALTER TRIGGER [Infrastructure].[trg_Whiteboard_Log]
ON [Infrastructure].[Whiteboard]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- CREATED 
    INSERT INTO [Infrastructure].[LearningComponentAudit] (
        ComponentId, Width, Height, Depth,
        X, Y, Z, Orientation, IsDeleted,
        ComponentType, MarkerColor,
        Action, ModifiedAt
    )
    SELECT
        lc.ComponentId, lc.Width, lc.Height, lc.Depth,
        lc.X, lc.Y, lc.Z, lc.Orientation, lc.IsDeleted,
        'Whiteboard', i.MarkerColor,
        'CREATED', GETUTCDATE()
    FROM inserted i
    LEFT JOIN deleted d ON i.ComponentId = d.ComponentId
    JOIN Infrastructure.LearningComponent lc ON lc.ComponentId = i.ComponentId
    WHERE d.ComponentId IS NULL;

    -- UPDATE
    INSERT INTO [Infrastructure].[LearningComponentAudit] (
        ComponentId, Width, Height, Depth,
        X, Y, Z, Orientation, IsDeleted,
        ComponentType, MarkerColor,
        Action, ModifiedAt
    )
    SELECT
        lc.ComponentId, lc.Width, lc.Height, lc.Depth,
        lc.X, lc.Y, lc.Z, lc.Orientation, lc.IsDeleted,
        'Whiteboard', i.MarkerColor,
        'UPDATED', GETUTCDATE()
    FROM inserted i
    JOIN deleted d ON i.ComponentId = d.ComponentId
    JOIN Infrastructure.LearningComponent lc ON lc.ComponentId = i.ComponentId
    WHERE lc.IsDeleted = 0;

    -- LOGICAL DELETE
    INSERT INTO [Infrastructure].[LearningComponentAudit] (
        ComponentId, Width, Height, Depth,
        X, Y, Z, Orientation, IsDeleted,
        ComponentType, MarkerColor,
        Action, ModifiedAt
    )
    SELECT
        lc.ComponentId, lc.Width, lc.Height, lc.Depth,
        lc.X, lc.Y, lc.Z, lc.Orientation, lc.IsDeleted,
        'Whiteboard', d.MarkerColor,
        'DELETED', GETUTCDATE()
    FROM deleted d
    JOIN Infrastructure.LearningComponent lc ON lc.ComponentId = d.ComponentId
    WHERE lc.IsDeleted = 1;
END;
GO



CREATE OR ALTER TRIGGER [Infrastructure].[trg_Projector_Log]
ON [Infrastructure].[Projector]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- CREATED
    INSERT INTO [Infrastructure].[LearningComponentAudit] (
        ComponentId,Width, Height, Depth,
        X, Y, Z, Orientation, IsDeleted,
        ComponentType, ProjectedContent, ProjectedHeight, ProjectedWidth,
        Action, ModifiedAt
    )
    SELECT
        lc.ComponentId, lc.Width, lc.Height, lc.Depth,
        lc.X, lc.Y, lc.Z, lc.Orientation, lc.IsDeleted,
        'Projector', i.ProjectedContent, i.ProjectedHeight, i.ProjectedWidth,
        'CREATED', GETUTCDATE()
    FROM inserted i
    LEFT JOIN deleted d ON i.ComponentId = d.ComponentId
    JOIN Infrastructure.LearningComponent lc ON lc.ComponentId = i.ComponentId
    WHERE d.ComponentId IS NULL; 

    -- UPDATE
    INSERT INTO [Infrastructure].[LearningComponentAudit] (
        ComponentId, Width, Height, Depth,
        X, Y, Z, Orientation, IsDeleted,
        ComponentType, ProjectedContent, ProjectedHeight, ProjectedWidth,
        Action, ModifiedAt
    )
    SELECT
        lc.ComponentId, lc.Width, lc.Height, lc.Depth,
        lc.X, lc.Y, lc.Z, lc.Orientation, lc.IsDeleted,
        'Projector', i.ProjectedContent, i.ProjectedHeight, i.ProjectedWidth,
        'UPDATED', GETUTCDATE()
    FROM inserted i
    JOIN deleted d ON i.ComponentId = d.ComponentId
    JOIN Infrastructure.LearningComponent lc ON lc.ComponentId = i.ComponentId
    WHERE lc.IsDeleted = 0;


    -- LOGICAL DELETE
    INSERT INTO [Infrastructure].[LearningComponentAudit] (
        ComponentId, Width, Height, Depth,
        X, Y, Z, Orientation, IsDeleted,
        ComponentType, ProjectedContent, ProjectedHeight, ProjectedWidth,
        Action, ModifiedAt
    )
    SELECT
        lc.ComponentId, lc.Width, lc.Height, lc.Depth,
        lc.X, lc.Y, lc.Z, lc.Orientation, lc.IsDeleted,
        'Projector', d.ProjectedContent, d.ProjectedHeight, d.ProjectedWidth,
        'DELETED', GETUTCDATE()
    FROM deleted d
    JOIN Infrastructure.LearningComponent lc ON lc.ComponentId = d.ComponentId
    WHERE lc.IsDeleted = 1;

END;
GO


ALTER TABLE [Infrastructure].[LearningComponent]
    ADD DisplayId AS Infrastructure.GetComponentDisplayId(ComponentId);
