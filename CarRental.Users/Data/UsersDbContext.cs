using CarRental.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Users.Data;

public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<UserRoleAssignment> UserRoleAssignments =>
        Set<UserRoleAssignment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Ignore(u => u.Roles);

            entity.HasMany(u => u.RoleAssignments)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserRoleAssignment>(entity =>
        {
            entity.HasKey(r => new { r.UserId, r.Role });

            entity.Property(r => r.Role)
                .HasConversion<int>();
        });
    }
}
