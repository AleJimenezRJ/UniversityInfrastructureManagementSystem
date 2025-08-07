using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure;

/// <summary>
/// This class represents the database context, serving as the primar point of 
/// interaction with the underlying database through Entity Framework Core.
/// </summary>
internal class ThemeParkDataBaseContext : DbContext
{

    /// <summary>
    /// Gets or sets the DbSet representing the Buildings table in the database.
    /// This property provides access to query and save instances of <see cref="Building"/> entities.
    /// </summary>
    public virtual DbSet<Building> Buildings { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the Area table in the database.
    /// This property provides access to query and save instances of <see cref="Area"/> entities.
    /// </summary>
    public virtual DbSet<Area> Area { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the Area table in the database.
    /// This property provides access to query and save instances of <see cref="Area"/> entities.
    /// </summary>
    public virtual DbSet<Campus> Campus { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the University table in the database.
    /// This property provides access to query and save instances of <see cref="University"/> entities.
    /// </summary>
    public virtual DbSet<University> University { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the BuildingLog table in the database.
    /// </summary>
    public virtual DbSet<BuildingLog> BuildingLog { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the Floors table in the database.
    /// </summary>
    public virtual DbSet<Floor> Floors { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the LearningSpaces table in the database.
    /// </summary>
    public virtual DbSet<LearningSpace> LearningSpaces { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the LearningSpaceLog table in the database.    
    /// </summary>
    public virtual DbSet<LearningSpaceLog> LearningSpaceLog { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the LearningComponents table in the database.
    /// </summary>
    public virtual DbSet<LearningComponent> LearningComponents { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the Projectors table in the database.
    /// </summary>
    public virtual DbSet<Projector> Projectors { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the Whiteboards table in the database.
    /// </summary>
    public virtual DbSet<Whiteboard> Whiteboards { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the Person table in the database.
    /// </summary>
    public virtual DbSet<Person> Persons { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the UserAccount table in the database.
    /// </summary>
    public virtual DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the UserRoles table in the database.
    /// </summary>
    public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the Roles table in the database.
    /// </summary>
    public virtual DbSet<Role> Roles { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the RolePermission table in the database.
    /// This property provides access to query and save instances of <see cref="RolePermission"/> entities,
    /// which define the association between roles and permissions.
    /// </summary>
    public virtual DbSet<RolePermission> RolePermissions { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the Permission table in the database.
    /// This property provides access to query and save instances of <see cref="Permission"/> entities,
    /// which represent the permissions that can be assigned to roles.
    /// </summary>
    public virtual DbSet<Permission> Permissions { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the combined User-Person view in the database.
    /// This property provides access to query user records with their associated person information,
    /// including user credentials and personal details in a single entity.
    /// </summary>
    public virtual DbSet<UserWithPerson> UsersWithPerson { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the Student table in the database.
    /// This property provides access to query and save instances of <see cref="Student"/> entities,
    /// which extend the <see cref="Person"/> entity with student-specific information,
    /// including institutional email and student ID.
    /// </summary>
    public virtual DbSet<Student> Students {  get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the Staff table in the database.
    /// This property provides access to query and save instances of <see cref="Staff"/> entities,
    /// </summary>
    public virtual DbSet<Staff> Staff { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the UserAudit table in the database.
    /// </summary>
    public virtual DbSet<UserAudit> UserAudits { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet representing the LearningComponentAudit table in the database.
    /// </summary>
    public virtual DbSet<LearningComponentAudit> LearningComponentAudits { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeParkDataBaseContext"/> class.
    /// This constructor accepts configuration options that define how the context connects to the database.
    /// </summary>
    /// <param name="options">
    /// Configuration options for this db context, including connection string,
    /// provider information, and behavior settings.
    /// </param>
    public ThemeParkDataBaseContext(DbContextOptions<ThemeParkDataBaseContext> options) : base(options)
    {
    }

    /// <summary>
    /// Configures the model from the entity types
    /// </summary>
    /// <param name="modelBuilder">
    /// The builder being used to construct the model for this context.
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        // Apply entity configurations from separate configuration classes
        modelBuilder.ApplyConfiguration(new BuildingEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UniversityEntityConfiguration());
        modelBuilder.ApplyConfiguration(new AreaEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CampusEntityConfiguration());
        modelBuilder.ApplyConfiguration(new FloorEntityConfiguration());
        modelBuilder.ApplyConfiguration(new LearningSpaceEntityConfiguration());
        modelBuilder.ApplyConfiguration(new LearningSpaceLogEntityConfiguration());
        modelBuilder.ApplyConfiguration(new LearningComponentEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectorEntityConfiguration());
        modelBuilder.ApplyConfiguration(new WhiteboardEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PersonEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RoleEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RolePermissionEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionEntityConfiguration());
        modelBuilder.ApplyConfiguration(new StudentEntityConfiguration());
        modelBuilder.ApplyConfiguration(new StaffEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserAuditEntityConfiguration());
        modelBuilder.ApplyConfiguration(new BuildingLogEntityConfiguration());
        modelBuilder.ApplyConfiguration(new LearningComponentAuditEntityConfiguration());
    }

    [Obsolete("Only meant to be used by Moq library. Do not use in code.")]
    internal ThemeParkDataBaseContext() 
    {
        // This constructor is only for mocking purposes and should not be used in production code.
        // It allows the creation of a ThemeParkDataBaseContext instance without any configuration.
    }
}
