using Microsoft.EntityFrameworkCore;
using SSTAlumniAssociation.Api.Entities;

namespace SSTAlumniAssociation.Api.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Users
    public DbSet<Entities.User> Users { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<AlumniMember> AlumniMembers { get; set; }
    public DbSet<EmployeeMember> EmployeeMembers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<ServiceAccount> ServiceAccounts { get; set; }
    public DbSet<SystemAdmin> SystemAdmins { get; set; }

    public DbSet<Entities.Event> Events { get; set; }
    public DbSet<UserEvent> UserEvents { get; set; }

    public DbSet<Entities.Article> Articles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Store enum value as string to reduce risk of breaking changes vs storing as an int.
        modelBuilder
            .Entity<Member>()
            .Property(u => u.Membership)
            .HasConversion<string>();

        modelBuilder
            .Entity<AlumniMember>()
            .Property(a => a.GraduationYear)
            .HasColumnName("GraduationYear");

        modelBuilder
            .Entity<EmployeeMember>()
            .Property(e => e.GraduationYear)
            .HasColumnName("GraduationYear");

        modelBuilder.Entity<SystemAdmin>()
            .HasData(
                new SystemAdmin(
                    id: Guid.Parse("df90f5ea-a236-413f-a6c1-ca9197427631"),
                    name: "Qin Guan",
                    firebaseId: "GuZZVeOdlhNsf5dZGQmU2yV1Ox33",
                    email: "qinguan20040914@gmail.com"
                )
            );

        modelBuilder.Entity<AlumniMember>()
            .HasData(
                new AlumniMember(
                    id: Guid.Parse("829bc4dc-2d8f-46df-acbb-c52c0e7f958f"),
                    name: "Tan Zheng Jie",
                    firebaseId: "5ZPERFPTvfMfxwhH7SGsOmXqSco2",
                    email: "tan_zheng_jie@sstaa.org",
                    membership: Membership.Exco,
                    memberId: "EXCO-1"
                )
            );

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}