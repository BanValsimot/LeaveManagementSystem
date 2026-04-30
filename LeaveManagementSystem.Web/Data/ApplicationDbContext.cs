using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Data;

// Custom database context for the application -> inheritance makes this class a Database Model
// We inherit from IdentityDbContext to include all Identity (users, roles, etc.) tables
// <ApplicationUser> tells Identity to use our custom user class instead of the default IdentityUser
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    // Constructor receives configuration (like connection string) via Dependency Injection
    // DbContextOptions contains settings for connecting to the database
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) // Pass those settings to the base DbContext class (Inherited class)
    {
    }

    //Insert a Table into a Database:
    //Set of collections of a certain Type
    //Data Type + Name of the Table
    public DbSet<LeaveType> LeaveTypes { get; set; }

    // This method is used to configure entity types and seed initial data
    // It is called when EF Core builds the model (during migrations)
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // VERY IMPORTANT: call base method so Identity can configure its own entity types (tables)
        // Builder object -> builds seeded data
        base.OnModelCreating(builder);

        // ============================
        // SEED ROLES (AspNetRoles)
        // ============================
        // IdentityRole = entity class -> provided by ASP.NET Identity — you do NOT create it yourself.
        // AspNetRoles = database table
        // HasData() = insert initial records during migration

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                // Primary Key (must be hardcoded for seeding)
                // GUID values: https://guidgenerator.com/
                Id = "363b0dc6-6a9f-4aff-8e58-17d5bad0abed",

                // Role name (used in code)
                Name = "Employee",

                // Normalized name (uppercase, used internally by Identity for comparisons)
                NormalizedName = "EMPLOYEE"
            },
            new IdentityRole
            {
                Id = "9f89515a-0c92-49d7-887a-9d8c537c3ab3",
                Name = "Supervisor",
                NormalizedName = "SUPERVISOR"
            },
            new IdentityRole
            {
                Id = "40fb5ecf-a2a8-452c-9ad4-baaafcc4725c",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            });

        // ============================
        // CREATE PASSWORD HASHER
        // ============================
        // Passwords are NEVER stored as plain text
        // PasswordHasher converts password into a secure hashed value

        var hasher = new PasswordHasher<ApplicationUser>();

        // ============================
        // SEED DEFAULT USER (AspNetUsers) -> ADMINISTRATOR
        // ============================
        // ApplicationUser = entity class (inherits from IdentityUser)
        // IdentityUser = original Entity Class
        // AspNetUsers = database table

        builder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                // Primary Key (must match later in UserRole mapping)
                Id = "56f05747-4972-49dd-8d67-941fdcc08825",

                // Email and username
                Email = "admin@localhost.com",
                UserName = "admin@localhost.com",

                // Normalized values (required by Identity for lookups)
                NormalizedEmail = "ADMIN@LOCALHOST.COM",
                NormalizedUserName = "ADMIN@LOCALHOST.COM",

                // Password must be stored as a HASH (not plain text)
                // First parameter = user object (used internally by Identity for hashing logic)
                // Here it is set to null because we do not need user-specific hashing behavior
                // (e.g., no custom salts or user-dependent data are used)
                // This is acceptable for seeding a default user,
                // but normally you would pass the actual user instance
                PasswordHash = hasher.HashPassword(null, "Satouf94!"),

                // Skip email confirmation step for seeded user
                EmailConfirmed = true,

                // SecurityStamp = unique value used by Identity to track security changes
                // If user data changes (e.g., password), this value changes and invalidates old logins (forces logout)
                SecurityStamp = Guid.NewGuid().ToString(),

                // Custom properties from ApplicationUser
                FirstName = "Default",
                LastName = "Default",
                DateOfBirth = new DateOnly(1986, 11, 7)
            });

        // ============================
        // SEED USER-ROLE RELATIONSHIP (AspNetUserRoles)
        // ============================
        // IdentityUserRole = join entity type (many-to-many relationship)
        // AspNetUserRoles = join table

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                // RoleId must match Administrator role above
                RoleId = "40fb5ecf-a2a8-452c-9ad4-baaafcc4725c",

                // UserId must match seeded admin user above
                UserId = "56f05747-4972-49dd-8d67-941fdcc08825"
            });
    }
}
