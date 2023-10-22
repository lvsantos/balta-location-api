using Baltaio.Location.Api.Domain;
using Baltaio.Location.Api.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Baltaio.Location.Api.Infrastructure.Persistance;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<State> States { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
            .HasKey(c => c.Code);
        modelBuilder.Entity<City>()
            .HasOne(c => c.State)
            .WithMany()
            .HasForeignKey(c => c.StateCode)
            .IsRequired();
        
        modelBuilder.Entity<State>().HasKey(s => s.Code);
        
        modelBuilder.Entity<User>().HasKey(u => u.Code);
    }
}

