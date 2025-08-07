# Data Requirements for ThemePark@UCR

## Prequels

### Entity: Building

#### General Description

This entity represents a building within the theme park. Each building has a unique internal identifier, a unique name, and can have multiple floors.

#### Attributes

- **InternalID**: The unique identifier to each building autoincremented by the system.
- **Name**: Unique name to identify each building.
- **Color**: The color to paint the building.
- **Height**: The height of the building. Part of the dimensions of the building.
- **Length**: The length of the building. Part of the dimensions of the building.
- **Width**: The width to create the building. Part of the dimensions of the building.
- **X**: Part of the coordinates of the building. Represents the X coordinate where the building will be added.
- **Y**: Part of the coordinates of the building. Represents the Z coordinate where the building will be added.
- **Z**: Part of the coordinates of the building. Represents the Z coordinate where the building will be added.

#### Relationships

- A bulding can have several floors.
- Multiple buildings can be managed by an Administrative Unit.
- Multiple buildings can be in the same Area.

#### Restrictions

- All attribures can not be null. All of them are obligatory to add a building.
- The name has to be unique.
- The building can not exist without an Area. If it's Area is deleted, the building gets deleted too.

---

### Entity: Area

#### General Description

This entity represents an Area within a campus.

#### Attributes

- **Name**: Unique name to identify each Area.

#### Relationships

- An Area can have multiple buildings.
- Multiple Areas can belong to a Campus.

#### Restrictions

- The name has to be unique and it's obligatory.
- If the Area is deleted, all Buildings in it are deleted too.
- The Area can not exist without a Campus. If the Campus is deleted, the Area gets deleted too.

---

### Entity: Campus

#### General Description

This entity represents a Campus for a University.

#### Attributes

- **Name**: Unique name to identify each Campus.
- **Location**: Where the campus is located.

#### Relationships

- A Campus can have multiple Areas.
- Multiple Campus can belong to a University.

#### Restrictions

- The name has to be unique and it's obligatory.
- The location is obligatory.
- The campus can not exist without a University. If the University is deleted, the Campus gets deleted too.
- If the Campus is deleted, all Areas and Buildings in it are deleted too.

---

### Entity: University

#### General Description

This entity represents a University.

#### Attributes

- **Name**: Unique name to identify each university.
- **Country**: Where the university is located.

#### Relationships

- A University can have multiple Campus.
- A University can have multiple Administrative Units.

#### Restrictions

- The name has to be unique and it's obligatory.
- The country is obligatory.
- If the University is deleted, all Campus in it are deleted too.

---

### Entity: Administrative Unit	

#### General Description

This entity represents a Administrative Unit within the theme park. For example, a faculty or school. Currently not implemented in the system, but planned for future development.

#### Attributes

- **Name**: Unique name to identify each Administrative Unit.
- **Type**: What kind of Administrative Unit is, if it's a faculty, school, rectory, etc.

#### Relationships

- A Administrative Unit can be in charge of multiple Buildings.
- Multiple Administrative Units can belong to a single University.
- A Administrative Unit can be in charge of multiple Administrative Units (recursive relationship).

#### Restrictions

- The name has to be unique and it's obligatory.
- The type is obligatory.

---

## Sprinters

### Entity: Person

#### General Description

This entity represents a person within the theme park system. A person can be a student, staff member, or any other type of individual that interacts with the park. Specialized roles such as Student, Staff, and Guest inherit from this entity.

#### Attributes

- **Id**: A unique identifier for each person.
- **FirstName**: The person's first name.
- **LastName**: The person's last name.
- **Email**: The person’s personal email. Must be unique.
- **IdentityNumber**: National identification number. Must be unique.
- **BirthDate**: The date of birth of the person.
- **Phone**: An optional national phone number for contacting.

#### Relationships

- A person is the superclass of Student, Staff, and Guest (specialization/inheritance).
- A person can be linked to one User account. (one-to-one)

#### Restrictions

- All attributes except Phone are mandatory.
- Email and IdentityNumber must be unique.
- BirthDate must represent a valid past date.

---

### Entity: User

#### General Description

This entity represents a user account used to log into the system.

#### Attributes

- **Id**: Unique identifier for the account.
- **UserName**: Unique login username.
- **PersonId**: References the person (also serves as foreign key).

#### Relationships

- Each User is linked to exactly one Person. (one to one)
- Connected to one or more Roles. (many-to-many)
- Inherits permissions through its Roles.

#### Restrictions

- UserName must be unique and required.
- A Person can have only one User account.

---

### Entity: Role

#### General Description

Represents a role a user can have in the system (e.g., "Student", "Staff", "Admin").

#### Attributes

- **Id**: Unique identifier for the role.
- **Name**: Role name. Must be unique.

#### Relationships

- A Role can have many Permissions. (many-to-many)
- A Role can be assigned to many Users. (many-to-many)

#### Restrictions

- Name is required and must be unique.

---

### Entity: Permission

#### General Description

Represents a specific permission granted to a role (e.g., "Access to ...").

#### Attributes

- **Id**: Unique identifier for the permission.
- **Description**: Explanation of the permission.

#### Relationships

- Multiple permissions may be assigned to many Roles. (many-to-many)

#### Restrictions

- Description is required.

---

### Entity: Student

#### General Description

Specialized entity that represents a person who is a student.

#### Attributes

- **PersonId**: References the person (also serves as foreign and primary key).
- **StudentId**: Unique identifier assigned to the student.
- **InstitutionalEmail**: The student's institutional email. Must be unique.

#### Relationships

- Subclass of Person

