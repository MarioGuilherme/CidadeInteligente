using CidadeInteligente.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CidadeInteligente.Infrastructure.Persistence;

public class CidadeInteligenteDbContext(DbContextOptions<CidadeInteligenteDbContext> options) : DbContext(options) {
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Media> Medias { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.EnableSensitiveDataLogging();
}