using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Entities.User> Users { get; set;  }
    public DbSet<Event> Events { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<UserEvent> UserEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