#### Restrictions

- PersonId must exist in Person and is unique.
- StudentId is required and must be unique across all students.
- InstitutionalEmail is unique and required.

---

### Entity: Staff

#### General Description

Specialized entity that represents a person who is a staff member.

#### Attributes

- **PersonId**: References the person (also serves as foreign and primary key).
- **StaffType**: A predefined code identifying the staff role (e.g., “ADM”, “DOC”, “ADMDOC”).
- **InstitutionalEmail**: Staff member's institutional email. Must be unique.

#### Relationships

- Subclass of Person

#### Restrictions

- PersonId must exist in Person.
- InstitutionalEmail is required and unique.
- StaffType must be one of: 'ADM', 'DOC', 'ADMDOC'. (Will change in the future)

---

### Entity: Guest

#### General Description

Specialized entity representing a guest who is not classified as student or staff.

#### Attributes

- **PersonId**: References the person (also serves as foreign and primary key).

#### Relationships

- Subclass of Person

#### Restrictions

- PersonId must exist in Person.

---

### Entity: UserRole

#### General Description

Junction table to connect Users to Roles.

#### Attributes

- **UserId**: References the user account.
- **RoleId**: References the role.

#### Relationships

- Many-to-many between User and Role.

#### Restrictions

- Combination of UserId and RoleId must be unique.

---

### Entity: RolePermission

#### General Description

Junction table to connect Roles to Permissions.

#### Attributes

- **RoleId**: References the role.
- **PermissionId**: References the permission.

#### Relationships

- Many-to-many between Role and Permission.

#### Restrictions

- Combination of RoleId and PermissionId must be unique.

---

## SQLit's

### Entity: Floor

#### General Description

Represents the different floors of a building.

#### Attributes

- **FloorId**: Unique internal identifier for the floor. It is a foreign key and cannot be null.

#### Relationships

- The entity has two weak relationships:
- Each floor must belong to a single building.
- Each floor can contain multiple learning spaces.

#### Restrictions

- The floor_ID must be unique and not null.

---
### Entity: Learning Space

#### General Description

Represents the various classrooms or learning spaces that may exist within a university. It is the location where university learning sessions take place.

#### Attributes

- **Type**: Type of learning space. Possible values include: Classroom, Auditorium, Laboratory.
- **Length**: Physical length of the space.
- **Width**: Physical width of the space.
- **Height**: Physical height of the space.
- **CeilingColor**: Color and pattern of the ceiling.
- **WallColor**: Color and pattern of the walls.
- **FloorColor**: Color and pattern of the floor.
- **LearningSpaceId**: Unique internal identifier for the learning space, that goes secuential (1,2,3,..). It serves as a foreign key.
- **Name**: Attribute to identify the learning space don't have a pattern.

#### Relationships

- All relationships are logical weak and stem from weak entities.
- A learning space must belong to one and only one floor.
- A learning space may contain multiple learning components.

#### Restrictions

- The ID must be unique, not null, and mandatory.
- Dimension attributes (length, width, height) must be positive, non-zero, and not null.
- The colour must be a valid color name type like (Red,Green,Blue,...).
- The type must be one of the following values: Classroom, Auditorium, Laboratory; it cannot be null.
- The maximum capacity must be greater than 1 and not null.
---
## #Cprendió++

### Entity: Learning Component

#### General Description

This entity represents a physical component located within a learning space in the theme park. Examples include projectors and whiteboards.

#### Attributes

- **ComponentId**: The unique identifier to each learning component autoincremented by the system.
- **LearningSpaceId**: The learning space where the component is located in. Foreign key.
- **Width**: The width of the learning component. Part of the dimensions.
- **Height**: The height of the learning component. Part of the dimensions.
- **Depth**: The depth of the learning component. Part of the dimensions.
- **X**: The x cooordinate where the learning component is placed. Part of the position. 
- **Y**: The y cooordinate where the learning component is placed. Part of the position.
- **Z**:The z cooordinate where the learning component is placed. Part of the position.
- **Orientation**: Orientation of the component in the space.

#### Relationships

- Each learning component belongs to one learning space. 
- Multiple learning components can exist in one learning space.

#### Restrictions

- The LearningSpaceId must reference a valid existing LearningSpace.
- Position (X,Y,Z) and dimensions (Width, Height, Depth) must be non-negative.
- Orientation must be one of the following values: "North", "South", "East", "West".

---

### Entity: Projector

#### General Description

This entity represents a projector as specialized type of learning component located within a learning space. It inherits attributes from LearningComponent.

#### Attributes

- **ComponentID**: The unique identifier of the component. Foreign key.
- **projectedContent**: The content that will be projected.
- **projectedWidth**: The width of the projected content.
- **projectedHeight**: The height of the projected content. 

#### Relationships

- Each projector is a subtype of a LearningComponent.
- It is associated with one learning space through its base component.
- Multiple learning components can exist in one learning space.

#### Restrictions

- ComponentID must reference an exisiting LearningComponent.

---


### Entity: Whiteboard

#### General Description
This entity represents a whiteboard as specialized type of learning component located within a learning space. It inherits attributes from LearningComponent.

#### Attributes

- **ComponentID**: The unique identifier of the component. Foreign key.
- **markerColor**: The color of the marker that will be used in the whiteboard.

#### Relationships

- Each whiteboard is a subtype of a LearningComponent.
- It is associated with one learning space through its base component.
- Multiple learning components can exist in one learning space.

#### Restrictions

- ComponentID must reference an existing LearningComponent.
- MarkerColor is required and should represent a valid color name.
